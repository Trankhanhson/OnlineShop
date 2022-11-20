using Microsoft.Ajax.Utilities;
using Project_3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.common
{
    public static class Paggination
    {
        public static PagedData<T> PagedResult<T>(this List<T> list,int PageNumber, int PageSize ) where T : class
        {
            var result = new PagedData<T>();
            result.Data = list.Skip(PageSize * (PageNumber - 1)).Take(PageSize).ToList();
            result.TotalCount = list.Count;

            return result;
        }
    }
}