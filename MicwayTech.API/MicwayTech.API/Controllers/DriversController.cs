using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Micway_Tech_Test.DAL;
using Micway_Tech_Test.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Micway_Tech_Test.Controllers
{
    /// <summary>
    /// Web Api Controller 
    /// </summary>
    /// Url: /api/drivers
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        #region Private declarations
        private readonly IDriverDataStore _dataStore;
        private readonly ILogger _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Injecting IDriverDataStore and ILogger
        /// </summary>
        /// <param name="dbContext"></param>
        public DriversController(IDriverDataStore dataStore,
        ILogger<DriversController> logger)
        {
            _dataStore = dataStore;
            _logger = logger;
        }
        #endregion
        #region Methods
        // GET api/drivers/list
        /// <summary>
        ///  List all Drivers Method
        /// </summary>
        /// <returns>List of Drivers</returns>
        [HttpGet]
        [Route("List")]
        public async Task<ActionResult<IEnumerable<Driver>>> GetAll()
        {
            IEnumerable<Driver> drivers;
            try
            {
                //try to retrieve drivers and stores drivers' details
                drivers = await _dataStore.GetItemsAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "GET api/drivers/list Error");
                return NotFound();
            }
            return Ok(drivers);
        }

        // GET api/drivers/{id}/details
        /// <summary>
        /// Driver details by Id
        /// </summary>
        /// <param name="id">Driver's Id</param>
        /// <returns>Driver's details</returns>
        [HttpGet("{id}")]
        [Route("{id}/Details")]
        public async Task<ActionResult<Driver>> Get(string id)
        {
            Driver driver;
            try
            {
                //try to retrieve driver by Id and stores driver's details
                driver = await _dataStore.GetItemByIdAsync(id);
                //return Not Found http response for blank details
                if (driver == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "GET api/drivers/{id}/details Error");
                return NotFound();
            }

            return Ok(driver);
        }

        // POST api/drivers/insert
        /// <summary>
        /// Insert new driver
        /// </summary>
        /// <param name="value"> New driver's details</param>
        /// <returns> New driver's inserted details and url</returns>
        [HttpPost]
        [Route("Insert")]
        public async Task<ActionResult<Driver>> Post([FromBody] Driver value)
        {
            string result;
            try
            {
                //try to insert new driver's details and stores new driver's Id
                result = await _dataStore.AddItemAsync(value);
                //check if new driver's Id isn't empty
                if (string.IsNullOrEmpty(result))
                {
                    return BadRequest();
                }
                value.Id = result;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "POST api/drivers/insert Error");
                return NotFound();
            }
            //Return Created http response, new driver details and url from new created driver
            return CreatedAtAction(nameof(Get), new { id = value.Id }, value);
        }

        // PUT api/drivers/{id}/update
        /// <summary>
        /// Edit driver's details 
        /// </summary>
        /// <param name="id">Driver's Id</param>
        /// <param name="value">Driver's new details</param>
        /// <returns>Http code 204 No Content</returns>
        [HttpPut("{id}")]
        [Route("{id}/Update")]
        public async Task<ActionResult> Put(string id, [FromBody] Driver value)
        {
            bool result;
            try
            {
                //check if Id from url matches from driver details
                if (id != value.Id)
                {
                    return BadRequest();
                }
                //try to update driver's details 
                result = await _dataStore.UpdateItemAsync(value);
                //return not found http request if driver's not found or if failed
                if (!result)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "POST api/drivers/insert Error");
                return NotFound();
            }
            //return No content http request
            return NoContent();
        }

        // DELETE api/drivers/{id}/delete
        /// <summary>
        /// Delete driver's details by Id
        /// </summary>
        /// <param name="id">Driver's Id</param>
        /// <returns>Http code 204 No Content</returns>
        [HttpDelete("{id}")]
        [Route("{id}/Delete")]
        public async Task<ActionResult> Delete(string id)
        {
            bool result;
            try
            {
                //try to delete driver's details 
                result = await _dataStore.DeleteItemAsync(id);
                //return not found http request if driver's not found or if failed
                if (!result)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "DELETE api/drivers/{id}/delete Error");
                return NotFound();
            }
            //return No content http request
            return NoContent();
        }
        #endregion
    }
}