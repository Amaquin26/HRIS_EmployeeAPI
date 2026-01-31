using HRIS_Employee.API.DTOs;

namespace HRIS_Employee.API.External.Graph
{
    public interface IGraphService
    {
        Task<EntraUserDto?> GetUser(string userPrincipalName);

        Task<PaginatedItemsDto<EntraUserDto>> GetPaginatedUsers(PaginationQueryDto pagination);
    }
}
