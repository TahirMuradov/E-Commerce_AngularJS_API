﻿using Microsoft.EntityFrameworkCore;

namespace Shop.Application.PaginationHelper
{
    public class PaginatedList<T>
    {
        public List<T> PaginatedData { get; set; }
        public int Page { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; set; }
        public int CollectionSize { get; set; }
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PaginatedData = items;
            Page = pageIndex;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            CollectionSize = count;
          
        }
        public bool HasNextPage => Page * PageSize < CollectionSize;
        public bool HasPreviousPage => Page > 1;
        /// <summary>
        /// Creates a paginated list from a queryable source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            int count = await source.CountAsync();
            if (pageSize == 0) pageSize = count;
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        /// <summary>
        /// Creates a paginated list from a list of items.
        /// This method is useful when you already have a list and want to paginate it without querying a database. 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static  PaginatedList<T> Create(List<T> source, int pageIndex, int pageSize)
        {
            int count =  source.Count;
            if (pageSize == 0) pageSize = count;
            var items =  source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
