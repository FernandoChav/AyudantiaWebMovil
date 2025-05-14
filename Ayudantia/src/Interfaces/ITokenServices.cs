

using Ayudantia.Src.Models;

namespace Ayudantia.Src.Interfaces
{
    public interface ITokenServices
    {
        string GenerateToken(User user, string role);
    }
}