# TrackingApp
Guidelines on How to Build and Run the Application:

1.	Open the solution project named “TrackingApp” using VS 2019 and Run as an Administrator.
2.	Build the Project Solution and wait until it restores all the Package Dependencies into your machine.
3.	After the build is succeeded, There are two projects running in a website or IIS. Right click the “Tracking.Api” Project and Set as StartUp Project.
  

IMPORTANT: check the appsettings.json config for data connections for DB and other app keys.
on my case only change the Server Connection in appsettings.json file.
 

4.	Data Migration / Code First
I used code first on my data migrations to database, see the following images below for the workaround.
Expand the “Tracking.DataAccess” Project and as you can see I already have my Migrations folder there and “addedUserAndEmployeeTables” first migration,
 
If you can see that, try run this command directly in your PM Console “PM> Update-Database -Context TrackingAppDbContext”, or if it gives you an error, remove the class named “TrackingAppDbContextModelSnapshot” below it then Run this command…
 

Then do “PM> Update-Database -Context TrackingAppDbContext” after adding migration and it will sync to your connection database.

5.	Build the Solution again, and remember we set the startup project to “Tracking.Api”. Hit “CTRL + F5” to run the project without the debugging, and it will redirect you to swagger gen UI for the endpoints, or you can use Fiddler or Postman for api testing, but leave it for now.
 

6.	After seeing that swagger UI, you can now run the Platform of “Tracking.App” project which is our portal…
 
	Right click on that and Set as StartUp Project, then hit “CTRL + F5”.
	Notice also that I have my appsettings.json that connects the server of the database.

7.	After you build and Run it without any issue, you will be redirected to the actual platform, see images below…
Home Page:
 


Login Page: the default Username = “test” and password is = “p@s5w0rD”
 

Employees Manager Page: with Add, Edit and Delete 

Unauthorized Page: when accessing with JWT token auth
 

That’s it!

Thank You
