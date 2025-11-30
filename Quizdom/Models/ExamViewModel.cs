using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Có thể cần nếu dùng validation

namespace Quizdom.Models // Đảm bảo namespace này khớp với thư mục Models
{
    // Lớp chính được sử dụng trong @model của View Start.cshtml
    public class ExamViewModel
    {
        // Dùng cho @Html.HiddenFor: để biết bài thi nào đang được submit
        public int ExamID { get; set; }

        // Thông tin hiển thị
        public string Title { get; set; }
        public int DurationMinutes { get; set; }

        // Danh sách các câu hỏi
        public List<QuestionViewModel> Questions { get; set; }
    }

    // Lớp Model cho mỗi câu hỏi
    public class QuestionViewModel
    {
        public int QuestionID { get; set; } // Dùng làm tiền tố cho input radio name
        public string QuestionText { get; set; }

        // Danh sách các lựa chọn
        public List<ChoiceViewModel> Choices { get; set; }
    }

    // Lớp Model cho mỗi lựa chọn
    public class ChoiceViewModel
    {
        public int ChoiceID { get; set; } // Dùng làm value cho input radio
        public string ChoiceText { get; set; }
    }
}
