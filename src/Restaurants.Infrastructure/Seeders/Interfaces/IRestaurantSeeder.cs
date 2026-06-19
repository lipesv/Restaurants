namespace Restaurants.Infrastructure.Seeders.Interfaces;

/// <summary>
/// Interface para o seeding do banco de dados, responsável por popular o banco com dados iniciais, como usuários, roles e restaurantes.
/// </summary>
public interface IDatabaseSeeder
{
    Task SeedAsync();
}