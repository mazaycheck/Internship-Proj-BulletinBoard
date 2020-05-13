using System;
using System.Linq.Expressions;

namespace Baraholka.Data.Dtos
{
    public class OrderParams<T>
    {
        public Expression<Func<T, object>> OrderBy { get; set; }
        public bool Descending { get; set; }
    }
}