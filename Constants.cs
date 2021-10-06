using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem
{
    public static class Constants
    {
        //Permissions
        public const string IventoryAdd = "AddInventory";
        public const string IventoryDelete = "DeleteInventory";
        public const string MealInOut = "MealInOut";


        public static readonly string[] AllowedPermissonsForMembers = { 
            MealInOut
        };
    }
}
