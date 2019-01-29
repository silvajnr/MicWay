using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicwayTech.DAL
{
    /// <summary>
    /// Generic Repository interface
    /// </summary>
    /// <typeparam name="T">Class</typeparam>
    public interface IDataStore<T>
    {
        Task<string> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<T> GetItemByIdAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<T>> GetItemsByPredicateAsync(Func<T, bool> predicate);
    }
}
