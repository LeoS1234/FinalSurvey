using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalSurvey.Models;

namespace FinalSurvey.Data
{
    public class DataContext : DbContext
    {
        public DataContext (DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<FinalSurvey.Models.User> User { get; set; } = default!;

        public DbSet<FinalSurvey.Models.Category> Category { get; set; }

        public DbSet<FinalSurvey.Models.Question> Question { get; set; }

        public DbSet<FinalSurvey.Models.QuestionAnswer> QuestionAnswer { get; set; }

        public DbSet<FinalSurvey.Models.Role> Role { get; set; }

        public DbSet<FinalSurvey.Models.Survey> Survey { get; set; }

        public DbSet<FinalSurvey.Models.UserAnswer> UserAnswer { get; set; }
    }
}
