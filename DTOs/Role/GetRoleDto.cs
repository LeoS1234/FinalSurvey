﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FinalSurvey.DTOs.Role
{
    public class GetRoleDto
    {
        public Guid IdRole { get; set; }

        public string Name { get; set; } = null!;
    }
}
