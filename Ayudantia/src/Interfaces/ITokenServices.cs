using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Models;

namespace Ayudantia.Src.Interfaces
{
    public interface ITokenServices
    {
        string GenerateToken(User user, string role);
    }
}