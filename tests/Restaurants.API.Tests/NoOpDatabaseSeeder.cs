using Restaurants.Infrastructure.Seeders.Interfaces;

namespace Restaurants.API.Tests;

public class NoOpDatabaseSeeder : IDatabaseSeeder
{
    /// <summary>
    /// Implementação de IDatabaseSeeder que não realiza nenhuma operação de seeding.
    /// Útil para testes onde o banco de dados já está pré-configurado ou quando o seeding não é necessário.
    /// </summary>
    /// <returns></returns>
    public Task SeedAsync() => Task.CompletedTask;
}