using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Services.PaginationServices
{
    public class PropertyMappingService<T1,T2>
        : IPropertyMappingService<T1, T2> where T1 : class where T2 : class
    {
        private IList<IPropertyMapping> propertyMappings;

        public PropertyMappingService()
        {
            propertyMappings = new List<IPropertyMapping>();
        }


        public Dictionary<string, PropertyMappingValue> GetPropertyMapping(Dictionary<string, PropertyMappingValue> propertyMappingProfile)
        {
            propertyMappings.Add(new PropertyMapping<T1, T2>(propertyMappingProfile));
            // Get matching mapping
            var matchingMapping = propertyMappings.OfType<PropertyMapping<T1, T2>>();

            if (matchingMapping.Count() == 1) return matchingMapping.First()._mappingDictionary;

            throw new Exception($"Cannot find exact property mapping instance of <{typeof(T1)}>");
        }

        public bool ValidMappingExistsFor(string fields, Dictionary<string, PropertyMappingValue> propertyMappingProfile)
        {
            var propertyMapping = GetPropertyMapping(propertyMappingProfile);

            if (string.IsNullOrWhiteSpace(fields)) return true;

            // The string is separated by "," so we split it
            var fieldsAfterSplit = fields.Split(',');

            // Run through the fields clauses
            foreach (var field in fieldsAfterSplit)
            {
                // Trim
                var trimmedField = field.Trim();

                // Remove everything after the first " " - if the fields
                // are coming from an orderBy string, this part must be ignored
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                // Find the matching property
                if (!propertyMapping.ContainsKey(propertyName)) return false;
            }

            return true;
        }
    }
}
