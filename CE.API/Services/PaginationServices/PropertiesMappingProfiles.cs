using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Services.PaginationServices
{
    public class PropertiesMappingProfiles
    {
        public static Dictionary<string, PropertyMappingValue> _userPropertyMinInfoMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>(){ "Id"}) },
                {"NombreCompleto", new PropertyMappingValue(new List<string>(){ "ApellidoPaterno", "ApellidoMaterno", "Nombre"}, true) },
                {"Email", new PropertyMappingValue(new List<string>(){ "Email"}) },
                {"TelefonoCelular", new PropertyMappingValue(new List<string>(){ "Email" }) },
                {"AnioNacimiento", new PropertyMappingValue(new List<string>(){ "AnioNacimiento"}) }
            };

        public static Dictionary<string, PropertyMappingValue> _rolePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>(){ "Id"}) },
                {"RoleName", new PropertyMappingValue(new List<string>(){ "RoleName"}) }
            };
    }
}
