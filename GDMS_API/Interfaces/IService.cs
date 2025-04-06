using GDMS_API.Models;
using System.Runtime.CompilerServices;

namespace GDMS_API.Interfaces
{
    public interface IService
    {
        Task<OwnerInfos> GetTitleByRen(string REN , string Token );
        Task<string> GetToken();
    }
}
