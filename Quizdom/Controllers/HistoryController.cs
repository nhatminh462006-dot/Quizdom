using System.Linq;
using System.Web.Mvc;
using Quizdom.Models; // Đảm bảo sử dụng Models của bạn

public class HistoryController : Controller
{
    private QuizApp4Entities1 db = new QuizApp4Entities1(); // Context DB của bạn

    public ActionResult Index()
    {
        // Lấy UserID từ Session
        if (Session["UserID"] == null)
        {
            // Điều hướng nếu chưa đăng nhập (BẮT BUỘC)
            return RedirectToAction("Login", "Account");
        }

        // Đảm bảo UserID là kiểu int
        int userId = (int)Session["UserID"];

        // Truy vấn Lịch sử làm bài
        var history = db.Results
            // ĐIỂM CẦN KIỂM TRA: Đảm bảo rằng userId này khớp với cột UserID trong bảng Results
            .Where(r => r.UserID == userId)
            .OrderByDescending(r => r.SubmittedAt)
            .Select(r => new Quizdom.Models.HistoryViewModel
            {
                ResultID = r.ResultID,
                // ĐIỂM CẦN KIỂM TRA: Đảm bảo mối quan hệ Result -> Exam là hợp lệ
                ExamTitle = r.Exam.Title,
                Score = r.Score ?? 0.0m, // Đã sửa lỗi decimal?
                SubmittedAt = r.SubmittedAt
            })
            .ToList();

        return View(history);
    }
}