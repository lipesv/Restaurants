using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dtos.Restaurants;
using Restaurants.Application.Restaurants.CreateRestaurant;
using Restaurants.Application.Restaurants.DeleteRestaurant;
using Restaurants.Application.Restaurants.GetRestaurantById;
using Restaurants.Application.Restaurants.GetRestaurants;
using Restaurants.Application.Restaurants.UpdateRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Auth.Policies;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
    {
        var restaurants = await mediator.Send(new GetRestaurantsQuery());
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = PolicyNames.HasNationality)]
    public async Task<ActionResult<RestaurantDto?>> GetById([FromRoute] int id)
    {
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
        return Ok(restaurant);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Owner)]
    public async Task<IActionResult> Create(CreateRestaurantCommand command)
    {
        int id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, UpdateRestaurantCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await mediator.Send(new DeleteRestaurantCommand(id));
        return NoContent();
    }
}
