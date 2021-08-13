using EPRO.Infrastructure.Contracts;
using EPRO.Infrastructure.Data.Common;
using EPRO.Infrastructure.Data.Models.Common;
using EPRO.Infrastructure.ViewModels.Cdn;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPRO.Infrastructure.Services
{
    public class CdnService : ICdnService
    {
        protected readonly IGridFSBucket gridFsBucket;
        protected readonly IRepository repo;
        public CdnService(
            IRepository _repo,
            IConfiguration _config,
            IMongoClient mongoClient)
        {
            repo = _repo;
            string fileDbName = _config.GetValue<string>("FileDbName");
            var database = mongoClient.GetDatabase(fileDbName);
            gridFsBucket = new GridFSBucket(database);
        }
        public IEnumerable<CdnItemVM> Select(int sourceType, string sourceId, string fileId = null)
        {
            int[] sourceTypes = new List<int>(){
                sourceType
            }.ToArray();

            return Select(sourceTypes, sourceId, fileId);
        }
        public IEnumerable<CdnItemVM> Select(int[] sourceTypes, string sourceId, string fileId = null)
        {
            if (!string.IsNullOrEmpty(fileId))
            {
                return repo.AllReadonly<MongoFile>()
                                .Where(x => x.FileId == fileId)
                                .Select(x => new CdnItemVM
                                {
                                    MongoFileId = x.Id,
                                    SourceType = x.SourceType,
                                    SourceId = x.SourceId,
                                    FileId = x.FileId,
                                    Title = x.Title ?? x.FileName,
                                    FileName = x.FileName,
                                    UserUploaded = x.UserId,
                                    DateUploaded = x.DateWrt,
                                    DateExpired = x.DateExpired,
                                    FileSize = x.FileSize
                                }).OrderBy(x => x.DateUploaded);
            }
            else
            {
                return repo.AllReadonly<MongoFile>()
                                .Where(x => x.SourceId == sourceId && sourceTypes.Contains(x.SourceType))
                                .Select(x => new CdnItemVM
                                {
                                    MongoFileId = x.Id,
                                    SourceType = x.SourceType,
                                    SourceId = x.SourceId,
                                    FileId = x.FileId,
                                    Title = x.Title ?? x.FileName,
                                    FileName = x.FileName,
                                    UserUploaded = x.UserId,
                                    DateUploaded = x.DateWrt,
                                    DateExpired = x.DateExpired,
                                    FileSize = x.FileSize
                                }).OrderBy(x => x.DateUploaded);

            }
        }
        public async virtual Task<CdnDownloadResult> GetFileById(string fileId)
        {
            using (var file = await gridFsBucket.OpenDownloadStreamAsync(ObjectId.Parse(fileId)))
            {
                byte[] fileContent = new byte[(int)file.Length];
                file.Read(fileContent, 0, (int)file.Length);


                CdnDownloadResult result = new CdnDownloadResult()
                {
                    FileId = fileId,
                    ContentType = file.FileInfo.Metadata.GetValue("contentType").AsString,
                    FileName = file.FileInfo.Filename,
                    FileContentBase64 = Convert.ToBase64String(fileContent)
                };

                await file.CloseAsync();

                return result;
            }
        }

        public async Task<bool> MongoCdn_DeleteFiles(CdnFileSelect request)
        {
            bool result = true;
            var selectedFiles = Select(request.SourceType, request.SourceId, request.FileId);

            foreach (var _file in selectedFiles)
            {
                result &= await MongoCdn_DeleteFile(_file.FileId);
            }

            return result;
        }

        public async Task<CdnUploadResult> MongoCdn_UploadFile(CdnUploadRequest request)
        {
            CdnUploadResult result = new CdnUploadResult() { Succeded = false };
            Dictionary<string, object> metadata = new Dictionary<string, object>();
            metadata.Add("contentType", request.ContentType);
            metadata.Add("sourceType", request.SourceType);
            metadata.Add("sourceId", request.SourceId);
            metadata.Add("title", request.Title);

            var options = new GridFSUploadOptions
            {
                Metadata = new BsonDocument(metadata)
            };

            if (!string.IsNullOrEmpty(request.FileContentBase64) && request.FileContent == null)
            {
                request.FileContent = Convert.FromBase64String(request.FileContentBase64);
            }

            try
            {
                string mongoFileId = (await gridFsBucket.UploadFromBytesAsync(request.FileName, request.FileContent, options)).ToString();

                if (!string.IsNullOrEmpty(mongoFileId))
                {
                    result.Succeded = await SaveMongoFileData(request, mongoFileId);
                }

                result.FileId = mongoFileId;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }

            return result;
        }


        public async Task<bool> MongoCdn_DeleteFile(string id)
        {
            await gridFsBucket.DeleteAsync(ObjectId.Parse(id));

            return DeleteMongoFileData(id);
        }

        public bool DeleteMongoFileData(string mongoFileId)
        {
            var saved = repo.All<MongoFile>(x => x.FileId == mongoFileId).FirstOrDefault();

            if (saved != null)
            {
                repo.Delete(saved);
                repo.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<bool> SaveMongoFileData(CdnUploadRequest file, string mongoFileId)
        {
            try
            {
                var mongoFile = new MongoFile()
                {
                    FileId = mongoFileId,
                    SourceType = file.SourceType,
                    SourceId = file.SourceId,
                    Title = file.Title,
                    FileSize = file.FileContent.Length,
                    FileName = file.FileName,
                    UserId = file.UserUploaded,
                    DateWrt = DateTime.Now
                };


                await repo.AddAsync(mongoFile);
                await repo.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<CdnDownloadResult> MongoCdn_Download(int mongoFileId)
        {
            string fileId = repo.AllReadonly<MongoFile>().Where(x => x.Id == mongoFileId).Select(x => x.FileId).FirstOrDefault();

            return await MongoCdn_Download(fileId);
        }

        public async Task<CdnDownloadResult> MongoCdn_Download(string fileId)
        {
            return await MongoCdn_Download(new CdnFileSelect() { FileId = fileId });
        }

        public async Task<CdnDownloadResult> MongoCdn_Download(CdnFileSelect request)
        {
            string fileId = request.FileId;
            string title = String.Empty;
            var fileItem = Select(request.SourceType, request.SourceId, request.FileId).FirstOrDefault();
            if (fileItem == null)
            {
                return null;
            }
            title = fileItem.Title;


            CdnDownloadResult downloadInfo = await GetFileById(fileItem);
            downloadInfo.FileTitle = title;

            return downloadInfo;
        }

        private async Task<CdnDownloadResult> GetFileById(CdnItemVM fileItem)
        {
            using (var file = await gridFsBucket.OpenDownloadStreamAsync(ObjectId.Parse(fileItem.FileId)))
            {
                byte[] fileContent = new byte[(int)file.Length];
                file.Read(fileContent, 0, (int)file.Length);
                byte[] newContent = fileContent;


                CdnDownloadResult result = new CdnDownloadResult()
                {
                    FileId = fileItem.FileId,
                    ContentType = file.FileInfo.Metadata.GetValue("contentType").AsString,
                    FileName = file.FileInfo.Filename,
                    FileContentBase64 = Convert.ToBase64String(newContent)
                };

                await file.CloseAsync();
                return result;
            }
        }


        public async Task<bool> MongoCdn_AppendUpdate(CdnUploadRequest request)
        {
            var files = Select(request.SourceType, request.SourceId).ToList();
            foreach (var file in files)
            {
                DeleteMongoFileData(file.FileId);
            }
            var uploadResult = await MongoCdn_UploadFile(request);
            if (uploadResult.Succeded)
            {
                request.FileId = uploadResult.FileId;
            }

            return !string.IsNullOrEmpty(uploadResult.FileId);
        }
    }
}
