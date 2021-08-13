using EPRO.Infrastructure.ViewModels.Cdn;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPRO.Infrastructure.Contracts
{
    public interface ICdnService
    {
        Task<CdnUploadResult> MongoCdn_UploadFile(CdnUploadRequest request);

        Task<bool> MongoCdn_DeleteFile(string id);
        Task<bool> MongoCdn_DeleteFiles(CdnFileSelect request);
        Task<CdnDownloadResult> MongoCdn_Download(int mongoFileId);
        Task<CdnDownloadResult> MongoCdn_Download(string fileId);
        Task<CdnDownloadResult> MongoCdn_Download(CdnFileSelect request);

        Task<CdnDownloadResult> GetFileById(string id);        

        Task<bool> MongoCdn_AppendUpdate(CdnUploadRequest request);

        Task<bool> SaveMongoFileData(CdnUploadRequest file, string mongoFileId);
        bool DeleteMongoFileData(string mongoFileId);

        IEnumerable<CdnItemVM> Select(int sourceType, string sourceId, string fileId = null);
        IEnumerable<CdnItemVM> Select(int[] sourceTypes, string sourceId, string fileId = null);
    }
}
