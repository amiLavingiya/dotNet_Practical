using Microsoft.AspNetCore.Mvc;

namespace DotNetPratical.Controllers
{
    public class TimetableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CalculateHours(int WorkingDays, int subjectsPerDay, int totalSubjects)
        {
            try
            {
                int totalHours = WorkingDays * subjectsPerDay;
                ViewBag.TotalHours = totalHours;
                ViewBag.TotalSubjects = totalSubjects;
                TempData["WorkingDays"] = WorkingDays;
                TempData["subjectsPerDay"] = subjectsPerDay;
                return View("SubjectPerHours");
                 
            }
            catch (Exception ex)
            {
            }
            return View();
        }
        [HttpPost]
        public IActionResult Generate(List<string> subjects, List<int> hours, int totalHours)
        {
            if (hours.Sum() != totalHours)
            {
                ViewBag.Error = "Total hours for subjects must equal total hours for the week.";
                return View("SubjectPerHours");
            }
    

            var timetable = GenerateTimetable(subjects, hours);
            return View("Timetable", timetable);
        }

        public List<List<string>> GenerateTimetable(List<string> subjects, List<int> hours)
        {
            var timetable = new List<List<string>>();
            var subjectQueue = subjects.SelectMany((subject, index) => Enumerable.Repeat(subject, hours[index])).ToList();
            var random = new Random();
            subjectQueue = subjectQueue.OrderBy(_ => random.Next()).ToList();

            int index = 0;
            var subjectsPerDays = Convert.ToInt32( TempData["subjectsPerDay"]);
            var WorkingDays =Convert.ToInt32( TempData["WorkingDays"]);
            for (int i = 0; i < WorkingDays; i++)
            {
                var daySchedule = new List<string>();
                for (int j = 0; j < subjectsPerDays; j++)
                {
                    daySchedule.Add(subjectQueue[index++]);
                }
                timetable.Add(daySchedule);
            }

            return timetable;
        }
    }


}

