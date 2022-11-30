namespace FinalSurvey.DTOs.UserAnswer
{
    public class GetUserAnswerDto
    {
        public Guid IdUserAnswer { get; set; }

        public string UserAns { get; set; } = null!;

        public Guid UserId { get; set; }

        public Guid QuestionId { get; set; }
    }
}
