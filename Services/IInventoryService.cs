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

        InventoryItem GetInventoryItem(int inventoryItemId);

        IList<InventoryItemType> GetInventoryItemTypes();

        void AddInventoryItemType(AddInventoryItemTypeViewModel model);

        public void UpdateInventoryItemType(AddInventoryItemTypeViewModel model);

        void DeleteInventoryItemType(int inventoryItemLTypeId);

        InventoryItemType GetInventoryItemType(int inventoryItemLTypeId);

    }
}
