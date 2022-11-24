using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FinalSurvey.DTOs
{
    public class UserRegisterDto
    {

        public string Name { get; set; } = null!;

        public string FirstSurname { get; set; } = null!;

        public string? LastSurname { get; set; }

        public string Password { get; set; } = string.Empty;

        public byte[]? Photo { get; set; }

        public bool? Status { get; set; }

    }
}
