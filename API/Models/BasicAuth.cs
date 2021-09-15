using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace API.Models
{
    // Trieda pre základnú autorizáciu prístupov
    public class BasicAuth : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            // ak sa v hlavičke požiadavky nenájde token autorizácie, ihneď vráti odpoveď o neautorizovanom prístupe
            if(String.IsNullOrEmpty(actionContext.Request.Headers.Authorization.Parameter))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                // načítanie tokenu autorizácia (ak nie je zadaný, je null)
                string authToken = actionContext.Request.Headers.Authorization.Parameter;
                // v tokene je uložené heslo zakódované algoritmom Base64, po prijatí ho treba dekódovať
                string password = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

                // zahashovanie hesla pomocou SHA256, aby sa mohlo overiť podľa údajú v konfiguračnom súbore
                var sha256 = new SHA256Managed();
                byte[] sha256bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes($"13{password}12"));
                password = "";
                foreach(byte b in sha256bytes)
                {
                    password += b.ToString("x2");
                }

                if(Security.Login(password))
                {
                    // ak je prihlásenie úspešné, vytvorí sa identifkátor prihláseného používateľa
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("pouzivatel"), null);
                }
                else
                {
                    // aj je prihlásenie neúspešné, vráti sa HTTP Status 405
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}