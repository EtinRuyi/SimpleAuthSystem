namespace SimpleAuthSystem.Application.ApiResponse
{
    public class PageList<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public PageList(List<T> items, int totalCount, int pageSize, int currentPage)
        {
            Items = items;
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        }
    }
}
