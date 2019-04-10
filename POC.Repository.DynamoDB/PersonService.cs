using POC.Repository.DynamoDB.Operations;
using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Repository.DynamoDB
{
    public interface IPersonService
    {
        bool CreateTable(ICreateTable createTable);
    }

    public class PersonService: IPersonService
    {
        //private readonly ICreateTable _createTable;
        //private readonly IPutItem _putItem;
        //private readonly IGetItem _getItem;
        //private readonly IUpdateItem _updateItem;
        //private readonly IDeleteTable _deleteTable;

        public PersonService()
        //public PersonService(ICreateTable createTable, IPutItem putItem, IGetItem getItem, IUpdateItem updateItem, IDeleteTable deleteTable)
        {
            //_createTable = createTable;
            //_putItem = putItem;
            //_getItem = getItem;
            //_updateItem = updateItem;
            //_deleteTable = deleteTable;
        }

        public bool CreateTable(ICreateTable createTable)
        {
            createTable.CreateDynamoDbTable();

            return true;
        }

        public bool GetPersons(IGetPerson getPerson)
        {
            getPerson.GetPersons(null);

            return true;
        }
    }
}
