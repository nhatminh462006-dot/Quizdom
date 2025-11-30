using Quizdom.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Quizdom.Controllers
{
    public class ExamController : Controller
    {
        QuizApp4Entities1 db = new QuizApp4Entities1();

        // ----------------------------
        // HIỂN THỊ TRANG LÀM BÀI THI
        // ----------------------------
        public ActionResult Start(int examId)
        {
            // Lấy bài thi + các câu hỏi + lựa chọn
            var exam = db.Exams
             .Include("ExamsQuestions")
             .Include("ExamsQuestions.Question")
             .Include("ExamsQuestions.Question.Choices")
             .FirstOrDefault(e => e.ExamID == examId);

            var model = new ExamViewModel
            {
                ExamID = exam.ExamID,
                Title = exam.Title,
                DurationMinutes = exam.DurationMinutes,
                Questions = exam.ExamsQuestions.Select(eq => new QuestionViewModel
                {
                    QuestionID = eq.Question.QuestionID,
                    QuestionText = eq.Question.QuestionText,
                    Choices = eq.Question.Choices.Select(c => new ChoiceViewModel
                    {
                        ChoiceID = c.ChoiceID,
                        ChoiceText = c.ChoiceText
                    }).ToList()
                }).ToList()
            };


            return View(model);
        }

        // 3.2. Xử lý Nộp bài và Hiển thị Trang điểm
        [HttpPost]
        public ActionResult Submit(int examId, FormCollection form)
        {
            // 1. Tải TẤT CẢ các đáp án đúng cho bài thi này
            var correctAnswers = db.ExamsQuestions
                                   .Where(eq => eq.ExamID == examId)
                                   .Select(eq => eq.Question.Choices.FirstOrDefault(c => c.IsCorrect == true))
.Where(c => c != null)
                                   .ToList();

            int totalQuestions = correctAnswers.Count;
            int correctCount = 0;

            // 2. So sánh với câu trả lời của người dùng
            foreach (var correctAnswer in correctAnswers)
            {
                // Tên trường trong form là "Q_" + QuestionID (ví dụ: Q_1, Q_2)
                string formFieldName = "Q_" + correctAnswer.QuestionID;
                string userChoiceIdString = form[formFieldName];

                if (!string.IsNullOrEmpty(userChoiceIdString))
                {
                    // ChoiceID người dùng chọn có khớp với ChoiceID đúng không?
                    if (userChoiceIdString == correctAnswer.ChoiceID.ToString())
                    {
                        correctCount++;
                    }
                }
            }

            // 3. Tính điểm
            int score = (totalQuestions > 0)
                        ? (int)(((double)correctCount / totalQuestions) * 100)
                        : 0;

            // 4. LƯU KẾT QUẢ vào bảng Results
            int userId = 1; // Giả định UserID (cần lấy từ Session/Cookie thực tế)

            var result = new Result
            {
                UserID = userId,
                ExamID = examId,
                Score = score,
                SubmittedAt = System.DateTime.Now
            };

            db.Results.Add(result);
            db.SaveChanges();

            // 5. Chuyển hướng đến trang điểm
            return RedirectToAction("Result", new { resultId = result.ResultID });
        }


        // 3.3. Hiển thị Trang điểm
        public ActionResult Result(int resultId)
        {
            // Lấy thông tin kết quả từ cơ sở dữ liệu
            var result = db.Results.Include("Exam").Include("User").FirstOrDefault(r => r.ResultID == resultId);

            if (result == null)
            {
                return HttpNotFound();
            }

            return View(result);
        }
    }
}