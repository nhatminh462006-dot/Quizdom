using Quizdom.Models;
using System.Linq;
using System.Web.Mvc;

namespace Quizdom.Controllers
{
    public class LevelController : Controller
    {

        QuizApp4Entities1 db = new QuizApp4Entities1();

        public ActionResult Index(string level)
        {
            // Bắt đầu với tất cả các bài thi (Exam)
            var exams = db.Exams.AsQueryable();

            // Khai báo biến để lưu giá trị Level số
            byte targetLevel;

            if (!string.IsNullOrEmpty(level))
            {
                // Chuyển cấp độ string từ URL thành giá trị số (1 hoặc 2)
                if (level.ToLower() == "basic" || level.ToLower() == "cơ bản")
                {
                    targetLevel = 1; // 1: Cơ bản
                }
                else if (level.ToLower() == "advanced" || level.ToLower() == "nâng cao")
                {
                    targetLevel = 2; // 2: Nâng cao
                }
                else
                {
                    // Nếu level không hợp lệ, giữ nguyên exams và tiếp tục
                    targetLevel = 0;
                }

                // Thực hiện lọc chỉ khi targetLevel hợp lệ
                if (targetLevel > 0)
                {
                    // Lọc những Exam mà TẤT CẢ (All) các câu hỏi liên kết 
                    // qua ExamsQuestion đều có Level bằng targetLevel.
                    // Giả sử Navigation Property là 'ExamsQuestions'
                    exams = exams.Where(e => e.ExamsQuestions.Any(eq => eq.level == targetLevel));
                }
            }

            // Gán kết quả cuối cùng vào biến examList (đã sửa lỗi)
            var examList = exams.ToList();

            // Gửi tên level ra View
            ViewBag.Level = level;

            // Trả về danh sách đã lọc
            return View(examList);
        }

        public ActionResult SelectLevel()
        { return View(); }

    }
}