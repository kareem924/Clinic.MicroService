using Common.CQRS;
using Security.Core.Entities;
using System;

namespace Security.API.Application.Commands.RegisterUser
{
    public class RegisterUserCommand : ICommand
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Email { get; private set; }

        public Address Address { get; private set; }

        public DateTime BirthDate { get; private set; }

        public string PhoneNumber { get; private set; }

        public string Password { get; private set; }

        public RegisterUserCommand(
            string firstName,
            string lastName,
            string email,
            string street,
            string state,
            string city,
            string country,
            DateTime birthDate,
            string phoneNumber,
            string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Address = new Address(street, city, state, country, string.Empty);
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            Password = password;
        }
    }
}
