using TrainingDotnetAPI.Models;

namespace TrainingDotnetAPI.Services.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser username, IList<string> roles);
    }
}
