using DonationHub.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoriesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }

    [HttpGet("{Id}")]
    public IActionResult GetById(Guid Id)
    {
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{Id}")]
    public IActionResult Post(Guid Id, Category category)
    {
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{Id}")]
    public IActionResult Put(Guid Id, Category category)
    {
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{Id}")]
    public IActionResult Delete(Guid Id)
    {
        return Ok();
    }

}
