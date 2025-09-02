using lapCURDwebAPI.Entity;  // สำหรับ Calculate
using lapCURDwebAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace lapCURDwebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculateGradeController : ControllerBase
    {
        // คำนวณเกรดแบบกรอกเลขลง
        [HttpGet("Calculate")]
        public ActionResult<Calculate> GetGrade([FromQuery] int score)
        {
            var calculateResult = new Calculate
            {
                Score = score,
                Grade = CalculateGrade(score)
            };

            return Ok(calculateResult); // ส่งคืน Calculate
        }

        [HttpGet("ScoreRange")]
        public ActionResult<Calculate> GetScoreRange([FromQuery] string grade)
        {
            var (minScore, maxScore, message) = GetScoreRangeForGrade(grade);

            var calculateResult = new Calculate
            {
                Score = minScore, // ใช้คะแนนต่ำสุดในช่วง
                Grade = grade
            };

            // ตรวจสอบว่า message มีค่าหรือไม่
            var responseMessage = message == "ไม่มีเกรดดังกล่าว"
                ? message // ถ้า message มีค่า แสดง "ไม่มีเกรดดังกล่าว"
                : $"ช่วงคะแนนสำหรับเกรด {grade} คือ {minScore} ถึง {maxScore}";

            var result = new
            {
                Calculate = calculateResult,
                Message = responseMessage
            };


            return Ok(result);
        }

        // Method สำหรับคำนวณเกรด แบบตัวเลข
        private string CalculateGrade(int score)
        {
            if (score <= 100 && score >= 80)
            {
                return "A";
            }
            else if (score <= 79 && score >= 75)
            {
                return "B+";
            }
            else if (score <= 75 && score >= 70)
            {
                return "B";
            }
            else if (score <= 69 && score >= 65)
            {
                return "C+";
            }
            else if (score <= 65 && score >= 60)
            {
                return "C";
            }
            else if (score <= 59 && score >= 55)
            {
                return "D+";
            }
            else if (score <= 55 && score >= 50)
            {
                return "D";
            }
            else if (score >= 0 && score <= 49)
            {
                return "F";
            }
            else 
            {
                return "ไม่มีเกรดดั่งกล่าว";
            }
        }

        // Method สำหรับคำนวณเกรด แบบตัวหนังสือ
        private (int minScore, int maxScore, string message) GetScoreRangeForGrade(string grade)
        {
            switch (grade.ToUpper())
            {
                case "A":
                    return (80, 100, "เกรด A");
                case "B+":
                    return (75, 79, "เกรด B+");
                case "B":
                    return (70, 74, "เกรด B");
                case "C+":
                    return (65, 69, "เกรด C+");
                case "C":
                    return (60, 64, "เกรด C");
                case "D+":
                    return (55, 59, "เกรด D+");
                case "D":
                    return (50, 54, "เกรด D");
                case "F":
                    return (0, 49, "เกรด F");
                default:
                    return (0, 0, "ไม่มีเกรดดังกล่าว"); // หรือสามารถส่งคืนข้อผิดพลาดที่เหมาะสม
            }
        }

    }
}
