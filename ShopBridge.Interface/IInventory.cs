using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopBridge.Model;

namespace ShopBridge.Interface
{

    public interface IInventory
    {
        Task<List<Inventory>> GetAllItems();
        Task<Inventory> Item(int? itemID);
        Task<bool> ItemByID(int? itemID);
        Task<bool> CheckItemNameAlready(string itemName);
        Task<int> SaveItem(Inventory item);
        Task UpdateItem(Inventory item);
        Task<int> DeleteItem(int? itemID);





    }
}
