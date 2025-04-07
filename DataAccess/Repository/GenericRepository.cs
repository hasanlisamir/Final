using DataAccess.Data;
using DataAccess.Models;
using DataAccess.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DataAccess.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel

    {
        private readonly CarMSAppDbContext _context;

        public DbSet<T> Query => _context.Set<T>();


        public GenericRepository(CarMSAppDbContext context)
        {
            _context = context;

        }

        public void Add(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();//set
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public T Get(int id)
        {

            return _context.Set<T>().Find(id);

        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public void Delete(int id)
        {
            var entity= _context.Set<T>().Find(id);
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public async Task<PageableListResponse<T>> GetListPagedAsync(int pageNumber, int pageSize)
        {
            var query = Query.AsQueryable();
            return await query.GetListPagedAsync(pageNumber, pageSize);
        }


    }
}
