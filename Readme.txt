dotnet add package Microsoft.EntityFrameworkCore --version 8.0.26
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.11
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.26
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.26
dotnet add package HslCommunication --version 12.8.1



dotnet ef migrations add Init
dotnet ef migrations add InitialCreate3 --context backend.Data.NpgContext 

dotnet ef database update --context backend.Data.NpgContext 