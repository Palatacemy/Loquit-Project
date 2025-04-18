﻿using Loquit.Data.Entities.Abstractions;
using Loquit.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Data.Repositories
{
    //basic CRUD methods
    public class CrudRepository<T> : ICrudRepository<T>
        where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public CrudRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"Item does not exist: {id}");
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate)
                .ToListAsync();
        }
        public IEnumerable<T> GetSync(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate)
                .ToList();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            //_context.Entry(entity).State = EntityState.Modified;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
    }

}
