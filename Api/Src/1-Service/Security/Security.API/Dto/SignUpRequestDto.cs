using Security.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.API.Dto
{
    public class SignUpRequestDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
        public String Street { get; set; }

        public String City { get; set; }

        public String State { get; set; }

        public String Country { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }
    }
}
