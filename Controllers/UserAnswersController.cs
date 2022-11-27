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
using FinalSurvey.DTOs.Category;
using FinalSurvey.DTOs.Survey;
using FinalSurvey.DTOs.UserAnswer;
using Azure;

namespace FinalSurvey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAnswersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserAnswersController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/UserAnswers
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetUserAnswerDto>>>> GetUserAnswer()
        {
            var response = new ServiceResponse<IEnumerable<GetUserAnswerDto>>();

            var userans = await _context.UserAnswer.ToListAsync();

            response.Data = userans.Select(c => _mapper.Map<GetUserAnswerDto>(c)).ToList();

            return Ok(response);
        }

        // GET: api/UserAnswers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetUserAnswerDto>>>> GetUserAnswer(int id)
        {
            var response = new ServiceResponse<GetUserAnswerDto>();
            var userans = await _context.Survey.FirstOrDefaultAsync(c => c.IdSurvey.ToString().ToUpper() == id.ToString().ToUpper());

            if (userans != null)
            {
                response.Data = _mapper.Map<GetUserAnswerDto>(userans);
            }
            else
            {
                response.Success = false;
                response.Message = "USER ANSWER NOT FOUND";

                return NotFound(response);
            }

            return Ok(response);
        }

        // PUT: api/UserAnswers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserAnswerDto>>> PutUserAnswer(Guid id, UserAnswer userAnswer)
        {
            ServiceResponse<GetUserAnswerDto> response = new ServiceResponse<GetUserAnswerDto>();
            try
            {
                var userans = await _context.Survey.FindAsync(id);

                if (UserAnswerExists(id))
                {
                    _mapper.Map(userAnswer, userans);

                    await _context.SaveChangesAsync();

                    response.Data = _mapper.Map<GetUserAnswerDto>(userans);
                }
                else
                {
                    response.Success = false;
                    response.Message = "USER ANSWER NOT FOUND";
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

            return response;
        }

        // POST: api/UserAnswers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetUserAnswerDto>>>> PostUserAnswer(AddUserAnswerDto userAns)
        {
            var response = new ServiceResponse<IEnumerable<GetUserAnswerDto>>();

            UserAnswer userans = _mapper.Map<UserAnswer>(userAns);

            _context.UserAnswer.Add(userans);

            await _context.SaveChangesAsync();

            response.Data = await _context.UserAnswer.Select(c => _mapper.Map<GetUserAnswerDto>(c)).ToListAsync();

            return response;
        }

        // DELETE: api/UserAnswers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetUserAnswerDto>>>> DeleteUserAnswer(Guid id)
        {
            ServiceResponse<IEnumerable<GetUserAnswerDto>> response = new ServiceResponse<IEnumerable<GetUserAnswerDto>>();

            try
            {
                UserAnswer usernas = await _context.UserAnswer.FirstOrDefaultAsync(c => c.IdUserAnswer == id);

                if (usernas != null)
                {
                    _context.UserAnswer.Remove(usernas);
                    await _context.SaveChangesAsync();

                    response.Data = _context.UserAnswer.Select(c => _mapper.Map<GetUserAnswerDto>(c)).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "USER ANSWER NOT FOUND";

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

        private bool UserAnswerExists(Guid id)
        {
            return _context.UserAnswer.Any(e => e.IdUserAnswer == id);
        }
    }
}
