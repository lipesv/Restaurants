using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.CreateDish;
using Restaurants.Application.Dishes.DeleteDishesByRestaurant;
using Restaurants.Application.Dishes.GetDishById;
using Restaurants.Application.Dishes.GetDishesByRestaurantId;
using Restaurants.Application.Dtos.Dishes;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants/{restaurantId}/dishes")]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] int restaurantId, CreateDishCommand command)
    {
        command.RestaurantId = restaurantId;
        var dishId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetByIdForRestaurant), new { restaurantId, dishId }, null);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAllForRestaurant([FromRoute] int restaurantId)
    {
        var dishes = await mediator.Send(new GetDishesByRestaurantIdQuery(restaurantId));
        return Ok(dishes);
    }

    [HttpGet("{dishId}")]
    public async Task<ActionResult<DishDto>> GetByIdForRestaurant([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var dish = await mediator.Send(new GetDishByIdQuery(restaurantId, dishId));
        return Ok(dish);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveDishesForRestaurant([FromRoute] int restaurantId)
    {
        await mediator.Send(new DeleteDishesByRestaurantCommand(restaurantId));
        return NoContent();
    }
}
