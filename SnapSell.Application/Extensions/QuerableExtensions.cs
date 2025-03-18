namespace SnapSell.Application.Extensions
{
    public static class QuerableExtensions
    {
        public static Task<IQueryable<T>> ToPaginatedListAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public static Task<IQueryable<T>> OrderBy<T>(this IQueryable<T> query, string type, string column)
        {
            throw new NotImplementedException();
        }
    }
}
