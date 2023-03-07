﻿namespace CI_Project_Main
{
    //it is generic class
    public class PaginationList<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public string searchTerm { get; set; }

        public PaginationList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            //12 count in database : page size -> 5 -- 12 / 5 -- 3 
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static PaginationList<T> Create(List<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count;

            //main thing
            //at first index is 1 so what it does : skip ( 1 - 1 ) * 5 = 0 and take ( 5 )
            //in second skip( 2 - 1 ) * 5 = 5 and take ( 5 ) next skip 10 .. 15 .. so on
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginationList<T>(items, count, pageIndex, pageSize);

        }
    }
}
