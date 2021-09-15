using System.Configuration;

namespace API.Models
{
    public class Security
    {
        // metóda na zistenie, či sa zadané heslo zhoduje s heslom z konfiguračného súboru
        public static bool Login(string password)
        {
            return (password == ConfigurationManager.AppSettings["pass"]);
        }
    }
}