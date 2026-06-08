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
                NationalId = dto.NationalId,
                ProfilePhotoUrl = dto.ProfilePhotoUrl,
                IsActive = dto.IsActive,
                GenderId = dto.GenderId,
                UserStatusId = dto.UserStatusId,
                PropertyId = dto.PropertyId
            };
        }

        public static User ToEntity(this UPDATEUserDto dto)
        {
            return new User
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


        public static ApplicationUser ToEntity(this UPDATEBasicUserDto dto)
        {
            return new ApplicationUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.EmailAddress,
                PhoneNumber = dto.MobileNumber,
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
                NationalId = dto.NationalId,
                ProfilePhotoUrl = dto.ProfilePhotoUrl,
                IsActive = dto.IsActive,
                GenderId = dto.GenderId,
                UserStatusId = UserStatus,
                PropertyId = dto.PropertyId
            };
        }





        public static User UpdateEntity(this UPDATEUserDto dto, User existingUser)
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
                PropertyId = dto.Property?.Id,
                PropertyName = dto.Property?.Name,
                UserStatus = dto.UserStatus.Item,
            };
        }

        public static READAppUserDto ToReadDto(this ApplicationUser dto)
        {
            return new READAppUserDto
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
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
                UserStatusId = dto.UserStatusId
            };
        }
    }
}
