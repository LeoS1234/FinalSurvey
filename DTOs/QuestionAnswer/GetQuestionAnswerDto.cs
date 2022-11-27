﻿using FinalSurvey.DTOs.Question;

namespace FinalSurvey.DTOs.QuestionAnswer
{
    public class GetQuestionAnswerDto
    {
        public Guid IdQuestionAnswer { get; set; }

        public string AnswerOption { get; set; } = null!;

        public bool Correct { get; set; }

        public bool Status { get; set; }

        public Guid QuestionId { get; set; }    

        public GetQuestionDto Question { get; set; }
    }
}
