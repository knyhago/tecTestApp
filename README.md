🚀 Key Development Improvements

1.  Clean, Maintainable & Reusable Code
    Applied SOLID principles throughout the application for a scalable and easy-to-maintain architecture.
    Layered structure with clear separation of concerns: Controllers, Services, DTOs, and Data Access.
  
2.  Introduced DTOs
	  Replaced direct use of Entity Models with Data Transfer Objects (DTOs) for clean separation of concerns and API best practices.

3.	Database Upgrade
	  Migrated from in-memory storage to Azure SQL Database for persistent, production-ready data handling.

4.	CI/CD Pipeline
	  Implemented a CI/CD pipeline with:
	  Automated builds.
	  Test execution.
	  Continuous deployment to Azure.

5.	Dual App Deployment
	  Pipeline deploys both applications:
	  ASP.NET Core MVC (server-side, baseline).
	  Blazor WebAssembly (client-side, flagship).

6.	Scalable & Maintainable Architecture
	Structured for future growth with separation of concerns, async EF Core operations, and cloud-ready deployment.
 
🌐 Live Applications
•	ASP.NET Core MVC (Baseline):
https://usermanagement-gacpf2b0hhfqhbfe.ukwest-01.azurewebsites.net/

•	Blazor WebAssembly (Flagship, Full-Featured):
https://usermanagement-blazor-akcwauduftbbgnc6.ukwest-01.azurewebsites.net/

👉 Product Decision:
•	The MVC app demonstrates the baseline CRUD implementation.

•	The Blazor app showcases advanced features, enhanced logging, pagination, and a richer UI experience.

 
 Features
 
•	User management (Add, Edit, View, Delete)

•	Filters for Active / Inactive users

•	Detailed activity logs with drill-down views

•	Pagination for large datasets

•	Responsive, modern UI design

•	Backend powered by Entity Framework Core

•	Hosted in the cloud with Azure App Service
 


