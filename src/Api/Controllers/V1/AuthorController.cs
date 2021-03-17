using Api.Contracts.V1;
using Core.Interfaces;
using Core.Resources;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers.V1 {
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [ApiController]
  [Route(ApiRoutes.Generic)]
  public class AuthorController : Controller {
    private IAuthorService _service;

    public AuthorController(IAuthorService service) { _service = service; }

    [HttpGet]
    [Route("GetAll")]
    [SwaggerResponse((int) HttpStatusCode.OK, Description = "All Authors",
                     Type = typeof(IEnumerable<AuthorResource>))]
    [SwaggerResponse((int) HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<IEnumerable<AuthorResource>>>
    GetAuthors(CancellationToken cancellationToken) {
      return Ok(await _service.GetAuthors(cancellationToken));
    }
    [HttpGet]
    [Route("CountAll")]
    [SwaggerResponse((int) HttpStatusCode.OK, Description = "Number of Authors",
                     Type = typeof(int))]
    [SwaggerResponse((int) HttpStatusCode.InternalServerError)]
    public async Task<IActionResult>
    GetAuthorsCount(CancellationToken cancellationToken) {
      return Ok(await _service.CountAuthors(cancellationToken));
    }
  }
}
