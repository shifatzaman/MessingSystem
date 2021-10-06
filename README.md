# MessingSystem
 
A web app to manage inventory and messing

## Used Technologies
1. ASP.NET 5
2. Entity Framework Core 5
3. KnockoutJs
4. MSSQL Server


## Requirements
1. Visual Studio 16.10.4 or later
2. .NET 5 SDK 5.0.1 or later
3. MSSQL Server and Management Studio


## Steps to run
1. Download and Install necessary tools
2. Update connection string in appsettings.json
3. Build the project
4. Run ef core migrations (in terminal -> dotnet ef database update)
5. Run the project
6. Login using admin@ms.com


## Note
1. dotnet ef is not included with .NET SDK anymore. Run dotnet tool install --global dotnet-ef in CLI to install the tool globally before running ef related commands.


ChangeLog 06/10
1. Added edit and delete option in inventory
2. Added option to set unit price while adding bazar entry (unit price of inventory will be updated based on average value)
3. Added delete option for bazar. Deleting bazar item will remove that item's quantity from inventory.
