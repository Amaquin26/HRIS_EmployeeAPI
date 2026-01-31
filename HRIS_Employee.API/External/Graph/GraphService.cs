using Azure;
using Azure.Identity;
using HRIS_Employee.API.DTOs;
using HRIS_Employee.Infrastructure.Persistence.Models;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace HRIS_Employee.API.External.Graph
{
    public class GraphService : IGraphService
    {
        private readonly GraphServiceClient _graphClient;

        public GraphService (IOptions<AzureAdSettings> azureAdSettings)
        {            
            var tenantId = azureAdSettings.Value.TenantId;
            var clientId = azureAdSettings.Value.ClientId;
            var clientSecret = azureAdSettings.Value.ClientSecretKey;

            var options = new TokenCredentialOptions
            {
                AuthorityHost = new Uri($"{azureAdSettings.Value.Instance}{tenantId}")
            };

            var clientSecretCredential = new ClientSecretCredential(
                tenantId,
                clientId,
                clientSecret,
                options
            );

            var scopes = new[] { "https://graph.microsoft.com/.default" };

            _graphClient = new GraphServiceClient(clientSecretCredential, scopes);
        }

        public async Task<EntraUserDto?> GetUser(string userPrincipalName)
        {

            var result = await _graphClient.Users.GetAsync(requestConfig =>
            {
                requestConfig.Headers.Add("ConsistencyLevel", "eventual");

                requestConfig.QueryParameters.Search =
                    $"\"displayName:{userPrincipalName}\" OR \"mail:{userPrincipalName}\"";


                requestConfig.QueryParameters.Select = new[]
                {
                    "id",
                    "displayName",
                    "mail",
                    "givenName",
                    "surname",
                    "userPrincipalName",
                    "jobTitle",
                    "mobilePhone",
                    "officeLocation"
                };

                requestConfig.QueryParameters.Top = 10;
            });

            var graphUser = result?.Value?.FirstOrDefault();
            if (graphUser == null)
                return null;

            return new EntraUserDto
            {
                ObjectId = graphUser.Id!,
                DisplayName = graphUser.DisplayName!,
                Email = graphUser.Mail ?? graphUser.UserPrincipalName,
                GivenName = graphUser.GivenName,
                Surname = graphUser.Surname,
                JobTitle = graphUser.JobTitle,
                MobilePhone = graphUser.MobilePhone,
                OfficeLocation = graphUser.OfficeLocation
            };
        }

        public async Task<PaginatedItemsDto<EntraUserDto>> GetPaginatedUsers(PaginationQueryDto pagination)
        {

            UserCollectionResponse? result;

            if (string.IsNullOrEmpty(pagination.SkipToken))
            {
                result = await _graphClient.Users.GetAsync(requestConfig =>
                {
                    requestConfig.Headers.Add("ConsistencyLevel", "eventual");

                    if (!string.IsNullOrWhiteSpace(pagination.SearchTerm))
                    {
                        requestConfig.QueryParameters.Search =
                            $"\"displayName:{pagination.SearchTerm}\" OR \"mail:{pagination.SearchTerm}\"";
                    }

                    requestConfig.QueryParameters.Select = new[]
                    {
                    "id",
                    "displayName",
                    "mail",
                    "givenName",
                    "surname",
                    "userPrincipalName",
                    "jobTitle",
                    "mobilePhone",
                    "officeLocation"
                };

                    requestConfig.QueryParameters.Top = pagination.PageSize;
                    requestConfig.QueryParameters.Count = true;
                });
            }
            else
            {

                result = await _graphClient.Users
                .WithUrl(pagination.SkipToken)
                .GetAsync();
            }

            var users = result?.Value ?? [];

            return new PaginatedItemsDto<EntraUserDto>
            {
                SkipToken = result?.OdataNextLink,
                TotalRecords = result?.OdataCount != null ? (int)result.OdataCount : 0,
                Items = users.Select(u => new EntraUserDto
                {
                    ObjectId = u.Id!,
                    DisplayName = u.DisplayName!,
                    Email = u.Mail ?? u.UserPrincipalName,
                    GivenName = u.GivenName,
                    Surname = u.Surname,
                    JobTitle = u.JobTitle,
                    MobilePhone = u.MobilePhone,
                    OfficeLocation = u.OfficeLocation
                }).ToList()
            };
        }
    }
}
