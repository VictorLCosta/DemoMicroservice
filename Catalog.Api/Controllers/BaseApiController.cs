namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController(ICatalogContext context) : ControllerBase
{
    protected readonly ICatalogContext _context = context;
}