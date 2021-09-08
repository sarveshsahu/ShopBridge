using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBridge.Model;
using ShopBridge.Interface;
using ShopBridge.Business;
using Microsoft.Extensions.Logging;

namespace ShopBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger _logger;
        IInventory _IInventory;
        public InventoryController(IInventory IInventory)
        {
            _IInventory = IInventory;
        }

        [HttpGet]
        [Route("GetAllItems")]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var categories = await _IInventory.GetAllItems();
                if (categories == null)
                {
                    return NotFound();
                }

                return Ok(categories);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("GetItem")]
        public async Task<IActionResult> GetItem(int? itemId)
        {
            if (itemId == null)
            {
                return BadRequest();
            }

            try
            {
                var item = await _IInventory.Item(itemId);

                if (item == null)
                {
                    return NotFound();
                }

                return Ok(item);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Route("AddItem")]
        public async Task<IActionResult> AddItem([FromBody]Inventory model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _IInventory.CheckItemNameAlready(model.Name))
                    {
                        return StatusCode(409, $"Item '{model.Name}' already exists.");
                    }

                    var itemId = await _IInventory.SaveItem(model);
                    if (itemId > 0)
                    {
                        return Ok(itemId);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception)
                {

                    return BadRequest();
                }

            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("DeleteItem")]
        public async Task<IActionResult> DeleteItem(int? itemId)
        {
            int result = 0;

            if (itemId == null)
            {
                return BadRequest();
            }

            try
            {
                result = await _IInventory.DeleteItem(itemId);
                if (result == 0)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }


        [HttpPut]
        [Route("UpdateItem")]
        public async Task<IActionResult> UpdateItem([FromBody]Inventory model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    if (model.ItemID != 0)
                    {
                        await _IInventory.UpdateItem(model);

                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    if (ex.GetType().FullName ==
                             "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                    {
                        return NotFound();
                    }

                    return BadRequest();
                }

            }

            return BadRequest();
        }

    }
}