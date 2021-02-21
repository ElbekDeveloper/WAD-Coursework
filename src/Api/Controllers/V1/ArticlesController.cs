using Api.Contracts.V1;
using Core.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Controllers.V1
{
    [ApiController]
    [Route(ApiRoutes.Generic)]
    public class ArticlesController : Controller
    {
        private IArticleService _service;

        public ArticlesController(IArticleService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "All Articles", Type = typeof(List<Article>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles(CancellationToken cancellationToken)
        {
            return Ok(await _service.GetArticles());
        }

        }
}
