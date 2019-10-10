using Security.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.API.Quries.GetUserByUserName
{
    public class MapperService : IMapperService
    {
        public LoginUserDto MapUserToLoginUserDto(User user)
        {
            var userDto = new LoginUserDto()
            {
                Id = user.Id,
                BirthDate = user.BirthDate,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsDeleted = user.IsDeleted,
                CreatedBy = user.CreatedBy,
                CreationDate = user.CreationDate,
                UpdateBy = user.UpdateBy,
            };
            return userDto;
        }
       
    }

    public interface IMapperService
    {
        LoginUserDto MapUserToLoginUserDto(User user);
    }
}

  
