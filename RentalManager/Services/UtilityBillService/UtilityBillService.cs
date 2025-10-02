using RentalManager.DTOs.Unit;
using RentalManager.DTOs.UnitType;
using RentalManager.DTOs.UtilityBill;
using RentalManager.Mappings;
using RentalManager.Models;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.TenantRepository;
using RentalManager.Repositories.UtilityBillRepository;

namespace RentalManager.Services.UtilityBillService
{
    public class UtilityBillService : IUtilityBillService
    {

        private readonly IUtilityBillRepository _repo;
        private readonly IPropertyRepository _propertyrepo;
        private readonly ITenantRepository _tenantrepo;

        public UtilityBillService(IUtilityBillRepository repo, IPropertyRepository propertyrepo, ITenantRepository tenantrepo)
        {
            _repo = repo;
            _propertyrepo = propertyrepo;
            _tenantrepo = tenantrepo;
        }



        public async Task<ApiResponse<List<READUtilityBillDto>>> GetAll()
        {
            try
            {
                var bills = await _repo.GetAllAsync();

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
                var bill = await _repo.GetByIdAsync(id);

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
                var bills = await _repo.GetByPropertyIdAsync(id);

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


                var bills = await _repo.GetByPropertyIdAsync(tenant.User.PropertyId);


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
                var property = await _propertyrepo.FindAsync(AddedBill.PropertyId);

                if (property == null) return new ApiResponse<READUtilityBillDto>(null, "Property Provided Does Not Exist.");


                var entity = AddedBill.ToEntity();
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

                var existing = await _repo.FindAsync(id);

                if (existing == null) return new ApiResponse<READUtilityBillDto>(null, "No Such Data.");

                var property = await _propertyrepo.FindAsync(bill.PropertyId);

                if (property == null) return new ApiResponse<READUtilityBillDto>(null, "Property Does Not Exist.");


                var entity = bill.ToEntity(id);
                var updated = await _repo.UpdateAsync(entity);

                if (updated == null)
                    return new ApiResponse<READUtilityBillDto>(null, "Data Not Found.");

                return new ApiResponse<READUtilityBillDto>(null, "Updated successfully.");
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
                var entity = await _repo.FindAsync(id);

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

    }
}
