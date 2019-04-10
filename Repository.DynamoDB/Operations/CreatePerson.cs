using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using POC.Repository.DynamoDB.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace POC.Repository.DynamoDB.Operations
{
    public interface ICreatePerson
    {
        Task AddNewEntry(int id, string FirstName, string LastName, DateTime BirthDate);
    }

    public class CreatePerson : ICreatePerson
    {
        private readonly IAmazonDynamoDB _dynamoClient;

        public CreatePerson(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        public async Task AddNewEntry(int id, string FirstName, string LastName, DateTime BirthDate)
        {
            var queryRequest = RequestBuilder(id, FirstName, LastName, BirthDate);

            await PutItemAsync(queryRequest);
        }

        private PutItemRequest RequestBuilder(int id, string FirstName, string LastName, DateTime BirthDate)
        {
            var item = new Dictionary<string, AttributeValue>
            {
                {"Id", new AttributeValue {N = id.ToString()}},
                {"FirstName", new AttributeValue {S = FirstName}},
                {"LastName", new AttributeValue {S = LastName}},
                {"BirthDate", new AttributeValue {S = BirthDate.ToString()}}
            };

            return new PutItemRequest
            {
                TableName = "POC_Person",
                Item = item
            };
        }

        private async Task PutItemAsync(PutItemRequest request)
        {
            await _dynamoClient.PutItemAsync(request);
        }
    }
}
