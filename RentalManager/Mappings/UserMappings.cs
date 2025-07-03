using RentalManager.DTOs.User;
using RentalManager.Models;

namespace RentalManager.Mappings
{
    public static class UserMappings
    {
        public static User ToEntity(this CREATEUserDto dto)
        {
            return new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                MobileNumber = dto.MobileNumber,
                AlternativeNumber = dto.AlternativeNumber,
                PasswordHash = dto.PasswordHash,
                LastPasswordChange = dto.LastPasswordChange,
                NationalId = dto.NationalId,
                ProfilePhotoUrl = dto.ProfilePhotoUrl,
                IsActive = dto.IsActive,
                GenderId = dto.GenderId,
                UserStatusId = dto.UserStatusId,
                PropertyId = dto.PropertyId
            };
        }

        public static User ToEntity(this UPDATEUserDto dto, int id)
        {
            return new User
            {
                Id = id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                MobileNumber = dto.MobileNumber,
                AlternativeNumber = dto.AlternativeNumber,
                NationalId = dto.NationalId,
                ProfilePhotoUrl = dto.ProfilePhotoUrl,
                IsActive = dto.IsActive,
                GenderId = dto.GenderId,
                UserStatusId = dto.UserStatusId
            };
        }


        public static User ToEntity(this CREATEUserDto dto, int roleId, int UserStatus)
        {
            return new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                MobileNumber = dto.MobileNumber,
                AlternativeNumber = dto.AlternativeNumber,
                PasswordHash = dto.PasswordHash,
                LastPasswordChange = dto.LastPasswordChange,
                NationalId = dto.NationalId,
                ProfilePhotoUrl = dto.ProfilePhotoUrl,
                IsActive = dto.IsActive,
                GenderId = dto.GenderId,
                RoleId = roleId,
                UserStatusId = UserStatus,
                PropertyId = dto.PropertyId
            };
        }



        public static User UpdateEntity(this User dto, User existingUser)
        {
            existingUser.FirstName = dto.FirstName;
            existingUser.LastName = dto.LastName;
            existingUser.EmailAddress = dto.EmailAddress;
            existingUser.MobileNumber = dto.MobileNumber;
            existingUser.AlternativeNumber = dto.AlternativeNumber;
            existingUser.NationalId = dto.NationalId;
            existingUser.ProfilePhotoUrl = dto.ProfilePhotoUrl;
            existingUser.IsActive = dto.IsActive;
            existingUser.GenderId = dto.GenderId;

            return existingUser;
        }



        public static READUserDto ToReadDto(this User dto)
        {
            return new READUserDto
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                MobileNumber = dto.MobileNumber,
                AlternativeNumber = dto.AlternativeNumber,
                NationalId = dto.NationalId,
                ProfilePhotoUrl = dto.ProfilePhotoUrl,
                IsActive = dto.IsActive,
                GenderId = dto.GenderId,
                GenderName = dto.Gender.Item,
                UserStatus = dto.UserStatus.Item,
                RoleName = dto.Role.Name,
                propertyId = dto.PropertyId,
                PropertyName = dto.Property.Name
            };
        }

        public static UPDATEUserDto ToUpdateDto(this User dto)
        {
            return new UPDATEUserDto
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                MobileNumber = dto.MobileNumber,
                AlternativeNumber = dto.AlternativeNumber,
                NationalId = dto.NationalId,
                ProfilePhotoUrl = dto.ProfilePhotoUrl,
                IsActive = dto.IsActive,
                GenderId = dto.GenderId,
                UserStatusId = dto.UserStatusId
            };
        }
    }
}
