using api.Helpers;

namespace api.Service.Profile
{
    public interface IProfileService
    {
        Task<ServiceResponse<string>> GetProfileAsync(string username);
    }
}