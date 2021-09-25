using MessingSystem.Domain;
using MessingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Services
{
    public interface IInventoryService
    {
        void AddOrUpdateInventoryItem(AddInventoryViewModel model);
        IList<FetchInventoryItemViewModel> GetInventoryItems();

        void DeleteInventoryItem(int inventoryId);

        IList<InventoryItemType> GetInventoryItemTypes();

        void AddInventoryItemType(AddInventoryItemTypeViewModel model);


    }
}
