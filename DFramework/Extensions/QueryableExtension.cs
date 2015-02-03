using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DFramework
{
    public static class QueryableExtension
    {

        /// <summary>
        /// orderby
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="source">source list</param>
        /// <param name="propertyName">order feild</param>
        /// <param name="order">sort:ASC Or DESC</param>
        /// <returns>sorted list</returns>
        [DebuggerStepThrough]
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, string order)
            where T : class
        {
            Type type = typeof(T);

            PropertyInfo property = type.GetProperty(propertyName);
            if (property == null) throw new ArgumentException("propertyName", "Not Exist");

            ParameterExpression param = Expression.Parameter(type, "p");

            Expression propertyAccessExpression = Expression.MakeMemberAccess(param, property);

            LambdaExpression orderByExpression = Expression.Lambda(propertyAccessExpression, param);

            string methodName = order.ToLower().Equals("desc") ? "OrderByDescending" : "OrderBy";

            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName,
                                                             new Type[] { type, property.PropertyType },
                                                                           source.Expression,
                                                                           Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
