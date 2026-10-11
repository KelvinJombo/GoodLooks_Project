using Identity.Api.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Identity.Api.Services
{
    public interface IExternalLoginService
    {
        Task<ExternalLoginResult> CompleteAsync(
            ExternalLoginInfo externalLoginInfo);
    }
}
