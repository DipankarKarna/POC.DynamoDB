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
    public interface IDeletePerson
    {
        Task<DeleteTableResponse> DeletePerson(int id);
    }
    public class DeleteTable : IDeletePerson
    {
        private static readonly string tableName = "POC_Person";
        private readonly IAmazonDynamoDB _dynamoClient;

        public DeleteTable(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        public async Task<DeleteTableResponse> DeletePerson(int id)
        {
            var request = new DeleteTableRequest
            {
                TableName = tableName
            };

            var response = await _dynamoClient.DeleteTableAsync(request);

            return response;
        }
    }
}
