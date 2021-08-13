using EPRO.Core.Contracts;
using EPRO.Infrastructure.Constants;
using EPRO.Infrastructure.Contracts;
using EPRO.Infrastructure.Data.Common;
using EPRO.Infrastructure.Data.DataContracts;
using EPRO.Infrastructure.Data.Models.Common;
using EPRO.Infrastructure.Data.Models.Nomenclatures;
using EPRO.Infrastructure.ViewModels.Cdn;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using static EPRO.Infrastructure.Constants.NomenclatureConstants;

namespace EPRO.Core.Services
{
    public class ApiService : BaseService, IApiService
    {
        protected ICdnService cdnService;
        private readonly INomenclatureService nomService;
        public ApiService(
            IRepository _repo,
            INomenclatureService _nomService,
            ILogger<ApiService> _logger,
            IUserContext _userContext,
            ICdnService _cdnService)
        {
            repo = _repo;
            logger = _logger;
            nomService = _nomService;
            userContext = _userContext;
            cdnService = _cdnService;
        }

        public bool DismissalID_IfExists(string dismissalID)
        {
            Dismissal entity = repo.AllReadonly<Dismissal>()
                .Where(n => n.Id == dismissalID).FirstOrDefault();

            if (entity != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetNomenclatureIDByCode<T>(string Code) where T : BaseCommonNomenclature
        {
            T NomenclatureEntity = repo.AllReadonly<T>()
                .Where(n => n.Code == Code).FirstOrDefault();

            int result = -1;
            if (NomenclatureEntity != null)
            {
                result = NomenclatureEntity.Id;
            }

            return result;
        }

        //public bool ValidateInsertRequirements(DismissalRegistrationRequest model, DismissalRegistrationResponse response)
        //{
        //    return true;
        //}

        public bool ValidateDataConditions(DismissalRegistrationRequest model, ErrorModel Error)
        {
            //CDR01, CDR02 from API specifications

            bool Success = true;

            //CDR01
            if (model.DismissalType == "1"
                && model.ObjectionUpheld == null)
            //currently impossible, ObjectionUpheld is bool type?
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "ObjectionUpheld";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "'ObjectionUpheld' е задължителен в текущия случай.";
                return Success;
            }

            //CDR02
            if (model.DismissalType == "1")
            {
                if (model.Objection.DocumentDate == null ||
                    model.Objection.DocumentNumber == null ||
                    model.Objection.DocumentType == null ||
                    model.Objection.SideInvolmentKind == null ||
                    model.Objection.SideName == null)
                {
                    Success = false;
                    Error.ErrorType = "E099";
                    Error.FaultyAttribute = "Objection";
                    Error.Reason = "Некоректно подаден параметър";
                    Error.Description = "Детайлите за 'Искане за отвод' са задължителни в текущия случай.";
                    return Success;
                }
            }


            return Success;
        }

        public bool ValidateUpdateData(DismissalRegistrationRequest model, ErrorModel Error)
        {
            bool Success = true;

            #region Dismissal

            if (!ValidateDismissalGUID((Guid)model.DismissalId))
            {
                Success = false;
                Error.ErrorType = "E002";
                Error.FaultyAttribute = "DismissalId";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Грешен идентификатор на отвод.";
                return Success;
            }

            #endregion

            if (!ValidateInsertData(model, Error))
            {
                Success = false;

                return Success;
            }

            return Success;
        }

        public bool ValidateInsertData(DismissalRegistrationRequest model, ErrorModel Error)
        {
            bool Success = true;

            //Court
            #region Court
            if (model.Court.Length < 1 || model.Court.Length > 255)
            {
                Success = false;
                Error.ErrorType = "E001";
                Error.FaultyAttribute = "Court";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна дължина на параметър. Изисква се дължина от 1 до 255 символа.";
                return Success;
            }

            if (!model.Court.All(Char.IsLetterOrDigit))
            {
                Success = false;
                Error.ErrorType = "E001";
                Error.FaultyAttribute = "Court";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна стойност за параметър.";
                return Success;
            }

            int CourtID = GetNomenclatureIDByCode<Court>(model.Court);

            if (CourtID == -1)
            {
                Success = false;
                Error.ErrorType = "E001";
                Error.FaultyAttribute = "Court";
                Error.Reason = "Некоректно подаден параметър";
                return Success;
            }
            #endregion
            #region CaseType

            if (model.CaseType.Length > 255)
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "CaseType";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна дължина на параметър. Изисква се дължина от до 255 символа.";
                return Success;
            }

            if (!model.Court.All(Char.IsLetterOrDigit))
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "CaseType";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна стойност за параметър.";
                return Success;
            }

            int CaseTypeID = GetNomenclatureIDByCode<CaseType>(model.CaseType);

            if (CaseTypeID == -1)
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "CaseType";
                Error.Reason = "Некоректно подаден параметър";
                return Success;
            }

            #endregion

            #region CaseNumber

            if (!model.CaseNumber.All(Char.IsDigit))
            {
                Success = false;
                Error.ErrorType = "E003";
                Error.FaultyAttribute = "CaseNumber";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна стойност за параметър.";
                return Success;
            }

            if (model.CaseNumber?.Length != 14)
            {
                Success = false;
                Error.ErrorType = "E003";
                Error.FaultyAttribute = "CaseNumber";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна дължина на параметър. Изисква се дължина от 14 символа.";
                return Success;
            }

            if (!nomService.ValidateCaseNumber(model.CaseNumber, CourtID, model.CaseYear))
            {
                Success = false;
                Error.ErrorType = "E003";
                Error.FaultyAttribute = "CaseNumber";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалиден формат на параметър.";
                return Success;
            }

            #endregion

            #region CaseYear

            if (model.CaseYear.ToString().Length != 4)
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "CaseYear";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна стойност за параметър.";
                return Success;
            }

            if (!model.CaseYear.ToString().All(Char.IsDigit))
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "CaseYear";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна стойност за параметър.";
                return Success;
            }

            //Валидиране на номер на дело
            //if (CourtID > 0)
            //{
            //    if (!model.CaseNumber.StartsWith($"{model.CaseYear}{model.Court}"))
            //    {
            //        Success = false;
            //        Error.ErrorType = "E099";
            //        Error.FaultyAttribute = "CaseNumber";
            //        Error.Reason = "Некоректно подаден параметър";
            //        Error.Description = "Невалидна стойност за параметър.";
            //        return Success;
            //    }
            //}

            #endregion

            #region DismissalType
            if (model.DismissalType.Length > 255)
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "DismissalType";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна дължина на параметър. Изисква се дължина до 255 символа.";
                return Success;
            }

            if (!model.DismissalType.All(Char.IsDigit))
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "DismissalType";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна стойност за параметър.";
                return Success;
            }

            int DismissalType = GetNomenclatureIDByCode<DismissalType>(model.DismissalType);

            if (DismissalType == -1)
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "DismissalType";
                Error.Reason = "Некоректно подаден параметър";
                return Success;
            }
            #endregion

            #region DismissalReason
            if (model.DismissalReason.Length > 4000)
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "DismissalReason";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна дължина на параметър. Изисква се дължина до 4000 символа.";
                return Success;
            }

            //if (!model.DismissalReason.All(Char.IsLetterOrDigit))
            //{
            //    Success = false;
            //    Error.ErrorType = "E099";
            //    Error.FaultyAttribute = "DismissalReason";
            //    Error.Reason = "Некоректно подаден параметър";
            //    Error.Description = "Невалидна стойност за параметър.";
            //    return Success;
            //}

            #endregion

            #region Judge

            #region Name
            if (model.Judge.JudgeName.Length > 255)
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "JudgeName";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна дължина на параметър. Изисква се дължина до 255 символа.";
                return Success;
            }

            //if (!model.Judge.JudgeName.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c)))
            //{
            //    Success = false;
            //    Error.ErrorType = "E099";
            //    Error.FaultyAttribute = "JudgeName";
            //    Error.Reason = "Некоректно подаден параметър";
            //    Error.Description = "Невалидна стойност за параметър.";
            //    return Success;
            //}

            #endregion

            #region CaseRole
            if (model.CaseRole.Length > 255)
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "CaseRole";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна дължина на параметър. Изисква се дължина до 255 символа.";
                return Success;
            }

            if (!model.CaseRole.All(Char.IsDigit))
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "CaseRole";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна стойност за параметър.";
                return Success;
            }

            int CaseRoleID = GetNomenclatureIDByCode<CaseRole>(model.CaseRole);

            if (CaseRoleID == -1)
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "CaseRole";
                Error.Reason = "Некоректно подаден параметър";
                return Success;
            }
            #endregion

            #endregion

            if (DismissalType == NomenclatureConstants.DismissalTypes.Otvod)
            {

                #region Objection

                #region DocumentType
                if (model.Objection.DocumentType.Length > 255)
                {
                    Success = false;
                    Error.ErrorType = "E099";
                    Error.FaultyAttribute = "DocumentType";
                    Error.Reason = "Некоректно подаден параметър";
                    Error.Description = "Невалидна дължина на параметър. Изисква се дължина до 255 символа.";
                    return Success;
                }

                //if (!model.Objection.DocumentType.All(Char.IsLetterOrDigit))
                //{
                //    Success = false;
                //    Error.ErrorType = "E099";
                //    Error.FaultyAttribute = "DocumentType";
                //    Error.Reason = "Некоректно подаден параметър";
                //    Error.Description = "Невалидна стойност за параметър.";
                //    return Success;
                //}

                #endregion

                #region DocumentNumber
                if (model.Objection.DocumentNumber.ToString().Length > 255)
                {
                    Success = false;
                    Error.ErrorType = "E099";
                    Error.FaultyAttribute = "DocumentNumber";
                    Error.Reason = "Некоректно подаден параметър";
                    Error.Description = "Невалидна дължина на параметър. Изисква се дължина до 255 символа.";
                    return Success;
                }

                #endregion

                #region SideName

                if (model.Objection.SideName.Length > 255)
                {
                    Success = false;
                    Error.ErrorType = "E099";
                    Error.FaultyAttribute = "SideName";
                    Error.Reason = "Некоректно подаден параметър";
                    Error.Description = "Невалидна дължина на параметър. Изисква се дължина до 255 символа.";
                    return Success;
                }

                //if (!model.Objection.SideName.All(Char.IsLetterOrDigit))
                //{
                //    Success = false;
                //    Error.ErrorType = "E099";
                //    Error.FaultyAttribute = "SideName";
                //    Error.Reason = "Некоректно подаден параметър";
                //    Error.Description = "Невалидна стойност за параметър.";
                //    return Success;
                //}

                #endregion

                #region SideInvolmentKind

                if (model.Objection.SideInvolmentKind.Length > 255)
                {
                    Success = false;
                    Error.ErrorType = "E099";
                    Error.FaultyAttribute = "SideInvolmentKind";
                    Error.Reason = "Некоректно подаден параметър";
                    Error.Description = "Невалидна дължина на параметър. Изисква се дължина до 255 символа.";
                    return Success;
                }

                //if (!model.Objection.SideInvolmentKind.All(Char.IsLetterOrDigit))
                //{
                //    Success = false;
                //    Error.ErrorType = "E099";
                //    Error.FaultyAttribute = "SideInvolmentKind";
                //    Error.Reason = "Некоректно подаден параметър";
                //    Error.Description = "Невалидна стойност за параметър.";
                //    return Success;
                //}

                #endregion

                #endregion
            }
            #region Decision

            if(model.Decision.HearingDate >= DateTime.Now.Date.AddDays(1))
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "HearingDate";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна дата. Заседанието не може да бъде с бъдеща дата";
                return Success;
            }


            #region HearingType

            if (model.Decision.HearingType.Length > 255)
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "HearingType";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна дължина на параметър. Изисква се дължина до 255 символа.";
                return Success;
            }

            if (!model.Decision.HearingType.All(Char.IsLetterOrDigit))
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "HearingType";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна стойност за параметър.";
                return Success;
            }

            int HearingTypeID = GetNomenclatureIDByCode<HearingType>(model.Decision.HearingType);

            if (HearingTypeID == -1)
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "HearingType";
                Error.Reason = "Некоректно подаден параметър";
                return Success;
            }
            #endregion

            #region ActType

            if (model.Decision.ActType.Length > 255)
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "ActType";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна дължина на параметър. Изисква се дължина до 255 символа.";
                return Success;
            }

            if (!model.Decision.ActType.All(Char.IsLetterOrDigit))
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "ActType";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна стойност за параметър.";
                return Success;
            }

            int ActTypeID = GetNomenclatureIDByCode<ActType>(model.Decision.ActType);

            if (ActTypeID == -1)
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "ActType";
                Error.Reason = "Некоректно подаден параметър";
                return Success;
            }
            #endregion

            #region ActNumber

            if (model.Decision.ActNumber.ToString().Length > 255)
            {
                Success = false;
                Error.ErrorType = "E099";
                Error.FaultyAttribute = "ActNumber";
                Error.Reason = "Некоректно подаден параметър";
                Error.Description = "Невалидна дължина на параметър. Изисква се дължина до 255 символа.";
                return Success;
            }

            #endregion

            #endregion

            Error = null;
            return Success;
        }

        public bool ValidateDismissalGUID(Guid DismissalID)
        {
            return DismissalID_IfExists(DismissalID.ToString());
        }

        public bool ValidateActUpdateData(ActPublicationRequest model, UpdateResponse response)
        {
            bool Success = true;

            #region Dismissal

            if (!ValidateDismissalGUID(model.DismissalId))
            {
                Success = false;
                response.Error.ErrorType = "E002";
                response.Error.FaultyAttribute = "DismissalId";
                response.Error.Reason = "Некоректно подаден параметър";
                response.Error.Description = "Грешен идентификатор на отвод.";
                return Success;
            }

            #endregion

            #region Name
            if (model.FileName.Length > 255)
            {
                Success = false;
                response.Error.ErrorType = "E099";
                response.Error.FaultyAttribute = "FileName";
                response.Error.Reason = "Некоректно подаден параметър";
                response.Error.Description = "Невалидна дължина на параметър. Изисква се дължина до 255 символа.";
                return Success;
            }

            //if (!model.FileName.All(Char.IsLetterOrDigit))
            //{
            //    Success = false;
            //    response.Error.ErrorType = "E099";
            //    response.Error.FaultyAttribute = "FileName";
            //    response.Error.Reason = "Некоректно подаден параметър";
            //    response.Error.Description = "Невалидна стойност за параметър.";
            //    return Success;
            //}

            #endregion

            #region FileSource
            if (model.FileSource.Length > 5000000)
            //5MB Filesize limit, including headers
            {
                Success = false;
                response.Error.ErrorType = "E004";
                response.Error.FaultyAttribute = "FileSource";
                response.Error.Reason = "Надхвърлен размер на файл";
                response.Error.Description = "Невалиден размер на файл. Позволен е размер до 5MB.";
                return Success;
            }

            #endregion

            #region MimeType
            //Allowed MimeTypes: DOC/DOCX/RTF/PDF/HTML
            if (model.MimeType != "application/msword" &&
                model.MimeType != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" &&
                model.MimeType != "application/rtf" &&
                model.MimeType != "application/pdf" &&
                model.MimeType != "text/html")
            {
                Success = false;
                response.Error.ErrorType = "E099";
                response.Error.FaultyAttribute = "MimeType";
                response.Error.Reason = "Некоректно подаден параметър";
                response.Error.Description = "Невалиден тип на файл.";
                return Success;
            }

            #endregion

            return Success;
        }

        public bool ValidateReplaceUpdateData(ReplaceDismissalRequest model, UpdateResponse response)
        {
            bool Success = true;

            #region Dismissal

            if (!ValidateDismissalGUID(model.DismissalId))
            {
                Success = false;
                response.Error.ErrorType = "E002";
                response.Error.FaultyAttribute = "DismissalId";
                response.Error.Reason = "Некоректно подаден параметър";
                response.Error.Description = "Грешен идентификатор на отвод.";
                return Success;
            }

            #endregion

            #region Name
            if (model.ReplaceJudge.JudgeName.Length > 255)
            {
                Success = false;
                response.Error.ErrorType = "E099";
                response.Error.FaultyAttribute = "JudgeName";
                response.Error.Reason = "Некоректно подаден параметър";
                response.Error.Description = "Невалидна дължина на параметър. Изисква се дължина до 255 символа.";
                return Success;
            }

            //if (!model.ReplaceJudge.JudgeName.All(Char.IsLetter))
            //{
            //    Success = false;
            //    response.Error.ErrorType = "E099";
            //    response.Error.FaultyAttribute = "JudgeName";
            //    response.Error.Reason = "Некоректно подаден параметър";
            //    response.Error.Description = "Невалидна стойност за параметър.";
            //    return Success;
            //}

            #endregion

            return Success;
        }

        public async Task<DismissalRegistrationResponse> Insert(DismissalRegistrationRequest model)
        {
            try
            {
                DismissalRegistrationResponse result = new DismissalRegistrationResponse();
                result.Error = new ErrorModel();

                if (!ValidateInsertData(model, result.Error))
                {
                    return result;
                }

                if (!ValidateDataConditions(model, result.Error))
                {
                    return result;
                }

                //Insert
                Dismissal entity = new Dismissal();

                entity.EntryType = EntryType.API;
                entity.CaseNumber = model.CaseNumber;
                entity.CaseYear = model.CaseYear;
                entity.JudgeFullName = model.Judge.JudgeName;
                //entity.JudgeGivenName = model.Judge.GivenName;
                //entity.JudgeMiddleName = model.Judge.MiddleName;
                //entity.JudgeFamilyName = model.Judge.FamilyName;
                entity.JudgeIsChairman = model.Judge.IsChairman;

                //Invalid codes/Ids should be eliminated by validations by this point
                entity.CourtId = GetNomenclatureIDByCode<Court>(model.Court);
                entity.CaseTypeId = GetNomenclatureIDByCode<CaseType>(model.CaseType);
                entity.CaseRoleId = GetNomenclatureIDByCode<CaseRole>(model.CaseRole);
                entity.DismissalTypeId = GetNomenclatureIDByCode<DismissalType>(model.DismissalType);
                entity.ActTypeId = GetNomenclatureIDByCode<ActType>(model.Decision.ActType);
                entity.HearingTypeId = GetNomenclatureIDByCode<HearingType>(model.Decision.HearingType);
                entity.ObjectionUpheld = model.ObjectionUpheld;
                entity.DismissalReason = model.DismissalReason;

                if (entity.DismissalTypeId == NomenclatureConstants.DismissalTypes.Otvod)
                {
                    entity.DocumentType = model.Objection.DocumentType;
                    entity.DocumentDate = model.Objection.DocumentDate;
                    entity.DocumentNumber = model.Objection.DocumentNumber;
                    entity.SideFullName = model.Objection.SideName;
                    entity.SideInvolmentKind = model.Objection.SideInvolmentKind;
                }

                entity.HearingDate = model.Decision.HearingDate;
                entity.ActNumber = model.Decision.ActNumber;
                entity.ActDeclaredDate = model.Decision.ActDeclaredDate;
                entity.DateWrt = DateTime.Now;

                entity.Id = Guid.NewGuid().ToString();

                await repo.AddAsync(entity);
                await repo.SaveChangesAsync();

                result.DismissalId = Guid.Parse(entity.Id);

                result.Error = null;

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DismissalService.Insert");
                DismissalRegistrationResponse err = new DismissalRegistrationResponse();
                err.Error = new ErrorModel();
                err.Error.Description = ex.Message;
                return err;
            }
        }

        public async Task<UpdateResponse> Update(DismissalRegistrationRequest model)
        {
            try
            {
                UpdateResponse result = new UpdateResponse();
                result.Error = new ErrorModel();

                if (model.DismissalId != null)//Update
                {
                    if (!ValidateUpdateData(model, result.Error))
                    {
                        return result;
                    }

                    if (!ValidateDataConditions(model, result.Error))
                    {
                        return result;
                    }

                    var entity = await repo.GetByIdAsync<Dismissal>(model.DismissalId.ToString());

                    if (entity.EntryType != EntryType.API)
                    {
                        result.UpdateSuccessful = false;
                        result.Error = new ErrorModel();
                        result.Error.ErrorType = "E099";
                        result.Error.FaultyAttribute = "EntryType";
                        result.Error.Description = "Избраният запис не може да бъде редактиран";
                        return result;
                    }

                    entity.CaseNumber = model.CaseNumber;
                    entity.CaseYear = model.CaseYear;
                    entity.JudgeFullName = model.Judge.JudgeName;
                    //entity.JudgeGivenName = model.Judge.GivenName;
                    //entity.JudgeMiddleName = model.Judge.MiddleName;
                    //entity.JudgeFamilyName = model.Judge.FamilyName;
                    entity.JudgeIsChairman = model.Judge.IsChairman;

                    //Invalid codes/Ids should be eliminated by validations by this point
                    entity.CourtId = GetNomenclatureIDByCode<Court>(model.Court);
                    entity.CaseTypeId = GetNomenclatureIDByCode<CaseType>(model.CaseType);
                    entity.CaseRoleId = GetNomenclatureIDByCode<CaseRole>(model.CaseRole);
                    entity.DismissalTypeId = GetNomenclatureIDByCode<DismissalType>(model.DismissalType);
                    entity.ActTypeId = GetNomenclatureIDByCode<ActType>(model.Decision.ActType);
                    entity.HearingTypeId = GetNomenclatureIDByCode<HearingType>(model.Decision.HearingType);

                    //if (model.ObjectionUpheld != null)
                    //{
                    //    entity.ObjectionUpheld = (bool)model.ObjectionUpheld;
                    //}
                    entity.ObjectionUpheld = model.ObjectionUpheld;
                    entity.DismissalReason = model.DismissalReason;
                    entity.DocumentType = model.Objection.DocumentType;
                    entity.DocumentDate = model.Objection.DocumentDate;
                    entity.DocumentNumber = model.Objection.DocumentNumber;
                    entity.SideFullName = model.Objection.SideName;
                    //entity.SideGivenName = model.Objection.GivenName;
                    //entity.SideMiddleName = model.Objection.MiddleName;
                    //entity.SideFamilyName = model.Objection.FamilyName;
                    entity.SideInvolmentKind = model.Objection.SideInvolmentKind;
                    //entity.HearingType = model.Decision.HearingType;
                    entity.HearingDate = model.Decision.HearingDate;
                    entity.ActNumber = model.Decision.ActNumber;
                    entity.ActDeclaredDate = model.Decision.ActDeclaredDate;
                    entity.DateWrt = DateTime.Now;
                    repo.Update(entity);
                }
                else
                {
                    result.UpdateSuccessful = false;
                    result.Error = new ErrorModel();
                    result.Error.ErrorType = "E002";
                    result.Error.FaultyAttribute = "DismissalId";
                    result.Error.Reason = "Некоректно подаден параметър";
                    result.Error.Description = "Invalid dismissal guid";
                    return result;
                }
                await repo.SaveChangesAsync();

                result.UpdateSuccessful = true;
                result.Error = null;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DismissalService.Update");
                UpdateResponse result = new UpdateResponse();
                result.Error = new ErrorModel();
                result.Error.Description = ex.Message;
                result.UpdateSuccessful = false;
                return result;
            }
        }

        public async Task<UpdateResponse> ReplaceUpdate(ReplaceDismissalRequest model, string courtCode)
        {
            try
            {
                UpdateResponse result = new UpdateResponse();
                result.Error = new ErrorModel();
                if (model.DismissalId != null)//Update
                {
                    if (!ValidateReplaceUpdateData(model, result))
                    {
                        return result;
                    }

                    var saved = await repo.GetByIdAsync<Dismissal>(model.DismissalId.ToString());
                    if (saved != null)
                    {
                        var court = await repo.GetByIdAsync<Court>(saved.CourtId);
                        if (court.Code != courtCode)
                        {
                            result.UpdateSuccessful = false;
                            result.Error.FaultyAttribute = "DismissalId";
                            result.Error.Reason = "Некоректно подаден параметър";
                            result.Error.Description = "Отводът е регистриран в друг съд.";
                            return result;
                        }
                    }

                    saved.ReplaceJudgeFullName = model.ReplaceJudge.JudgeName;
                    saved.ReplaceJudgeIsChairman = model.ReplaceJudge.IsChairman;

                    repo.Update(saved);
                }
                else
                {
                    result.UpdateSuccessful = false;
                    result.Error = new ErrorModel();
                    result.Error.ErrorType = "E002";
                    result.Error.FaultyAttribute = "DismissalId";
                    result.Error.Reason = "Некоректно подаден параметър";
                    result.Error.Description = "Invalid dismissal guid";
                    return result;
                }
                await repo.SaveChangesAsync();

                result.UpdateSuccessful = true;
                result.Error = null;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DismissalService.ReplaceUpdate");
                UpdateResponse result = new UpdateResponse();
                result.Error = new ErrorModel();
                result.UpdateSuccessful = false;
                result.Error.Description = ex.Message;
                return result;
            }
        }

        public async Task<UpdateResponse> DismissalActUpdate(ActPublicationRequest model)
        {
            try
            {
                UpdateResponse result = new UpdateResponse();
                result.Error = new ErrorModel();

                if (model.DismissalId != null)//Update
                {
                    if (!ValidateActUpdateData(model, result))
                    {
                        return result;
                    }

                    CdnUploadRequest request = new CdnUploadRequest();

                    request.ContentType = model.MimeType;
                    request.FileContentBase64 = model.FileSource;
                    request.SourceType = SourceType.Dismissal;
                    request.FileName = model.FileName;

                    //validation should have eliminated null Ids by this point
                    request.SourceId = model.DismissalId.ToString();

                    await cdnService.MongoCdn_AppendUpdate(request);
                }
                else
                {
                    result.UpdateSuccessful = false;
                    result.Error = new ErrorModel();
                    result.Error.ErrorType = "E002";
                    result.Error.FaultyAttribute = "DismissalId";
                    result.Error.Reason = "Некоректно подаден параметър";
                    result.Error.Description = "Invalid dismissal guid";
                    return result;
                }
                result.UpdateSuccessful = true;
                result.Error = null;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DismissalService.DismissalActUpdate");
                UpdateResponse result = new UpdateResponse();
                result.Error.Description = ex.Message;
                result.UpdateSuccessful = false;
                return result;
            }
        }

    }
}
