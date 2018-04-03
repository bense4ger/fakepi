using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml;
using FakePI.Lib.Entities;

namespace FakePI.Lib.Data.Contexts.Contracts
{
    public interface IDataContext<T> where T: Entity
    {
        Task<T> GetAsync(Guid id);
        Task<ICollection<T>> GetAllAsync();
        Task<bool> PutAsync(T entity);
        Task<bool> PutManyAsync(ICollection<T> entities);
    }
}