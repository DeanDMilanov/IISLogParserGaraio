using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IISLogFileParserGaraio.Controllers
{
    public class StatisticsController : Controller
    {

        //private IIPExtractor ipExtractor;
        //private IFQDNExtractor fqdnExtractor;
        private IStatisticsGenerator statisticsGenerator;
        public StatisticsController(IStatisticsGenerator stGenerator)
        {
            statisticsGenerator = stGenerator;
        }

        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase logFile)
        {
            if (logFile != null && logFile.ContentLength > 0)
            {
                if (!logFile.FileName.EndsWith("log") && !logFile.FileName.EndsWith("txt"))
                {
                    return View("ErroneousFileFormat");
                }
                string filePath = SaveFile(logFile);
                TempData["filePath"] = filePath;
                return RedirectToAction("InformationSummary");
            }
            else
            {
                // file was not uploaded properly, display the upload form again
                return RedirectToAction("Index");
            }
        }

        public ActionResult InformationSummary()
        {
            string[] allLines = System.IO.File.ReadAllLines(TempData["filePath"].ToString());
            var statistics = statisticsGenerator.GenerateStatistics(allLines);
            return View(statistics);
        }

        private string SaveFile(HttpPostedFileBase logFile)
        {
            string filePath = GenerateFilePath(logFile.FileName);
            logFile.SaveAs(filePath);
            return filePath;
        }

        private string GenerateFilePath(string fileName)
        {
            return string.Format("{0}/{1}", Path.GetTempPath(), fileName);
        }
    }
}