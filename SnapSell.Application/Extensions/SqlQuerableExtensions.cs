using Microsoft.EntityFrameworkCore;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Linq.Expressions;
using System.Reflection;

namespace SnapSell.Application.Extensions
{
    public static class SqlQuerableExtensions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 10 : pageSize;
            int count = await source.CountAsync();
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return PaginatedResult<T>.Create(items, count, pageNumber, pageSize);
        }

        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string sorting)
        {
            if (string.IsNullOrWhiteSpace(sorting))
            {
                return query;
            }

            var fields = sorting.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
            bool first = true;
            foreach (string field in fields)
            {
                bool ascending = true;
                int spaceIndex = field.IndexOf(' ');
                string key = field;
                if (spaceIndex >= 0)
                {
                    key = field.Substring(0, spaceIndex);
                    string dir = field.Substring(spaceIndex + 1).Trim().ToLower();
                    if (!string.IsNullOrWhiteSpace(dir))
                    {
                        switch (dir)
                        {
                            case "asc":
                                ascending = true;
                                break;
                            case "desc":
                                ascending = false;
                                break;
                            default:
                                throw new ArgumentException("Invalid sorting direction");
                        }
                    }
                }

                query = ascending
                    ? ApplyOrder(query, key, first ? "OrderBy" : "ThenBy")
                    : ApplyOrder(query, key, first ? "OrderByDescending" : "ThenByDescending");
                first = false;
            }

            return query;
        }

        static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (pi is null)
                {
                    throw new ArgumentException("Invalid sorting property!");
                }
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(method
                => method.Name == methodName
                && method.IsGenericMethodDefinition
                && method.GetGenericArguments().Length == 2
                && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, [source, lambda]);

            return (IOrderedQueryable<T>)result;
        }
    }
}