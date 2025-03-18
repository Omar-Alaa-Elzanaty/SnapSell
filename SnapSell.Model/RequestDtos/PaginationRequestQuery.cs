namespace SnapSell.Domain.RequestDtos
{
    class PaginationRequestQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? KeyWord { get; set; }
    }
}
