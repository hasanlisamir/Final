using DataAccess.Models;
using DataAccess.Paging;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        protected DbSet<T> Query { get; }
        public Task<PageableListResponse<T>> GetListPagedAsync(int pageNumber, int pageSize);

        public void Add(T entity);
        public Task<List<T>> GetAllAsync();
        public T Get(int id);
        public void Update(T entity);
        public void Delete(int id);

    }
}
