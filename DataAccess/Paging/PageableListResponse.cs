﻿namespace DataAccess.Paging
{
    public class PageableListResponse<T>
    {
        public List<T> Data { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
