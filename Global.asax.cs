using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SteelGames.Models;
using OfficeOpenXml;

namespace SteelGames
{
    public class MvcApplication : System.Web.HttpApplication
    {
        DBConnector connector = DBConnector.getInstance();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            SteelGames.Models.GameList.getInstance().AddRange(connector.getGamesByQuery("SELECT Game.*, SystemRequirements.* " +
                                                         "FROM Game " +
                                                         "JOIN SystemRequirements ON " +
                                                         "Game.SystemRequirementsID = SystemRequirements.SystemRequirementsID;"));
        }
    }
}
