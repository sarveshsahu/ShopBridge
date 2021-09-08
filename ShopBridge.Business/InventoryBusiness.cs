using System;
using System.Collections.Generic;
using System.Text;
using ShopBridge.Model;
using ShopBridge.Data;
using ShopBridge.Interface;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ShopBridge.Business
{

    public class InventoryBusiness : IInventory
    {
        DatabaseContext _context;

        public InventoryBusiness(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> SaveItem(Inventory item)
        {
            if (item != null)
            {
                await _context.Inventory.AddAsync(item);
                await _context.SaveChangesAsync();
                return item.ItemID;
            }
            return 0;
        }

        public async Task<int> DeleteItem(int? itemID)
        {
            int result = 0;

            if (_context != null)
            {
                var item = await _context.Inventory.FirstOrDefaultAsync(x => x.ItemID == itemID);

                if (item != null)
                {
                    _context.Inventory.Remove(item);

                    result = await _context.SaveChangesAsync();
                }
                return result;
            }

            return result;

        }

        public async Task UpdateItem(Inventory item)
        {
            if (_context != null)
            {
                try
                {
                    _context.Inventory.Update(item);
                    await _context.SaveChangesAsync();

                }
                catch (Exception)
                {
                    throw;
                }

            }

        }

        public async Task<bool> CheckItemNameAlready(string ItemName)
        {
            bool result = false;

            if (_context != null)
            {
                var item = await _context.Inventory.FirstOrDefaultAsync(x => x.Name == ItemName);

                if (item != null)
                {
                    result = true;
                }
            }

            return result;
        }

        public async Task<bool> ItemByID(int? itemId)
        {
            bool result = false;

            if (_context != null)
            {
                var item = await _context.Inventory.FirstOrDefaultAsync(x => x.ItemID == itemId);

                if (item != null)
                {
                    result = true;
                }
            }

            return result;
        }

        public async Task<Inventory> Item(int? itemId)
        {
            if (_context != null)
            {
                return await _context.Inventory.FindAsync(itemId);
            }

            return null;
        }

        public async Task<List<Inventory>> GetAllItems()
        {
            if (_context != null)
            {
                return await _context.Inventory.ToListAsync();
            }

            return null;
        }

    }
}
