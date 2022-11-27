using AutoMapper;
using FinalSurvey.DTOs.AuthUser;
using FinalSurvey.DTOs.Category;
using FinalSurvey.DTOs.Question;
using FinalSurvey.DTOs.QuestionAnswer;
using FinalSurvey.DTOs.Role;
using FinalSurvey.DTOs.Survey;
using FinalSurvey.DTOs.UserAnswer;
using FinalSurvey.Models;

namespace FinalSurvey
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>();
            ////MAPEO POR CATEGORIA
            //CreateMap<Category, GetCategoryDto>();
            //CreateMap<AddCategoryDto, Category>();
            //CreateMap<UpdateCategoryDto, Category>();

            ////MAPEO POR QUESTION
            //CreateMap<Question, GetQuestionDto>();
            //CreateMap<AddQuestionDto, Question>();
            //CreateMap<UpdateQuestionDto, Question>();

            //Category mapping
            //CreateMap<Category, GetCategoryDto>();
            //CreateMap<AddCategoryDto, Category>();
            //CreateMap<UpdateCategoryDto, Category>();

            ////Category mapping
            //CreateMap<Question, GetQuestionDto>();
            //CreateMap<AddQuestionDto, Question>();
            //CreateMap<UpdateQuestionDto, Question>();

            ////Role mapping
            //CreateMap<Role, GetRoleDto>();
            //CreateMap<AddRoleDto, Role>();
            //CreateMap<UpdateRoleDto, Role>();

            ////QuestionAnswer mapping
            //CreateMap<QuestionAnswer, GetQuestionAnswerDto>();
            //CreateMap<AddQuestionAnswerDto, QuestionAnswer>();
            //CreateMap<UpdateQuestionAnswerDto, QuestionAnswer>();


            //User mapping
            CreateMap<User, GetUserDto>();

            //Category mapping
            CreateMap<Category, GetCategoryDto>();
            CreateMap<AddCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            //Question mapping
            CreateMap<Question, GetQuestionDto>();
            CreateMap<AddQuestionDto, Question>();
            CreateMap<UpdateQuestionDto, Question>();

            //QuestionAnswer mapping
            CreateMap<QuestionAnswer, GetQuestionAnswerDto>();
            CreateMap<AddQuestionAnswerDto, QuestionAnswer>();
            CreateMap<UpdateQuestionAnswerDto, QuestionAnswer>();

            //Role mapping
            CreateMap<Role, GetRoleDto>();
            CreateMap<AddRoleDto, Role>();
            CreateMap<UpdateRoleDto, Role>();

            //Survey mapping
            CreateMap<Survey, GetSurveyDto>();
            CreateMap<AddSurveyDto, Survey>();
            CreateMap<UpdateSurveyDto, Survey>();

            //UserAnswer mapping
            CreateMap<UserAnswer, GetUserAnswerDto>();
            CreateMap<AddUserAnswerDto, UserAnswer>();
            CreateMap<UpdateUserAnswerDto, UserAnswer>();
        }
    }
}
