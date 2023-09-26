using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Senao_EE.Controllers
{
    public class ProduceChangeLogController : Controller
    {
        //
        // GET: /ProduceChangeLog/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            Senao_EEEntities db = new Senao_EEEntities();

            var workOvertime = from view in db.v_ProduceChangeLog
                               select view;

            return View(workOvertime.ToList());
        }
	}
}