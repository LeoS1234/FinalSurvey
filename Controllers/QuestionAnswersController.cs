using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalSurvey.Data;
using FinalSurvey.Models;
using AutoMapper;
using FinalSurvey.DTOs.QuestionAnswer;
using Azure;
using FinalSurvey.DTOs.Category;

namespace FinalSurvey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionAnswersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public QuestionAnswersController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/QuestionAnswers
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetQuestionAnswerDto>>>> GetQuestionAnswer()
        {
            var response = new ServiceResponse<IEnumerable<GetQuestionAnswerDto>>();

            var questionA = await _context.QuestionAnswer.ToListAsync();

            response.Data = questionA.Select(c => _mapper.Map<GetQuestionAnswerDto>(c)).ToList();

            return Ok(response);
        }

        // GET: api/QuestionAnswers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetQuestionAnswerDto>>> GetQuestionAnswer(Guid id)
        {
            var response = new ServiceResponse<GetQuestionAnswerDto>();
            var questionAns = await _context.QuestionAnswer.FirstOrDefaultAsync(c => c.IdQuestionAnswer.ToString().ToUpper() == id.ToString().ToUpper());

            if (questionAns != null)
            {
                response.Data = _mapper.Map<GetQuestionAnswerDto>(questionAns);
            }
            else
            {
                response.Success = false;
                response.Message = "QUESTION ANSWER NOT FOUND";

                return NotFound(response);
            }

            return Ok(response);
        }

        // PUT: api/QuestionAnswers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetQuestionAnswerDto>>> PutCategory(UpdateQuestionAnswerDto questionAnswer, Guid id)
        {
   
            ServiceResponse<GetQuestionAnswerDto> response = new ServiceResponse<GetQuestionAnswerDto>();
            try
            {
                var questionAns = await _context.QuestionAnswer
                    .Include(q => q.Question)
                        .ThenInclude(s => s.Survey)
                            .ThenInclude(c => c.Category)
                    .FirstOrDefaultAsync(qa => qa.IdQuestionAnswer.ToString().ToUpper() == id.ToString().ToUpper());

                if (QuestionAnswerExists(id))
                {
                    _mapper.Map(questionAnswer, questionAns);

                    await _context.SaveChangesAsync();

                    response.Data = _mapper.Map<GetQuestionAnswerDto>(questionAns);
                }
                else
                {
                    response.Success = false;
                    response.Message = "QUESTION ANSWER NOT FOUND";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);

        }

        // POST: api/QuestionAnswers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetQuestionAnswerDto>>>> PostQuestionAnswer(AddQuestionAnswerDto questionAnswer)
        {
            var response = new ServiceResponse<IEnumerable<GetQuestionAnswerDto>>();

            QuestionAnswer questionAn = _mapper.Map <QuestionAnswer>(questionAnswer);

            _context.QuestionAnswer.Add(questionAn);

            await _context.SaveChangesAsync();

            response.Data = await _context.QuestionAnswer.Select(c => _mapper.Map<GetQuestionAnswerDto>(c)).ToListAsync();

            return Ok(response);
        }

        // DELETE: api/QuestionAnswers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetQuestionAnswerDto>>>> DeleteQuestionAnswer(Guid id)
        {
            ServiceResponse<IEnumerable<GetQuestionAnswerDto>> response = new ServiceResponse<IEnumerable<GetQuestionAnswerDto>>();

            try
            {
                QuestionAnswer questionAnswer = await _context.QuestionAnswer.FirstOrDefaultAsync(c => c.IdQuestionAnswer.ToString().ToUpper() == id.ToString().ToUpper());

                if (questionAnswer != null)
                {
                    _context.QuestionAnswer.Remove(questionAnswer);
                    await _context.SaveChangesAsync();

                    response.Data = _context.QuestionAnswer.Select(c => _mapper.Map<GetQuestionAnswerDto>(c)).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "QUESTION ANSWER NOT FOUND";

                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        private bool QuestionAnswerExists(Guid id)
        {
            return _context.QuestionAnswer.Any(e => e.IdQuestionAnswer == id);
        }
    }
}
