using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;
using UmarSeat.providers;

[assembly: OwinStartupAttribute(typeof(UmarSeat.Startup))]
namespace UmarSeat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           
            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            app.MapSignalR();

        }
        public void ConfigureOAuth(IAppBuilder app)
        {
           

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {

                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(30),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };
            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            var oauthBearOptions = new OAuthBearerAuthenticationOptions();
            oauthBearOptions.AccessTokenFormat = OAuthServerOptions.AccessTokenFormat;
            oauthBearOptions.AccessTokenProvider = OAuthServerOptions.AccessTokenProvider;
            oauthBearOptions.AuthenticationMode = OAuthServerOptions.AuthenticationMode;
            oauthBearOptions.AuthenticationType = OAuthServerOptions.AuthenticationType;
            oauthBearOptions.Description = OAuthServerOptions.Description;
            oauthBearOptions.Provider = new QueryStringOAuthBearerProvider("access_token");


            app.UseOAuthBearerAuthentication(oauthBearOptions);

        }
    }
}
