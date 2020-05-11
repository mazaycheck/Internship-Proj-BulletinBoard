using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Linq.Expressions;

namespace WebApplication2.Data.Dtos
{
    public class OrderParams<T>
    {
        public Expression<Func<T, object>> OrderBy { get; set; }
        public bool Descending { get; set; }
    }
}
