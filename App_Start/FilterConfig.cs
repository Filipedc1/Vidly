using System.Web;
using System.Web.Mvc;

//you can register global filters here.
//e.g. HandleErrorAttribute is a default global filter. This filter redirects the user to an error page when an action throws an exception
namespace Vidly
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
        }
    }
}
