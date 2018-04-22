namespace FBru.WebClient.Helpers
{
    public class PagingInfo
    {
        public PagingInfo(int totalCount, int totalPages, int currentPage,
            int pageSize, string previousPageLink, string nextPageLink)
        {
            TotalCount = totalCount;
            TotalPages = totalPages;
            CurrentPage = currentPage;
            PageSize = pageSize;
            PreviousPageLink = previousPageLink;
            NextPageLink = nextPageLink;
        }

        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public string PreviousPageLink { get; set; }
        public string NextPageLink { get; set; }
    }
}