using Api.Contracts.V1;
using Core.Auth.Requests;
using Core.Auth.Responses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers.V1 {
  [ApiController]
  [Route(ApiRoutes.Generic)]
  public class IdentityController : Controller {
    private readonly IIdentityService _service;
    public IdentityController(IIdentityService service) { _service = service; }
    [HttpPost]
    public async Task<IActionResult>
    Register([ FromBody ] UserRegistrationRequest request) {
      var authResult =
          await _service.RegisterAsync(request.Email, request.Password);

      if (authResult.IsSuccessful == false) {
        var failureResult = new AuthFailureResponse{Errors = authResult.Errors};
        return BadRequest(failureResult);
      }
      var successResult = new AuthSuccessResponse{Token = authResult.Token};
      return Ok(successResult);
    }
  }
}
