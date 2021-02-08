using System.Web;
using System.Web.Mvc;

namespace TestePratico_Trainee_ObaBox
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
