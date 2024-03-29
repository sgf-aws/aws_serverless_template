using System.Threading.Tasks;
using Amazon.ApiGatewayManagementApi;
using Amazon.DynamoDBv2.DataModel;
using Amazon.IotData;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Common;
using Microsoft.Extensions.DependencyInjection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Messages
{
    public class Function
    {
        private readonly ServiceProvider _serviceProvider;

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IEnvironmentWrapper, EnvironmentWrapper>();
            serviceCollection.AddTransient<ILambdaService, LambdaService>();
            serviceCollection.AddTransient<IResponseWrapper, ResponseWrapper>();            

            serviceCollection.AddTransient<IDynamoDbContextWrapper, DynamoDbContextWrapper>(
                x => DynamoDbConfig.CreateConfiguredDbContextWrapper());
            serviceCollection.AddTransient<IAmazonApiGatewayManagementApi, AmazonApiGatewayManagementApiClient>(
                x => ApiGatewayConfig.CreateConfiguredApiGatewayManagementApiClient());
            serviceCollection.AddTransient<IAmazonIotData, AmazonIotDataClient>(
                x => IotConfig.CreateAmazonIotDataClient());
        }

        public Function()
        { 
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
        
        public Task<APIGatewayProxyResponse> GetMessages(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return _serviceProvider
                .GetService<ILambdaService>()
                .GetMessages(request, context);
        }
        
        public Task<APIGatewayProxyResponse> PostMessage(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return _serviceProvider
                .GetService<ILambdaService>()
                .PostMessage(request, context);
        }

        public Task NotifyMessageUpdate(DynamoDBEvent update, ILambdaContext context)
        {
            return _serviceProvider
                .GetService<ILambdaService>()
                .NotifyMessageUpdate(update, context);
        }
    }
}
