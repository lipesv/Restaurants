using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
{
    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var restaurants = await dbContext.Restaurants.ToListAsync();
        return restaurants;
    }

    /// <summary>
    /// Retrieves all restaurants that match the given search phrase in their name or description.
    /// </summary>
    /// <param name="searchPhrase">The phrase to search for in restaurant names or descriptions. If null or empty, all restaurants are returned.</param>
    /// <returns>A collection of restaurants matching the search criteria.</returns>
    public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase,
                                                                          int pageNumber,
                                                                          int pageSize,
                                                                          string? sortBy,
                                                                          SortDirection sortDirection)
    {
        var query = dbContext.Restaurants.AsQueryable();

        if (!string.IsNullOrEmpty(searchPhrase))
        {
            query = query.Where(r => EF.Functions.Like(r.Name, $"%{searchPhrase}%")
                                     || EF.Functions.Like(r.Description, $"%{searchPhrase}%"));
        }

        var totalCount = await query.CountAsync();

        if (!string.IsNullOrEmpty(sortBy))
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                { nameof(Restaurant.Name), r => r.Name },
                { nameof(Restaurant.Description), r => r.Description },
                { nameof(Restaurant.Category), r => r.Category }
            };

            if (columnsSelector.TryGetValue(sortBy, out var expression))
            {
                query = sortDirection
                        == SortDirection.Ascending ? query.OrderBy(expression) : query.OrderByDescending(expression);
            }
        }

        var restaurants = await query.Skip(pageSize * (pageNumber - 1))
                                     .Take(pageSize)
                                     .ToListAsync();

        return (restaurants, totalCount);
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        var restaurant = await dbContext.Restaurants.Include(r => r.Dishes)
                                                    .FirstOrDefaultAsync(r => r.Id == id);

        return restaurant;
    }

    public async Task<IEnumerable<Restaurant>> GetByOwnerIdAsync(string ownerId)
    {
        var restaurants = await dbContext.Restaurants.Where(r => r.OwnerId == ownerId)
                                                                        .ToListAsync();

        return restaurants;
    }

    public async Task<int> CreateAsync(Restaurant restaurant)
    {
        dbContext.Restaurants.Add(restaurant);

        await dbContext.SaveChangesAsync();

        return restaurant.Id;
    }

    public async Task DeleteAsync(Restaurant restaurant)
    {
        dbContext.Remove(restaurant);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await dbContext.SaveChangesAsync();
    }
}