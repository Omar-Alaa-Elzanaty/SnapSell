using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Linq.Expressions;
using System.Reflection;

namespace SnapSell.Application.Extensions
{
    public static class MongoQueryableExtensions
    {
        public static async Task<PaginatedResult<T>> ToMongoPaginatedListAsync<T>(
            this IQueryable<T> source,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 10 : pageSize;
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;

            long count = await source.CountAsync(cancellationToken);

            List<T> items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return PaginatedResult<T>.Create(items, (int)count, pageNumber, pageSize);
        }

        public static async Task<PaginatedResult<T>> ToMongoPaginatedListAsync<T>(
            this IMongoCollection<T> collection,
            FilterDefinition<T> filter,
            SortDefinition<T> sort,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 10 : pageSize;
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;

            long count = await collection.CountDocumentsAsync(filter:filter, cancellationToken: cancellationToken);

            var items = await collection.Find(filter)
                .Sort(sort)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            return PaginatedResult<T>.Create(items, (int)count, pageNumber, pageSize);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string sorting)
        {
            if (string.IsNullOrWhiteSpace(sorting))
            {
                return query;
            }

            var fields = sorting.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Trim());

            bool first = true;
            foreach (string field in fields)
            {
                bool ascending = true;
                int spaceIndex = field.IndexOf(' ');
                string key = field;

                if (spaceIndex >= 0)
                {
                    key = field.Substring(0, spaceIndex);
                    string direction = field.Substring(spaceIndex + 1).Trim().ToLower();

                    if (!string.IsNullOrWhiteSpace(direction))
                    {
                        ascending = direction switch
                        {
                            "asc" => true,
                            "desc" => false,
                            _ => throw new ArgumentException("Invalid sorting direction")
                        };
                    }
                }

                query = ascending
                    ? ApplyOrder(query, key, first ? "OrderBy" : "ThenBy")
                    : ApplyOrder(query, key, first ? "OrderByDescending" : "ThenByDescending");

                first = false;
            }

            return query;
        }

        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            foreach (string prop in props)
            {
                PropertyInfo propertyInfo = type.GetProperty(
                    prop,
                    BindingFlags.IgnoreCase |
                    BindingFlags.Public |
                    BindingFlags.Instance)!;

                if (propertyInfo is null)
                    throw new ArgumentException($"Property {prop} not found on type {type.Name}");

                expr = Expression.Property(expr, propertyInfo);
                type = propertyInfo.PropertyType;
            }

            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods()
                .Single(method =>
                    method.Name == methodName &&
                    method.IsGenericMethodDefinition &&
                    method.GetGenericArguments().Length == 2 &&
                    method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { source, lambda })!;

            return (IOrderedQueryable<T>)result;
        }
    }
}