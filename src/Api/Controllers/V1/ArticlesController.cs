﻿using Api.Contracts.V1;
using Core.Auth.Extensions;
using Core.Auth.Roles;
using Core.Interfaces;
using Core.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "All Articles", Type = typeof(IEnumerable<ArticleResource>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<ArticleResource>>> GetArticles(CancellationToken cancellationToken)
        {
            return Ok(await _service.GetArticles());
        }


        [HttpGet("{id:int}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Get Article", Type = typeof(ArticleResource))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]

        public async Task<ActionResult<ArticleResource>> GetArticle([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _service.GetArticle(id, cancellationToken);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Add Article", Type = typeof(ArticleResource))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CanWriteArticle, CanManageUsers")]
        public async Task<ActionResult<ArticleResource>> AddArticle([FromBody]AddArticleResource model, CancellationToken cancellationToken)
        {
            var userId = UserExtension.GetUserId(HttpContext);
            
            return Ok(await _service.AddArticle(userId, model, cancellationToken));
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Article Updated ", Type = typeof(ArticleResource))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CanWriteArticle, CanManageUsers")]

        public async Task<ActionResult<ArticleResource>> UpdateArticle([FromRoute][Required] int id,[FromBody][Required] AddArticleResource model, CancellationToken cancellationToken)
        {
            var userId = UserExtension.GetUserId(HttpContext);

            return Ok(await _service.UpdateArticle(userId,id, model, cancellationToken));
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Article Deleted", Type = typeof(ArticleResource))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CanWriteArticle, CanManageUsers")]
        public async Task<ActionResult<ArticleResource>> DeleteArticle([FromRoute] int id, CancellationToken cancellationToken)
        {
            return Ok(await _service.DeleteArticle(id, cancellationToken));
        }

    }
}
