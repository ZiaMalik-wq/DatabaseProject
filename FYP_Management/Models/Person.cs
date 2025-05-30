using System;

namespace FYP_Management.Models
{
    public class Person(string firstName, string lastName, string contact, string email, DateTime dateOfBirth, int gender)
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;
        public string Contact { get; set; } = contact;
        public string Email { get; set; } = email;
        public DateTime DateOfBirth { get; set; } = dateOfBirth;
        public int Gender { get; set; } = gender;

        public Person(int id, string firstName, string lastName, string contact, string email, DateTime dateOfBirth, int gender)
        : this(firstName, lastName, contact, email, dateOfBirth, gender)
        {
            Id = id;
        }
    }
}
