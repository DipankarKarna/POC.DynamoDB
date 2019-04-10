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
    public interface IUpdatePerson
    {
        Task<Person> Update(int id, string FirstName, string LastName, DateTime BirthDate);
    }
    public class UpdateItem : IUpdatePerson
    {
        private readonly IGetPerson _getPerson;
        private static readonly string tableName = "POC_Person";
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public UpdateItem(IGetPerson getPerson, IAmazonDynamoDB dynamoDbClient)
        {
            _getPerson = getPerson;
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task<Person> Update(int id, string FirstName, string LastName, DateTime BirthDate)
        {
            var response = await _getItem.GetItems(id);

            var currentPrice = response.Items.Select(p => p.Price).FirstOrDefault();

            var replyDateTime = response.Items.Select(p => p.ReplyDateTime).FirstOrDefault();

            var request = RequestBuilder(id, price, currentPrice, replyDateTime);

            var result = await UpdateItemAsync(request);

            return new Item
            {
                Id = Convert.ToInt32(result.Attributes["Id"].N),
                ReplyDateTime = result.Attributes["ReplyDateTime"].N,
                Price = Convert.ToDouble(result.Attributes["Price"].N)
            };
        }

        private UpdateItemRequest RequestBuilder(int id, string FirstName, string LastName, DateTime BirthDate)
        {
            var request = new UpdateItemRequest
            {
                Key = new Dictionary<string, AttributeValue>
                {
                    {
                        "Id", new AttributeValue
                        {
                            N = id.ToString()
                        }
                    },
                    {
                        "FirstName", new AttributeValue
                        {
                            S = FirstName
                        }
                    },
                    {
                        "LastName", new AttributeValue
                        {
                            S = LastName
                        }
                    },
                    {
                        "BirthDate", new AttributeValue
                        {
                            S = BirthDate.ToString()
                        }
                    }
                },
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#N", "Id"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {
                        ":newprice", new AttributeValue
                        {
                            N = price.ToString()
                        }
                    },
                    {
                        ":currprice", new AttributeValue
                        {
                            N = currentPrice.ToString()
                        }
                    }
                },

                UpdateExpression = "SET #P = :newprice",
                ConditionExpression = "#P = :currprice",

                TableName = tableName,
                ReturnValues = "ALL_NEW"
            };

            return request;
        }
        private async Task<UpdateItemResponse> UpdateItemAsync(UpdateItemRequest request)
        {
            var response = await _dynamoDbClient.UpdateItemAsync(request);

            return response;
        }
    }
}
