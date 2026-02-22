using HRIS_Employee.API.DTOs;

namespace HRIS_Employee.API.Services
{
    public interface IAuthUserDetailsService
    {
        Task<AuthUserDetailsDto?> GetMyUserDetails(string entraObjectId);
    }
}
