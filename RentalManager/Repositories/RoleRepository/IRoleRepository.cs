﻿using RentalManager.Models;

namespace RentalManager.Repositories.RoleRepository
{
    public interface IRoleRepository
    {
        Task<List<Role>?> GetAllAsync();
        Task<Role?> GetByIdAsync(int id);
        Task<Role?> GetByNameAsync(string name);
        Task<Role> AddAsync(Role role);
        Task<Role> UpdateAsync(Role role);
        Task DeleteAsync(Role role);
        Task<Role> FindAsync(int id);
    }
}
