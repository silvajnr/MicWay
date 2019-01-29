using Microsoft.Extensions.Logging;
using MicwayTech.Data;
using MicwayTech.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicwayTech.DAL
{
    /// <summary>
    /// Driver repository implementation
    /// </summary>
    public class DriverDataStore : IDriverDataStore
    {
        #region Private Properties
        /// <summary>
        /// Database context
        /// </summary>
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        /// <summary>
        /// Drivers loaded items, because it's Web api and the lifecycle is too short to be used
        /// </summary>
        private IEnumerable<Driver> _items;
        #endregion

        #region Contructor
        /// <summary>
        /// Injecting ApplicationdbContext and ILogger
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        public DriverDataStore(ApplicationDbContext dbContext,
        ILogger<DriverDataStore> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        #endregion

        #region Interface Implementation
        /// <summary>
        /// Get Items Async (cached)
        /// </summary>
        /// <param name="forceRefresh">to enable cache select false</param>
        /// <returns> returns a list of Drivers from database</returns>
        public async Task<IEnumerable<Driver>> GetItemsAsync(bool forceRefresh = true)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (forceRefresh)
                    {
                        _items = _dbContext.Drivers.ToList();
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"GetItemsAsync forceRefresh:{forceRefresh} Error");
                    return null;
                }
                return _items;
            });
        }

        /// <summary>
        /// Get items by Predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Driver>> GetItemsByPredicateAsync(Func<Driver, bool> predicate)
        {
            return await Task.Run(() =>
            {
                IEnumerable<Driver> drivers = null;
                try
                {
                    drivers = _dbContext.Drivers.Where(predicate);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"GetItemsByPredicateAsync predicate:{predicate}  Error");
                    return null;
                }
                return drivers;
            });
        }

        public async Task<Driver> GetItemByIdAsync(string id)
        {
            return await Task.Run(() =>
            {
                Driver driver = null;
                try
                {
                    driver = _dbContext.Drivers.FirstOrDefault(d => d.Id == id);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"GetItemByIdAsync id:{id}  Error");
                    return null;
                }
                return driver;
            });
        }

        public async Task<string> AddItemAsync(Driver item)
        {
            try
            {
                var result = await _dbContext.Drivers.AddAsync(item);
                var changeResult = await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"AddItemAsync Driver:{item.Id},{item.FirstName}, {item.LastName}, {item.DOB}, {item.Email}  Error");
                return null;
            }

            return item.Id;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            try
            {
                var driver = _dbContext.Drivers.FirstOrDefault(d => d.Id == id);
                if (driver == null)
                {
                    return false;
                }

                var result = _dbContext.Drivers.Remove(driver);
                var changeResult = await _dbContext.SaveChangesAsync();

                if (changeResult == 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"AddItemAsync id:{id} Error");
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateItemAsync(Driver item)
        {
            try
            {
                var driver = _dbContext.Drivers.FirstOrDefault(d => d.Id == item.Id);
                if (driver == null)
                {
                    return false;
                }

                driver.FirstName = item.FirstName;
                driver.LastName = item.LastName;
                driver.DOB = item.DOB;
                driver.Email = item.Email;

                var changeResult = await _dbContext.SaveChangesAsync();
                if (changeResult == 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"AddItemAsync Driver:{item.Id},{item.FirstName}, {item.LastName}, {item.DOB}, {item.Email} Error");
                return false;
            }
            return true;
        }
        #endregion
    }
}
