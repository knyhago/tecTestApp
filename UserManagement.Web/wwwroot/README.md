🚀 Key Development Improvements
1.	Introduced DTOs
o	Replaced direct use of Entity Models with Data Transfer Objects (DTOs) for clean separation of concerns and API best practices.
2.	Database Upgrade
o	Migrated from in-memory storage to Azure SQL Database for persistent, production-ready data handling.
3.	CI/CD Pipeline
o	Implemented a CI/CD pipeline with:
	Automated builds.
	Test execution.
	Continuous deployment to Azure.
4.	Dual App Deployment
o	Pipeline deploys both applications:
	ASP.NET Core MVC (server-side, baseline).
	Blazor WebAssembly (client-side, flagship).
5.	Scalable & Maintainable Architecture
o	Structured for future growth with separation of concerns, async EF Core operations, and cloud-ready deployment.
 
🌐 Live Applications
•	ASP.NET Core MVC (Baseline):
https://usermanagement-gacpf2b0hhfqhbfe.ukwest-01.azurewebsites.net/
•	Blazor WebAssembly (Flagship, Full-Featured):
https://usermanagement-blazor-akcwauduftbbgnc6.ukwest-01.azurewebsites.net/
👉 Product Decision:
•	The MVC app demonstrates the baseline CRUD implementation.
•	The Blazor app showcases advanced features, enhanced logging, pagination, and a richer UI experience.
 
✨ Features
•	User management (Add, Edit, View, Delete)
•	Filters for Active / Inactive users
•	Detailed activity logs with drill-down views
•	Pagination for large datasets
•	Responsive, modern UI design
•	Backend powered by Entity Framework Core
•	Hosted in the cloud with Azure App Service
 
🛠 Tech Stack
•	Backend: ASP.NET Core, Entity Framework Core
•	Frontend: Razor Pages (MVC), Blazor WebAssembly
•	Database: Azure SQL Database
•	Infrastructure: Azure App Service
•	Pipeline: GitHub Actions (CI/CD)

