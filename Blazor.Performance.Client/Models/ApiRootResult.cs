using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Performance.Client.Models
{
    public class ApiRootResult<T>
    {
        public List<T> Items { get; set; }
        public int ItemCount { get; set; }
    }
}
