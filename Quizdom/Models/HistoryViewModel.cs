using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quizdom.Models
{
    public class HistoryViewModel
    {
        public int ResultID { get; set; }
        public string ExamTitle { get; set; }
        public double Score { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}