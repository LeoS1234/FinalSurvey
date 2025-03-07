﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FinalSurvey.DTOs.AuthUser
{
    public class GetUserDto
    {
        public Guid IdUser { get; set; }

        public string Name { get; set; } = null!;

        public string FirstSurname { get; set; } = null!;

        public string? LastSurname { get; set; }

        public byte[] PasswordHash { get; set; } = null!;

        public byte[] PasswordSalt { get; set; } = null!;

        public byte[]? Photo { get; set; }

        public bool? Status { get; set; }

    }
}
