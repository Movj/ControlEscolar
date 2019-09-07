using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CE.API.Services.PaginationServices;
using System.Linq.Dynamic.Core;

namespace CE.API.Extensions
{
    public class ApplySort : IApplySort
    {
        IQueryable<T> IApplySort.ApplySort<T>(IQueryable<T> source, string orderBy, Dictionary<string, PropertyMappingValue> mappingDictinary)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (mappingDictinary == null)
            {
                throw new ArgumentNullException("mappingDictinary");
            }

            if (string.IsNullOrWhiteSpace(orderBy)) return source;

            // The orderBy string is separated by "," ; so we split it
            var orderByAfterSplit = orderBy.Split(',');

            // Apply each orderBy clause in reverse order - otherwise, the
            // IQuerable will be ordered in the wrong order

            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                // Trim the orderByClause, as it might contain leading
                // or trailing spaces. Can't trim the var in foreach,
                // so use another var
                var trimmedOrderByClause = orderByClause.Trim();

                // If the sort options ends with " desc", we order
                // descending, otherwise ascending 
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                // Remove " asc" or " desc" from the orderByClause, so we
                // get the propery name to look for in the mapping dictionary
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                // Find the matching property
                if (!mappingDictinary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                // Get the propertyMappingValue
                var propertyMappingValue = mappingDictinary[propertyName];

                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                // Run through the property names in reverse
                // so the orderBy clauses are applied in the correct order
                foreach (var destinacionProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    // Revert sort order if necessary
                    if (propertyMappingValue.Revert) orderDescending = !orderDescending;

                    source = source.OrderBy(destinacionProperty + (orderDescending ? " descending" : " ascending"));
                }
            }

            return source;
        }

    }
}
