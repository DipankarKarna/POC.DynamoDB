using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POC.Repository.DynamoDB;
using POC.Repository.DynamoDB.Operations;

namespace API.Person.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private IPersonService _personService;
        private readonly ICreateTable _createTable;
        private readonly IGetPerson _getPersons;

        public PersonsController(ICreateTable TheCreateTable, IGetPerson TheGetPersons)
        {
            _personService = new PersonService();
            _createTable = TheCreateTable;
            _getPersons = TheGetPersons;
        }
        // GET: api/Persons
        [HttpGet]
        public IEnumerable<POC.Repository.DynamoDB.Models.Person> Get()
        {
            return _getPersons.GetPersons(null).Result.PersonList;
        }

        // GET: api/Persons/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Persons
        [HttpPost]
        public void Post()
        {
            _personService.CreateTable(_createTable);
        }

        // PUT: api/Persons/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


    }
}
