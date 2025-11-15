using ClassRecord;
using ClassRecord.MemberManagement;
using DataAccessManager;
using DataBaseModels.DBModels;
using EnumClasses;
using ModelValidateAndConvert.MemberManagement;

namespace ServiceManager.MemberManagement
{
    public interface IMemberManagementServices
    {
        public Task<ReturnRecord> CreateMember(MemberManagementCreateRecord Member);
        public Task<ReturnRecord> UpdateMember(int Id, MemberManagementUpdateRecord Member);
        public Task<ReturnRecord> DeleteMember(int Id);
        public Task<ReturnRecord> GetMemberById(int Id);
        public Task<ReturnRecord> GetAllMembers(int pageNumber, int size, string fullname, string email, string phone);
        public Task<ReturnRecord> GetAllMembers();
    }
    public class MemberManagementServices : IMemberManagementServices
    {
        private readonly IEFCoreDataAccessManager<MemberManagementTable> _dataAccess;
        public MemberManagementServices(IEFCoreDataAccessManager<MemberManagementTable> dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<ReturnRecord> CreateMember(MemberManagementCreateRecord MemberRecord)
        {
            try
            {
                MemberManagementCreate obj = new MemberManagementCreate(MemberRecord);
                if (!obj.IsValid())
                {
                    return new ReturnRecord(string.Empty, obj.GetErrorMessage(), ResultStatus.Failure);
                }
                var result = await _dataAccess.InsertAsync(obj.GetData());
                await _dataAccess.SaveChangesAsync();
                return new ReturnRecord(result.MemberId, "Create Sucessfull", ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure);
            }
        }

        public async Task<ReturnRecord> UpdateMember(int Id, MemberManagementUpdateRecord Member)
        {
            try
            {
                MemberManagementUpdate obj = new MemberManagementUpdate(Member);
                if (!obj.IsValid(Id))
                {
                    return new ReturnRecord(string.Empty, obj.GetErrorMessage(), ResultStatus.Failure);
                }
                var memberData = await _dataAccess.GetByIdAsync(Id);
                if (memberData == null
                    || memberData.IsDelete == (int)DeleteStatus.Delete)
                {
                    return new ReturnRecord(string.Empty, "Member Not Found", ResultStatus.Failure);
                }
                var result = await _dataAccess.UpdateAsync(Id, obj.GetData());
                await _dataAccess.SaveChangesAsync();
                return new ReturnRecord(result.MemberId, "Update Sucessfull", ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure);
            }
        }

        public async Task<ReturnRecord> DeleteMember(int Id)
        {
            try
            {
                var member = await _dataAccess.GetByIdAsync(Id);
                if (member == null
                   || member.IsDelete == (int)DeleteStatus.Delete)
                {
                    return new ReturnRecord(string.Empty, "Member Not Found", ResultStatus.Failure);
                }
                member.IsDelete = (int)DeleteStatus.Delete;
                var result = await _dataAccess.DeleteAsync(Id,member);
                await _dataAccess.SaveChangesAsync();
                if(result != null)
                    return new ReturnRecord(Id, "Delete Sucessfull", ResultStatus.Success);
                else
                    return new ReturnRecord(string.Empty, "Member Not Found", ResultStatus.Failure);
            }
            catch (Exception ex)
            {
                return new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure);
            }
        }

        public async Task<ReturnRecord> GetMemberById(int Id)
        {
            try
            {
                var result = await _dataAccess.GetByIdAsync(Id);
                if (result == null
                    || result.IsDelete == (int) DeleteStatus.Delete)
                {
                    return new ReturnRecord(string.Empty, "Member Not Found", ResultStatus.Failure);
                }
                else if(result.IsActive == (int)ActiveStatus.InActive)
                {
                    return new ReturnRecord(string.Empty, "Deactivate Member Found", ResultStatus.Failure);
                }
                return new ReturnRecord(new MemberManagementGet(result).GetData(), "Get Sucessfull", ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure);
            }
        }

        public async Task<ReturnRecord> GetAllMembers(int pageNumber,int size,string fullname,string email,string phone)
        {
            try
            {                
                var obj = new MemberManagementGetAll(fullname, email, phone, pageNumber, size);
                if (!obj.IsValid())
                {
                    return new ReturnRecord(string.Empty, obj.GetErrorMessage(), ResultStatus.Failure);
                }
                var result = await DapperDataAccessManager.QueryList<MemberManagementTable>(obj.GetQuery());
                if (result == null)
                {
                    return new ReturnRecord(string.Empty, "Member Not Found", ResultStatus.Failure);
                }
                return new ReturnRecord(new MemberManagementGetAll(result).GetData(), "Get All Sucessfull", ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure);
            }
        }

        public async Task<ReturnRecord> GetAllMembers()
        {
            try
            {
                var result = await _dataAccess.GetAllAsync();
                if (result == null)
                {
                    return new ReturnRecord(string.Empty, "Member Not Found", ResultStatus.Failure);
                }
                return new ReturnRecord(new MemberManagementGetAll(result).GetData(), "Get All Sucessfull", ResultStatus.Success);
            }
            catch (Exception ex)
            {
                return new ReturnRecord(string.Empty, ex.Message + "\n" + ex.InnerException, ResultStatus.Failure);
            }
        }
    }
}
