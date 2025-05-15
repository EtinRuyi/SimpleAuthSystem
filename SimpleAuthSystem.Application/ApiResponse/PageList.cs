namespace SimpleAuthSystem.Application.ApiResponse
{
    public class PageList<T>
    {
        public List<T> Users { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public PageList(List<T> users, int totalCount, int pageSize, int currentPage)
        {
            PageSize = pageSize <= 0 ? 50 : Math.Min(pageSize, 50);
            CurrentPage = currentPage <= 0 ? 1 : currentPage;

            Users = users;
            TotalCount = totalCount;
            TotalPages = PageSize > 0 ? (int)Math.Ceiling((double)totalCount / PageSize) : 0;
        }

        public static PageList<T> Create<TSource>(
            IEnumerable<TSource> source,
            int totalCount,
            int pageSize,
            int currentPage,
            Func<IEnumerable<TSource>, List<T>> mapper)
        {
            int effectivePageSize = pageSize <= 0 ? 50 : Math.Min(pageSize, 50);
            int effectivePage = currentPage <= 0 ? 1 : currentPage;

            var users = source
                .Skip((effectivePage - 1) * effectivePageSize)
                .Take(effectivePageSize)
                .ToList();

            return new PageList<T>(mapper(users), totalCount, effectivePageSize, effectivePage);
        }
    }
}
