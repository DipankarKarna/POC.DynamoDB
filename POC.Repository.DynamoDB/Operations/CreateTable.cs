using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace POC.Repository.DynamoDB.Operations
{
    public interface ICreateTable
    {
        void CreateDynamoDbTable();
    }

    public class CreateTable : ICreateTable
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private static readonly string tableName = "POC_Person";

        public CreateTable(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public void CreateDynamoDbTable()
        {
            try
            {
                CreateTempTable();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void CreateTempTable()
        {
            Console.WriteLine("Creating Table");

            var request = new CreateTableRequest
            {
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        AttributeType = "N"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "FirstName",
                        AttributeType = "S"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "LastName",
                        AttributeType = "S"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "BirthDate",
                        AttributeType = "S"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "BirthYear",
                        AttributeType = "N"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType = "HASH" // Partition Key
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "BirthYear",
                        KeyType = "Range" // Sort Key
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                TableName = tableName
            };

            try
            {
                var response = _dynamoDbClient.CreateTableAsync(request);
            }
            catch(Exception ex)
            {

            }

            WaitUntilTableReady(tableName);
        }

        public void WaitUntilTableReady(string tableName)
        {
            string status = null;

            do
            {
                Thread.Sleep(2000);
                try
                {
                    var res = _dynamoDbClient.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = tableName
                    });

                    if (res != null)
                        if (res.Result != null)
                            if (res.Result.Table != null)
                                if (res.Result.Table.TableStatus != null)
                                    status = res.Result.Table.TableStatus;
                }
                catch (ResourceNotFoundException)
                {

                }

            } while (status != "ACTIVE");
            {
                Console.WriteLine("Table Created Successfully");
            }
        }
    }
}
