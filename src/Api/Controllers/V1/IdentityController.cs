using Api.Contracts.V1;
using Core.Auth.Requests;
using Core.Auth.Responses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers.V1 {
  [ApiController]
  [Route(ApiRoutes.Generic)]
  public class IdentityController : Controller {
    private readonly IIdentityService _service;
    public IdentityController(IIdentityService service) { _service = service; }
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult>
    Register([ FromBody ] UserRegistrationRequest request) {
      if (ModelState.IsValid == false) {
        return BadRequest(new AuthFailureResponse{
            Errors = ModelState.Values.SelectMany(
                v => v.Errors.Select(e => e.ErrorMessage))});
      }
      var authResult =
          await _service.RegisterAsync(request.Email, request.Password);

      if (authResult.IsSuccessful == false) {
        var failureResult = new AuthFailureResponse{Errors = authResult.Errors};
        return BadRequest(failureResult);
      }
      var successResult = new AuthSuccessResponse{
          Token = authResult.Token, RefreshToken = authResult.RefreshToken

      };
      return Ok(successResult);
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult>
    Login([ FromBody ] UserLoginRequest request) {
      var authResult =
          await _service.LoginAsync(request.Email, request.Password);

      if (authResult.IsSuccessful == false) {
        var failureResult = new AuthFailureResponse{Errors = authResult.Errors};
        return BadRequest(failureResult);
      }
      var successResult = new AuthSuccessResponse{
          Token = authResult.Token, RefreshToken = authResult.RefreshToken};
      return Ok(successResult);
    }
    [HttpPost]
    [Route("Refresh")]
    public async Task<IActionResult>
    Refresh([ FromBody ] RefreshTokenRequest request) {
      var authResult =
          await _service.RefreshTokenAsync(request.Token, request.RefreshToken);

      if (authResult.IsSuccessful == false) {
        var failureResult = new AuthFailureResponse{Errors = authResult.Errors};
        return BadRequest(failureResult);
      }
      var successResult = new AuthSuccessResponse{
          Token = authResult.Token, RefreshToken = authResult.RefreshToken};
      return Ok(successResult);
    }
  }
}
