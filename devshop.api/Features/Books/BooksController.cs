using devshop.api.Cores;
using devshop.api.Features.Books.Requests;
using Microsoft.AspNetCore.Mvc;

namespace devshop.api.Features.Books;

public class BooksController(IServiceScopeFactory serviceScopeFactory) 
    : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            
            var service = serviceScope.ServiceProvider
                .GetRequiredService<BooksRequestHandler>();
            
            return Ok(await service.Books());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem();
        }
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            
            var service = serviceScope.ServiceProvider
                .GetRequiredService<BooksRequestHandler>();
            
            return Ok(await service.Books(id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BooksCreateRequest request)
    {
        try
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            
            var handler = serviceScope.ServiceProvider
                .GetRequiredService<BooksRequestHandler>();

            await handler.InsertAsync(request);
            
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem();
        }
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Put(Guid id, BooksUpdateRequest request)
    {
        try
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            
            var handler = serviceScope.ServiceProvider
                .GetRequiredService<BooksRequestHandler>();

            await handler.UpdateAsync(id, request);
            
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem();
        }
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            
            var handler = serviceScope.ServiceProvider
                .GetRequiredService<BooksRequestHandler>();

            await handler.DeleteAsync(id);
            
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
}