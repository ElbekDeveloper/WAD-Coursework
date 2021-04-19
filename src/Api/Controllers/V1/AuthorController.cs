using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Api.Contracts.V1;
using Core.Auth.Extensions;
using Core.Auth.Roles;
using Core.Interfaces;
using Core.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers.V1 {
    [ApiController]
    [Route (ApiRoutes.Generic)]
    public class AuthorController : Controller {
        private IAuthorService _service;

        public AuthorController (IAuthorService service) {
            _service = service;
        }

        [HttpGet]
        [Route ("GetArticles")]
        [SwaggerResponse ((int) HttpStatusCode.OK, Description = "All Articles of User", Type = typeof (IEnumerable<ArticleResource>))]
        [SwaggerResponse ((int) HttpStatusCode.InternalServerError)]
        [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<ArticleResource>>> GetArticlesByUser (CancellationToken cancellationToken) {
            var userId = UserExtension.GetUserId (HttpContext);

            return Ok (await _service.GetArticlesByUserAsync (userId, cancellationToken));
        }

        [HttpGet]
        [Route ("GetAll")]
        [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CanManageUsers")]
        [SwaggerResponse ((int) HttpStatusCode.OK, Description = "All Authors", Type = typeof (IEnumerable<AuthorResource>))]
        [SwaggerResponse ((int) HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<AuthorResource>>> GetAuthors (CancellationToken cancellationToken) {
            return Ok (await _service.GetAuthors (cancellationToken));
        }

        [HttpGet]
        [Route ("CountAll")]
        [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CanManageUsers")]
        [SwaggerResponse ((int) HttpStatusCode.OK, Description = "Number of Authors", Type = typeof (int))]
        [SwaggerResponse ((int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAuthorsCount (CancellationToken cancellationToken) {
            return Ok (await _service.CountAuthors (cancellationToken));
        }
    }
}