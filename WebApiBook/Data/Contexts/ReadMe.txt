---- Update EF Core ----

1. Tool > Nuget Package Manager > Package Manager Console
2. PM > Scaffold-DbContext -Connection name=DefaultConnection -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data/Contexts -Context BookContext -force

