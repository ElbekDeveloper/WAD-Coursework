using Api.Contracts.V1;
using Core.Interfaces;
using Core.Resources;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
namespace Api.Controllers.V1
{
    [ApiController]
    [Route(ApiRoutes.Generic)]
    public class AuthorController:Controller
    {
        private IAuthorService _service;

        public AuthorController(IAuthorService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "All Authors", Type = typeof(IEnumerable<AuthorResource>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<AuthorResource>>> GetAuthors(CancellationToken cancellationToken)
        {
            return Ok(await _service.GetAuthors());
        }



        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Add Author", Type = typeof(AuthorResource))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<AuthorResource>> AddAuthor([FromBody] AddAuthorResource model, CancellationToken cancellationToken)
        {
            return Ok(await _service.AddAuthor(model, cancellationToken));
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Author Deleted", Type = typeof(AuthorResource))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<AuthorResource>> DeleteAuthor([FromRoute] int id, CancellationToken cancellationToken)
        {
            return Ok(await _service.DeleteAuthor(id, cancellationToken));
        }

        [HttpGet("{id:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Get Author", Type = typeof(AuthorResource))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<AuthorResource>> GetAuthor([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _service.GetAuthor(id, cancellationToken);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpPut]
        [Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Author Updated ", Type = typeof(AuthorResource))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<AuthorResource>> UpdateAuthor([FromRoute][Required] int id, [FromBody][Required] AddAuthorResource model, CancellationToken cancellationToken)
        {
            return Ok(await _service.UpdateAuthor(id, model, cancellationToken));
        }

    }
}
