using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDbContext _context;

        public VillaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Villa entity)
        {
            _context.Add(entity);
        }

        public Villa Get(Expression<Func<Villa, bool>> filter, string? includeProperties = null)
        {
            IQueryable<Villa> query = _context.Set<Villa>();

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries)) 
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Villa> query = _context.Set<Villa>();

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries)) 
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.ToList();
        }

        public void Remove(Villa entity)
        {
            _context.Remove(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Villa entity)
        {
            _context.Update(entity);
        }
    }
}
