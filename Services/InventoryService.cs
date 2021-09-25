using MessingSystem.Data;
using MessingSystem.Domain;
using MessingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly AppDbContext dbContext;
        public InventoryService(AppDbContext appDbContext)
        {
            dbContext = appDbContext;
        }

        public void AddInventoryItemType(AddInventoryItemTypeViewModel model)
        {
            var item = new InventoryItemType
            {
                Name = model.Name,
                Unit = model.Unit,
                UnitPrice = model.UnitPrice,
                Quantity = model.Quantity
            };

            dbContext.InventoryItemTypes.Add(item);
            dbContext.SaveChanges();
        }

        public void AddOrUpdateInventoryItem(AddInventoryViewModel model)
        {
            var item = new InventoryItem
            {
                Date = model.Date,
                ItemType = model.ItemType,
                Quantity = model.Quantity
            };

            dbContext.InventoryItems.Add(item);
            dbContext.SaveChanges();


            if (item.ItemType > 0)
            {
                var inventoryType = dbContext.InventoryItemTypes.Where(it => it.ItemTypeId == item.ItemType).FirstOrDefault();

                if (inventoryType != null)
                {
                    inventoryType.Quantity += item.Quantity;
                    dbContext.InventoryItemTypes.Update(inventoryType);
                    dbContext.SaveChanges();
                }
            }
        }

        public void DeleteInventoryItem(int inventoryId)
        {
            var item = dbContext.InventoryItems.Where(it => it.InventoryItemId == inventoryId).FirstOrDefault();
            item.IsDeleted = true;
            dbContext.InventoryItems.Update(item);
            dbContext.SaveChanges();
        }

        public IList<FetchInventoryItemViewModel> GetInventoryItems()
        {
            var itemVms = (from item in dbContext.InventoryItems
                           join type in dbContext.InventoryItemTypes
                           on item.ItemType equals type.ItemTypeId
                           select new FetchInventoryItemViewModel
                           {
                               InventoryId = item.InventoryItemId,
                               Date = item.Date,
                               ItemType = item.ItemType,
                               Quantity = item.Quantity,
                               ItemName =  type.Name,
                               Unit = type.Unit,
                               UnitPrice = type.UnitPrice
                           });

            return itemVms.ToList();
        }

        public IList<InventoryItemType> GetInventoryItemTypes()
        {
            return dbContext.InventoryItemTypes.ToList();
        }
    }
}
