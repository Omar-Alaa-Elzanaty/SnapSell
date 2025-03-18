using System.Net;

namespace SnapSell.Domain.ResultDtos
{
    public class PaginatedResult<T>:Result<List<T>>
    {

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
