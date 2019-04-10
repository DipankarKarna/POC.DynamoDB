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
    public interface IGetPerson
    {
        Task<Persons> GetPersons(int? id);
    }

    public class GetPerson: IGetPerson
    {
        private readonly IAmazonDynamoDB _dynamoClient;

        public GetPerson(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        public async Task<Persons> GetPersons(int? id)
        {
            var queryRequest = RequestBuilder(id);

            var result = await ScanAsync(queryRequest);

            return new Persons
            {
                PersonList = result.Items.Select(Map).ToList()
            };
        }

        private Person Map(Dictionary<string, AttributeValue> result)
        {
            return new Person
            {
                Id = Convert.ToInt32(result["Id"].N),
                FirstName = result["FirstName"].S,
                LastName = result["LastName"].S,
                BirthDate = Convert.ToDateTime(result["BirthDate"].S)
            };
        }

        private async Task<ScanResponse> ScanAsync(ScanRequest request)
        {
            var response = await _dynamoClient.ScanAsync(request);

            return response;
        }

        private ScanRequest RequestBuilder(int? id)
        {
            if (id.HasValue == false)
            {
                return new ScanRequest
                {
                    TableName = "POC_Person"
                };
            }

            return new ScanRequest
            {
                TableName = "POC_Person",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {
                        ":v_Id", new AttributeValue { N = id.ToString()}}

                },
                FilterExpression = "Id = :v_Id",
                ProjectionExpression = "Id, FirstName, LastName, BirthDate"
            };
        }
    }
}
