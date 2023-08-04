using Core.Entities;
using Core.Interfaces;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        private readonly AppIdentityContext _context;
        public UnitOfWork(AppIdentityContext context)
        {
            _context= context;
        }
        public async Task Add(T entity)
        {
             await _context.AddAsync(entity);
        }

        public async Task ChangeStatus(TaxHistory tax, ActionStatus status)
        {
            tax.Status = status;
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetDataBasedOnMonthandID( Expression<Func<T,bool>> filters, Expression<Func<T, object>> IncludesList)
        {
            return await _context.Set<T>().Where(filters
            ).Include(IncludesList).ToListAsync();
        }

        public async Task<T?> GetOneEntityByExpression(Expression<Func<T, bool>> condition)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(condition);
        }

        public async Task SaveChangeAsync()
        {
           await _context.SaveChangesAsync();
        }
    }
}
