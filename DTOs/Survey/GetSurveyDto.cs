namespace FinalSurvey.DTOs.Survey
{
    public class GetSurveyDto
    {
        public int IdSurvey { get; set; }

        public string Name { get; set; } = null!;

        public DateTime RegisterDate { get; set; }

        public bool Status { get; set; }

        public Guid CategoryId { get; set; }
    }
}
