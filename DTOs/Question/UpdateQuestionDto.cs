﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FinalSurvey.DTOs.Question
{
    public class UpdateQuestionDto
    {
        public string QuestionTxt { get; set; } = null!;

        public string QuestionType { get; set; } = null!;

        public int SurveyId { get; set; }
    }
}
