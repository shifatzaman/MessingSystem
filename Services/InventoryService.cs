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

        public void UpdateInventoryItemType(AddInventoryItemTypeViewModel model)
        {
            var item = new InventoryItemType
            {
                ItemTypeId = model.ItemTypeId,
                Name = model.Name,
                Unit = model.Unit,
                UnitPrice = model.UnitPrice,
                Quantity = model.Quantity
            };

            dbContext.InventoryItemTypes.Update(item);
            dbContext.SaveChanges();
        }

        public void AddOrUpdateInventoryItem(AddInventoryViewModel model)
        {
            if (model.InventoryId > 0)
                UpdateInventoryItem(model);

            else
            {
                var item = new InventoryItem
                {
                    Date = model.Date,
                    ItemType = model.ItemType,
                    Quantity = model.Quantity,
                    UnitPrice = model.UnitPrice
                };

                dbContext.InventoryItems.Add(item);

                dbContext.SaveChanges();


                if (item.ItemType > 0)
                {
                    var inventoryType = dbContext.InventoryItemTypes.Where(it => it.ItemTypeId == item.ItemType).FirstOrDefault();

                    if (inventoryType != null)
                    {

                        //Avg Calculation
                        var totalExistingInventoryPrice = inventoryType.Quantity * inventoryType.UnitPrice;

                        var newItemTotalPrice = model.Quantity * model.UnitPrice;

                        if (inventoryType.Quantity > 0)
                        {
                            try
                            {
                                inventoryType.UnitPrice = ((totalExistingInventoryPrice + newItemTotalPrice) / (inventoryType.Quantity + model.Quantity));
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                        {
                            inventoryType.UnitPrice = model.UnitPrice;
                        }

                        inventoryType.Quantity += item.Quantity;
                        dbContext.InventoryItemTypes.Update(inventoryType);
                        dbContext.SaveChanges();
                    }
                }
            }
            
        }

        public void UpdateInventoryItem(AddInventoryViewModel model)
        {

            var existingItem = dbContext.InventoryItems.Where(it => it.InventoryItemId == model.InventoryId).FirstOrDefault();

            if (existingItem != null)
            {
               
                existingItem.ItemType = model.ItemType;
                existingItem.Quantity = model.Quantity;
                existingItem.Date = model.Date;
                existingItem.UnitPrice = model.UnitPrice;

                dbContext.InventoryItems.Update(existingItem);
                dbContext.SaveChanges();
            }
        }

        public void DeleteInventoryItem(int inventoryId)
        {
            var item = dbContext.InventoryItems.Where(it => it.InventoryItemId == inventoryId).FirstOrDefault();
            item.IsDeleted = true;
            dbContext.InventoryItems.Update(item);
            dbContext.SaveChanges();


            if (item.ItemType > 0)
            {
                var inventoryType = dbContext.InventoryItemTypes.Where(it => it.ItemTypeId == item.ItemType).FirstOrDefault();

                if (inventoryType != null)
                {
                    inventoryType.Quantity -= item.Quantity;
                    dbContext.InventoryItemTypes.Update(inventoryType);
                    dbContext.SaveChanges();
                }
            }
        }

        public IList<FetchInventoryItemViewModel> GetInventoryItems()
        {
            var itemVms = (from item in dbContext.InventoryItems.OrderByDescending(it => it.InventoryItemId)
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
                               UnitPrice = item.UnitPrice
                           });

            return itemVms.ToList();
        }

        public IList<InventoryItemType> GetInventoryItemTypes()
        {
            return dbContext.InventoryItemTypes.ToList();
        }

        public InventoryItem GetInventoryItem(int inventoryItemId)
        {
            return dbContext.InventoryItems.Where(it => it.InventoryItemId == inventoryItemId).FirstOrDefault();
        }

        public void DeleteInventoryItemType(int inventoryItemLTypeId)
        {
            var inventoryItemType = GetInventoryItemType(inventoryItemLTypeId);
            if (inventoryItemType != null)
            {
                dbContext.InventoryItemTypes.Remove(inventoryItemType);
                dbContext.SaveChanges();
            }
        }

        public InventoryItemType GetInventoryItemType(int inventoryItemLTypeId)
        {
            return dbContext.InventoryItemTypes.Where(it => it.ItemTypeId == inventoryItemLTypeId).FirstOrDefault();
        }
    }
}
