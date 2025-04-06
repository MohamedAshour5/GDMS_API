using GDMS_API.Enums;
using GDMS_API.Models;
using System.Runtime.CompilerServices;

namespace GDMS_API.Interfaces
{
    public interface ICommonHelperFunction
    {
        public string GetDepartementKeyFromValue(string value);
        int? checkStatusIsFound(int value);



    }
}
