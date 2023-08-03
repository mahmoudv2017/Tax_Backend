using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork<T> where T: class
    {
        Task Add(T entity);
        Task<T?> GetOneEntityByExpression(Expression<Func<T , bool>> condition);
        Task<List<T>> GetDataBasedOnMonthandID(Expression<Func<T, bool>> filters, Expression<Func<T, object>> IncludesList);
        Task SaveChangeAsync();
        Task ChangeStatus(TaxHistory tax, ActionStatus status);

    }
}
