using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PIMES_DMS.Data;
using PIMES_DMS.Models;
using System.Globalization;

namespace PIMES_DMS.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _Db;

        public DashboardController(AppDbContext db)
        {
            _Db = db;
        }

        public IActionResult DashView()
        {
            int yearNow = DateTime.Now.Year;
            TempData["selectedYear"] = yearNow;

            GetMonth();
            GetDataforLineChart(yearNow, DateTime.Now.Month);
            GetResValues(DateTime.Now.Month, yearNow);
            GetValdataforBar(yearNow, DateTime.Now.Month);
            GetDataforValidandInv();

            return View();
        }

        public IActionResult RecentYear(int year)
        {
            TempData["selectedYear"] = year;
            int currentMonths = 0;

            if (DateTime.Now.Year != year)
            {
                TempData["Months"] = 12;
                currentMonths = 12;
            }
            else
            {
                currentMonths = DateTime.Now.Month;
                TempData["Months"] = currentMonths - 1;
            }

            //GetMonth();
            GetResValues(currentMonths, year);
            GetDataforLineChart(year, currentMonths);
            GetDataforValidandInv();
            GetValdataforBar(year, currentMonths);


            GetDataforLineChart(year, currentMonths);            

            return View("DashView");
        }

        public IActionResult GetVerSummary(int year)
        {
            List<Vermodel> obj = new();
            IEnumerable<ActionModel> actionIDs = _Db.ActionDb.Where(j => !j.IsDeleted);

            foreach (var act in actionIDs)
            {
                IEnumerable<Vermodel> objs = _Db.VerDb.Where(j => j.ActionID == act.ActionID);

                if (objs.Any())
                {
                    objs = objs.OrderByDescending(j => j.DateVer).ToList();
                    obj.Add(objs.First());
                }
            }

            return NoContent();
        }

        public void GetValdataforBar(int year, int month)
        {
            IEnumerable<IssueModel> issues = _Db.IssueDb.Where(j => j.DateFound.Year == year && !j.isDeleted);

            int[] valids = new int[month + 1];
            int[] invalids = new int[month + 1];

            for (int i = 1; i <= month; i++)
            {
                DateTime YYear = new DateTime(year, i, 1);
                valids[i] = issues.Count(j => j.DateFound.Year == YYear.Year && j.DateFound.Month == YYear.Month && j.ValRes == "Valid");
                invalids[i] = issues.Count(j => j.DateFound.Year == YYear.Year && j.DateFound.Month == YYear.Month && j.ValRes == "Invalid");
            }

            int[] newValids = new int[month];
            int[] newInvalids = new int[month];

            Array.Copy(valids, 1, newValids, 0, month);
            Array.Copy(invalids, 1, newInvalids, 0, month);

            ViewBag.valid = JsonConvert.SerializeObject(newValids);
            ViewBag.invalid = JsonConvert.SerializeObject(newInvalids);
        }

        public void GetOpenData(int year)
        {
            int res = 0;

            foreach (var issue in _Db.IssueDb.Where(j => !j.isDeleted && j.ValidatedStatus && j.HasTES && j.DateFound.Year == year && j.ValRes == "Valid"))
            {
                foreach (var action in _Db.ActionDb.Where(j => !j.IsDeleted && j.ControlNo == issue.ControlNumber))
                {
                    if (!action.HasVer)
                    {
                        res++;
                        break;
                    }

                    IEnumerable<Vermodel> vers = _Db.VerDb.Where(j => !j.IsDeleted && j.ActionID == action.ActionID);

                    vers = vers.OrderByDescending(j => j.DateVer).ToList();

                    if (vers.First().Status == "o")
                    {
                        res++;
                        break;
                    }
                }
            }
            res = res + _Db.IssueDb.Count(j => !j.isDeleted && j.ValidatedStatus && !j.HasTES && j.DateFound.Year == year && j.ValRes == "Valid");

            ViewBag.open = JsonConvert.SerializeObject(res);
        }

        public void GetClosedData(int year)
        {
            int res = 0;

            var issues = _Db.IssueDb.Where(j => !j.isDeleted && j.ValidatedStatus && j.HasTES && j.DateFound.Year == year && j.ValRes == "Valid");

            foreach (var issue in issues)
            {
                List<Vermodel> verCheck = new List<Vermodel>();

                var actions = _Db.ActionDb.Where(j => !j.IsDeleted && j.ControlNo == issue.ControlNumber);

                foreach (var action in actions)
                {
                    IEnumerable<Vermodel> vers = _Db.VerDb.Where(j => !j.IsDeleted && j.ActionID == action.ActionID);

                    if (vers.Any())
                    {
                        verCheck.AddRange(vers);
                    }
                    else
                    {
                        verCheck.Clear();
                        break;
                    }
                }

                if (verCheck.Any(j => j.Status == "c") && !verCheck.Any(j => j.Status == "o"))
                {
                    res++;
                }
            }

            ViewBag.closed = JsonConvert.SerializeObject(res);
        }

        public void GetMonth()
        {
            var currentDate = DateTime.Now;
            var currentMonth = currentDate.Month;
            TempData["Months"] = currentMonth - 1;

            var months = new string[currentMonth];
            var dateTimeFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;

            for (var i = 1; i <= currentMonth; i++)
            {
                var monthName = dateTimeFormatInfo.GetMonthName(i);
                months[i - 1] = monthName;
            }

            ViewBag.Month = months;
        }

        public void GetDataforLineChart(int year, int month)
        {
            ViewBag.LineValid = null;
            ViewBag.LineInvalid = null;

            int[] valid = new int[month + 1];
            int[] invalid = new int[month + 1];

            IEnumerable<IssueModel> issues = _Db.IssueDb.Where(j => j.DateFound.Year == year && !j.isDeleted);

            for (int i = 1; i <= month; i++)
            {
                valid[i] = issues.Count(j => j.DateFound.Month == i && j.ValRes == "Valid");

                invalid[i] = issues.Count(j => j.DateFound.Month == i && j.ValRes == "Invalid");
            }            

            for (int i = 1; i <= month; i++)
            {
                if (i <= month)
                {
                    valid[i] = valid[i] + valid[i - 1];
                    invalid[i] = invalid[i] + invalid[i - 1];
                }
            }

            int[] newValidLine = new int[month];
            int[] newInvalidLine = new int[month];

            Array.Copy(valid, 1, newValidLine, 0, month);
            Array.Copy(invalid, 1, newInvalidLine, 0, month);

            ViewBag.LineValid = JsonConvert.SerializeObject(newValidLine);
            ViewBag.LineInvalid = JsonConvert.SerializeObject(newInvalidLine);
        }

        private int CalculateTotalDays(DateTime from, DateTime to)
        {
            TimeSpan totalSpan = TimeSpan.Zero;
            int totalDays = 0;

            while (from < to)
            {
                if (from.DayOfWeek != DayOfWeek.Saturday && from.DayOfWeek != DayOfWeek.Sunday)
                {
                    totalSpan += TimeSpan.FromDays(1);
                }

                from = from.AddDays(1);
            }

            totalDays += totalSpan.Days;

            return totalDays;
        }

        public void GetResValues(int month, int year)
        {
            List<IssueModel> issues = _Db.IssueDb.Where(j => j.ValidatedStatus && j.DateFound.Year == year && !j.isDeleted).OrderBy(j => j.DateFound).ToList();
            List<ResData> resValues = new List<ResData>();
            
            foreach (IssueModel issue in issues)
            {
                int DateMonth = issue.DateFound.Month;

                ResData rd = new ResData();
                {
                    //rd.Days = CalculateTotalDays(issue.DateFound, issue.DateVdal);
                    List<int> arraySetter = new List<int>();
                    
                    for (int k = 1; k <= DateMonth; k++)
                    {
                        if (k != DateMonth)
                        {
                            arraySetter.Add(0);
                        }
                        else
                        {
                            arraySetter.Add(CalculateTotalDays(issue.DateFound, issue.DateVdal));
                        }
                    }

                    rd.Days = arraySetter.ToArray();
                    rd.Validation = issue.ValRes!;
                }


                resValues.Add(rd);
            }

            ViewBag.DataForTat = JsonConvert.SerializeObject(resValues);
        }

        public void GetDataforValidandInv()
        {
            int year = (int)TempData["selectedYear"]!;
            TempData.Keep();

            IEnumerable<IssueModel> issues = _Db.IssueDb.Where(j => !j.isDeleted && j.ValidatedStatus && j.DateFound.Year == year);

            ViewBag.invalidforpie = issues.Count(j => j.ValidatedStatus && j.ValRes == "Invalid");
            ViewBag.validforpie = issues.Count(j => j.ValidatedStatus && j.ValidatedStatus && j.ValRes == "Valid");
        }

        public IActionResult GetInvalids()
        {
            IEnumerable<IssueModel> issues = _Db.IssueDb.Where(j => !j.isDeleted && j.ValidatedStatus && j.ValRes == "Invalid");

            TempData["sfd"] = "Invalid Claims";

            return View("StatusFromDashboard", issues);
        }

        public IActionResult GetValids()
        {
            List<IssueModel> issues = _Db.IssueDb.Where(j => j.Acknowledged && j.ValidatedStatus && j.HasCR && j.HasTES && !j.isDeleted && j.ValRes == "Valid").ToList();
            List<ActionModel> actions = new List<ActionModel>();
            List<GetActionPercent> gaps = new List<GetActionPercent>();
            ViewData["ActionPercents"] = null;

            foreach (IssueModel issue in issues)
            {
                List<ActionModel> actionlist = _Db.ActionDb.Where(j => j.ControlNo == issue.ControlNumber).ToList();
                actions.AddRange(actionlist);
                
                GetActionPercent gap = new GetActionPercent();
                {
                    gap.ControlNo = issue.ControlNumber;
                    gap.Percent = (float)actionlist.Count(j => j.ActionStatus == "Closed") / (float)actionlist.Count();
                }

                gaps.Add(gap);

            }

            ViewData["ActionPercents"] = gaps;

            return View(issues);
        }

        //public void GetART_8D()
        //{
        //    int year = (int)TempData["selectedYear"];
        //    int getMonthIndex = 0;

        //    if (year != DateTime.Now.Year)
        //    {
        //        getMonthIndex = 12;
        //    }
        //    else
        //    {
        //        getMonthIndex = DateTime.Now.Month;
        //    }

        //    ViewBag.Month = getMonthIndex;

        //    GetART_8DData(getMonthIndex, year);

        //}

        //public void GetART_8DData(int month, int year)
        //{
        //    IEnumerable<ART_8DModel> arts = _Db.ART_8D.Where(j => j.ControlNo.Substring(3,2) == year.ToString().Substring(2,2));

        //    int[] data = new int[month + 1];

        //    for (int i = 1; i <= month; i++)
        //    {
        //        var thisMonth = arts.Where(j => j.DateValidated.Month == i);
        //        int totalDays = 0;
        //        int countofindex = thisMonth.Any() ? thisMonth.Count() : 1;

        //        foreach (var j in thisMonth)
        //        {
        //            DateTime startDate = j.DateValidated;
        //            DateTime endDate = j.DateClosed;

        //            TimeSpan totalSpan = TimeSpan.Zero;

        //            while (startDate < endDate)
        //            {
        //                if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
        //                {
        //                    totalSpan += TimeSpan.FromDays(1);
        //                }

        //                startDate = startDate.AddDays(1);
        //            }

        //            totalDays += totalSpan.Days;
        //        }

        //        data[i] = totalDays / countofindex;
        //    }

        //    int[] newData = new int[month];
        //    Array.Copy(data, 1, newData, 0, month);
        //    ViewBag.artData = JsonConvert.SerializeObject(newData);

        //}

        //public IActionResult GetClosed()
        //{
        //    int year = (int)TempData["selectedYear"];
        //    List<IssueModel> issues = new List<IssueModel>();

        //    foreach (var issue in _Db.IssueDb.Where(j => !j.isDeleted && j.ValidatedStatus && j.HasTES && j.DateFound.Year == year && j.ValRes == "Valid"))
        //    {
        //        bool hasClosedVermodel = false;

        //        foreach (var action in _Db.ActionDb.Where(j => !j.IsDeleted && j.ControlNo == issue.ControlNumber))
        //        {
        //            IEnumerable<Vermodel> vers = _Db.VerDb.Where(j => !j.IsDeleted && j.ActionID == action.ActionID);

        //            vers = vers.OrderByDescending(j => j.DateVer);

        //            if (vers.Any() && vers.First().Status == "c")
        //            {
        //                hasClosedVermodel = true;
        //            }
        //            else
        //            {
        //                hasClosedVermodel = false;
        //            }
        //        }

        //        if (hasClosedVermodel)
        //        {
        //            issues.Add(issue);
        //        }
        //    }

        //    TempData["svp"] = "Closed Valid Claims";

        //    return View("ShowValProg", issues);
        //}

        //public IActionResult GetOpen()
        //{
        //    int year = (int)TempData["selectedYear"];
        //    TempData.Keep();

        //    List<IssueModel> issues = new List<IssueModel>();

        //    IEnumerable<IssueModel> getIssues = _Db.IssueDb.Where(j => !j.isDeleted && j.HasTES && j.DateFound.Year == year && j.ValRes == "Valid");

        //    foreach (var issue in getIssues)
        //    {
        //        var actions = _Db.ActionDb.Where(j => !j.IsDeleted && j.ControlNo == issue.ControlNumber);

        //        if (actions == null || !actions.Any())
        //        {
        //            issues.Add(issue);
        //        }
        //        else if (actions.Any(a => !a.HasVer))
        //        {
        //            issues.Add(issue);
        //        }
        //        else if (actions.All(a => a.HasVer))
        //        {

        //            foreach (var action in actions)
        //            {
        //                var vers = _Db.VerDb.Where(j => !j.IsDeleted && j.ActionID == action.ActionID).OrderByDescending(j => j.DateVer);

        //                if (vers.Any() && vers.First().Status == "o")
        //                {
        //                    issues.Add(issue);
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    var addedIssues = _Db.IssueDb.Where(j => !j.isDeleted && j.ValidatedStatus && j.DateFound.Year == year && !j.HasTES && j.ValRes == "Valid");

        //    if (addedIssues.Any())
        //    {
        //        issues.AddRange(addedIssues);
        //    }

        //    TempData["svp"] = "Open Claims";

        //    return View("ShowValProg", issues);
        //}
    }

    public class GetActionPercent
    {
        public string? ControlNo { get; set; }
        public float Percent { get; set; }
    }

    class ResData
    {
        public int[]? Days { get; set; }
        public string? Validation { get; set; }
    }

}
