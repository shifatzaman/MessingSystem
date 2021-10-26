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


## ChangeLog v1.0
1. Added edit and delete option in inventory
2. Added option to set unit price while adding bazar entry (unit price of inventory will be updated based on average value)
3. Added delete option for bazar. Deleting bazar item will remove that item's quantity from inventory.
4. Added option to create user account for mess member (email and password field added), member can login to portal using credentials
5. Added option to edit and delete mess members
6. Added option to Add/edit/delete extra messing bill for member (available for manager under billing and members list)
7. Added option to Add/edit/delete cafeterial bill for member (available for manager under billing and members list)
8. Added option to Add/edit/delete utility bill (available for manager under billing and members list)
9. Added option to generate bill in a date range and print as pdf (available for both member and manager)
10. Added option to add/edit/delete room, view vacant rooms (available for manager)
11. Added dashboard (placeholder images and texts added, needs to be replaced as per requirement)



## ChangeLog v2.0

Before running, take a pull from master, do a clean build and run migration

1. Accomodation - Changed rooms to accomodation in sidebar
2. Accomodation - Added statistics of rooms
3. Accomodation - Added date of entry field in rooms
4. Accomodation - updated design
5. Meal in/outs - Imposed timestamp restriction for memebers  (i) Breakfast: Previous day 8 PM (ii) Lunch: Present day 8 AM (iii) Tea break: Present day 8 AM (iv) Dinner: Present day 2 PM
6. Meal in/outs - Added notification for admin when a member changes his meal status (notification visible in dashboard)
7. Meal in/outs - Updated design
8. Daily messings - Added confirmation message while adding daily messing, reset after successfully adding daily messing
9. Daily messings - Added option to save a daily messing input as template for later use. Once a daily messing input is saved as template, user will be able to select it from dropdown while adding a new daily messing and it will autopopulate saved data
10. Daily messings - Added validation so that daily messing quantity for a particular item can't be more than the quantity available in store
11. Daily messings - Updated design
12. Store and Bazar - Added option to save unit price while adding bazar
13. Sote and Bazar - adding bazar will update store's per unit price based on average value
14. Sote and Bazar - newest bazar item will be displayed on top
15. Mess Members - changed members to mess members
16. Mess Members - Updated design, only name and action buttons will be displayed initially. Click on '+' to expand and display details information
17. Mess Members - Removed room column and data entry
18. Mess Members - Added marital status as a drop down list
19. Mess Members - Added ability to upload profile picture
20. Mess Members - Added ability to define member's role. Member can be defined as 'Admin'. Admin member will get all features of system admin. Admin members will be displayed in dashboard as committee members
21. Mess Members - Updated design, removed unnecessary buttons
22. Accounts - Change billing to accounts
23. Accounts - Moved monthly billing, extra messing, cafeteria bill to separate pages with minimum member information
24. Extra messing - Extra messing will be connected to inventory/store
25. Dashboard - Updated design
26. Dashboard - added dynamic notices and committee member sections. 
27. Dashboard - Notices can be added by admins from settings -> notice (check Visible in dashboard to display the notice in dashboard). 
28. Dashboard - Committee members will display mess members with admin user role. (see mess member section)
29. Dashboard - fixed design issues
30. Dashboard - Added 3 banners which will keep changing in interval. Placeholder images are set. Replace the images in wwwroot/img/banners to display custom/preferred images. Please note that, name of the image files shouldn't be change (banner-1.jpg, banner-2.jpg ... ) and image should be 1024 * 300. 


