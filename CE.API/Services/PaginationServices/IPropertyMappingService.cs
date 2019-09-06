using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Services.PaginationServices
{
    public interface IPropertyMappingService<T1, T2> where T1:class where T2:class
    {
        //bool ValidMappingExistsFor<TSource, TDestination>(string fields);
        //Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool ValidMappingExistsFor(string fields, Dictionary<string, PropertyMappingValue> propertyMappingProfile);
        Dictionary<string, PropertyMappingValue> GetPropertyMapping(Dictionary<string, PropertyMappingValue> propertyMappingProfile);
    }
}
