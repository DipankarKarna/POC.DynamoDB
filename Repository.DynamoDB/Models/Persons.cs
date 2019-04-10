using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Repository.DynamoDB.Models
{
    public class Persons
    {
        public IEnumerable<Person> PersonList { get; set; }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
