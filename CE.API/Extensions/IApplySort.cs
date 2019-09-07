using CE.API.Services.PaginationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.API.Extensions
{
    public interface IApplySort
    {
        IQueryable<T> ApplySort<T>(IQueryable<T> source, string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictinary);
    }
}
