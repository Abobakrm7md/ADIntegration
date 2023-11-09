using System.Collections.Generic;

namespace WFCustomization.Shared.Swagger
{
    public class OAuth2
    {
        public string AuthorizationUrl { get; set; }
        public string TokenUrl { get; set; }
        public string RefreshTokenUrl { get; set; }
        public IEnumerable<Scope> Scopes { get; set; }
    }
}
