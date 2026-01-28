using RentalManager.DTOs.UtilityBill;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.TenantRepository;
using RentalManager.Repositories.UtilityBillRepository;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Services.UtilityBillService
{
    public class UtilityBillService : IUtilityBillService
    {

        private readonly IUtilityBillRepository _repo;
        private readonly IPropertyRepository _propertyrepo;
        private readonly ITenantRepository _tenantrepo;
        private readonly ICurrentUser _currentuser;

        public UtilityBillService(
            IUtilityBillRepository repo,
            IPropertyRepository propertyrepo,
            ITenantRepository tenantrepo,
            ICurrentUser currentuser)
        {
            _repo = repo;
            _propertyrepo = propertyrepo;
            _tenantrepo = tenantrepo;
            _currentuser = currentuser;
        }



        public async Task<ApiResponse<List<READUtilityBillDto>>> GetAll()
        {
            try
            {
                var bills = await _repo.GetAllAsync(_currentuser);

                if (bills == null)
                {
                    return new ApiResponse<List<READUtilityBillDto>>(null, "Data Not Found.");
                }

                var billDtos = bills.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READUtilityBillDto>>(billDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READUtilityBillDto>>("Error Occurred");
            }
        }


        public async Task<ApiResponse<READUtilityBillDto>> GetById(int id)
        {
            try
            {
                var bill = await _repo.GetByIdAsync(_currentuser, id);

                if (bill == null)
                {
                    return new ApiResponse<READUtilityBillDto>(null, "Data Not Found.");
                }

                var billDto = bill.ToReadDto();

                return new ApiResponse<READUtilityBillDto>(billDto, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUtilityBillDto>("Error Occurred");
            }
        }


        public async Task<ApiResponse<List<READUtilityBillDto>>> GetByPropertyId(int id)
        {
            try
            {
                var bills = await _repo.GetByPropertyIdAsync(_currentuser, id);

                if (bills == null || bills.Count == 0)
                {
                    return new ApiResponse<List<READUtilityBillDto>>(null, "Data Not Found.");
                }

                var billDtos = bills.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READUtilityBillDto>>(billDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READUtilityBillDto>>("Error Occurred");
            }
        }


        public async Task<ApiResponse<List<READUtilityBillDto>>> GetByTenantId(int id)
        {
            try
            {
                var tenant = await _tenantrepo.GetByIdAsync(id);

                if(tenant == null)
                    return new ApiResponse<List<READUtilityBillDto>>(null, "tenant was not found");

                var bills = await _repo.GetByPropertyIdAsync(_currentuser, tenant.User.PropertyId);


                if (bills == null || bills.Count == 0)
                {
                    return new ApiResponse<List<READUtilityBillDto>>(null, "Data Not Found.");
                }

                var billDtos = bills.Select(it => it.ToReadDto()).ToList();

                return new ApiResponse<List<READUtilityBillDto>>(billDtos, "");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READUtilityBillDto>>("Error Occurred");
            }
        }


        public async Task<ApiResponse<READUtilityBillDto>> Add(CREATEUtilityBillDto AddedBill)
        {
            try
            {
                var property = await _propertyrepo.FindAsync(_currentuser, AddedBill.PropertyId);

                if (property == null) return new ApiResponse<READUtilityBillDto>(null, "Property Provided Does Not Exist.");


                var entity = AddedBill.ToEntity();
                entity.AccountId = _currentuser.AccountId;

                var bill = await _repo.AddAsync(entity);

                if (bill == null)
                {
                    return new ApiResponse<READUtilityBillDto>(null, "Data Not Found.");
                }


                return new ApiResponse<READUtilityBillDto>(null, "Utility Bill Created Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUtilityBillDto>($"Error Occurred: {ex.InnerException.Message} ");
            }
        }


        public async Task<ApiResponse<READUtilityBillDto>> Update(int id, UPDATEUtilityBillDto bill)
        {
            try
            {

                var existing = await _repo.FindAsync(_currentuser, id);

                if (existing == null) return new ApiResponse<READUtilityBillDto>(null, "No Such Data.");

                var property = await _propertyrepo.FindAsync(_currentuser, bill.PropertyId);

                if (property == null) return new ApiResponse<READUtilityBillDto>(null, "Property Does Not Exist.");


                var entity = bill.ToEntity();

                
                if (!HasUtilityBillChanged(existing, entity))
                {
                    return new ApiResponse<READUtilityBillDto>(null, "No changes detected.", false);
                }

                var updatedEntity = entity.ToUpdateEntity(existing);

                await _repo.UpdateAsync(updatedEntity);

                var result = updatedEntity.ToReadDto();

                return new ApiResponse<READUtilityBillDto>(result, "Updated successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUtilityBillDto>("Error Occurred.");
            }
        }


        public async Task<ApiResponse<READUtilityBillDto>> Delete(int id)
        {
            try
            {
                var entity = await _repo.FindAsync(_currentuser, id);

                if (entity == null)
                    return new ApiResponse<READUtilityBillDto>("Data Not Found.");

                await _repo.DeleteAsync(entity);

                return new ApiResponse<READUtilityBillDto>(null, "Deleted successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<READUtilityBillDto>($"Error Occurred: {ex.Message}");
            }
        }


        private static bool HasUtilityBillChanged( UtilityBill existing, UtilityBill updated)
        {
            return
                existing.Name != updated.Name ||
                existing.Amount != updated.Amount ||
                existing.isReccuring != updated.isReccuring ||
                existing.PropertyId != updated.PropertyId;
        }


    }
}
