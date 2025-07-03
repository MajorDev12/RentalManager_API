using RentalManager.DTOs.User;
using RentalManager.Models;

namespace RentalManager.Services.UserService
{
    public interface IUserService
    {
        Task<ApiResponse<List<READUserDto>>> GetAll();
        Task<ApiResponse<READUserDto>> GetById(int id);
        Task<ApiResponse<READUserDto>> Add(CREATEUserDto type);
        Task<ApiResponse<READUserDto>> Update(int id, UPDATEUserDto type);
        Task<ApiResponse<READUserDto>> Delete(int id);
    }
}
