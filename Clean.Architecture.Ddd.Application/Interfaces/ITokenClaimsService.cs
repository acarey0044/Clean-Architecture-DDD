using System.Threading.Tasks;

namespace Clean.Architecture.Ddd.Application.Interfaces
{
    public interface ITokenClaimsService
    {
        Task<string> GetTokenAsync(string userName);
    }
}
