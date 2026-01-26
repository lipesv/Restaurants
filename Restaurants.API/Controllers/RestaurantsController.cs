using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.CreateRestaurant;
using Restaurants.Application.Restaurants.DeleteRestaurant;
using Restaurants.Application.Restaurants.GetRestaurantById;
using Restaurants.Application.Restaurants.GetRestaurants;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await mediator.Send(new GetRestaurantsQuery());
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));

        if (restaurant is null)
        {
            return NotFound();
        }

        return Ok(restaurant);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRestaurantCommand command)
    {
        int id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var isDeleted = await mediator.Send(new DeleteRestaurantCommand(id));

        if (isDeleted)
        {
            return NoContent();
        }

        return NotFound();
    }
}
