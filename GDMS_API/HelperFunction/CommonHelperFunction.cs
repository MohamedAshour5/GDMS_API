using System.ComponentModel;
using GDMS_API.Enums;
using GDMS_API.Interfaces;
using GDMS_API.Models;

namespace GDMS_API.HelperFunction
{
    public class CommonHelperFunction : ICommonHelperFunction
    {
        private readonly IConfiguration _configuration;
        public CommonHelperFunction(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetDepartementKeyFromValue(string value)
        {
            var JWTSettings = _configuration.GetSection("JWTSettings");
            foreach (var section in JWTSettings.GetChildren())
            {
                foreach (var child in section.GetChildren())
                {
                    if (child.Value == value)
                    {
                        return child.Key;
                    }
                }
            }
            return null;
        }
        public int? checkStatusIsFound(int value)
        {
            var status = new List<int>();
            var section = _configuration.GetSection("PropertyStatus");
            foreach (var child in section.GetChildren())
            {
                status.Add(int.Parse(child.Key));
            }
            int? result = status.Find(x => x == value);
            return result;
        }
    }
}
