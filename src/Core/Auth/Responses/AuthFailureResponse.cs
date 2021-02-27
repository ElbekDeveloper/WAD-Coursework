using System.Collections.Generic;

namespace Core.Auth.Responses
{
    public class AuthFailureResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
