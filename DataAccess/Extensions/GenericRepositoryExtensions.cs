using DataAccess.Models;
using DataAccess.Paging;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Paging
{
    public static class GenericRepositoryExtensions
    {
        public static async Task<PageableListResponse<T>> GetListPagedAsync<T>(
            this IQueryable<T> query, int pageNumber, int pageSize) where T : BaseModel
        {
            var totalCount = await query.CountAsync();

            var data = await query.Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return new PageableListResponse<T>
            {
                Data = data,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                TotalPages = totalPages
            };
        }
    }
}
