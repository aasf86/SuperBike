

# Dentro do diretório: SuperBike\src\SuperBike.Auth
## Commands para criar a migração e criar as tabelas do asp.net.identity

```cmd

	dotnet ef database add Initial --context AuthIdentityDbContext -s ..\SuperBike.Api -v

	dotnet ef database update --context AuthIdentityDbContext -s ..\SuperBike.Api -v

```