using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FinalSurvey.DTOs.Question
{
    public class GetQuestionDto
    {
        public Guid IdQuestion { get; set; }

        public string QuestonTxt { get; set; } = null!;

        public string QuestionType { get; set; } = null!;

        public int SurveyId { get; set; }

        public int SurveyDto { get; set; }
    }
}
