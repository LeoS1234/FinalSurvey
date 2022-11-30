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
using Azure;
using FinalSurvey.DTOs.Role;
using FinalSurvey.DTOs.QuestionAnswer;
using FinalSurvey.DTOs.Survey;
using FinalSurvey.DTOs.Category;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace FinalSurvey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveysController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SurveysController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Surveys
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetSurveyDto>>>> GetSurvey()
        {
            var response = new ServiceResponse<IEnumerable<GetSurveyDto>>();

            var survey = await _context.Survey.ToListAsync();

            response.Data = survey.Select(c => _mapper.Map<GetSurveyDto>(c)).ToList();

            return Ok(response);
        }

        // GET: api/Surveys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetSurveyDto>>>> GetSurvey(int id)
        {
            var response = new ServiceResponse<GetSurveyDto>();
            var survey = await _context.Survey.FirstOrDefaultAsync(c => c.IdSurvey.ToString().ToUpper() == id.ToString().ToUpper());
           
            if (survey != null)
            {
                response.Data = _mapper.Map<GetSurveyDto>(survey);
            }
            else
            {
                response.Success = false;
                response.Message = "SURVEY NOT FOUND";

                return NotFound(response);
            }

            return Ok(response);
        }

        // PUT: api/Surveys/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetSurveyDto>>> PutSurvey(int id, Survey upSurvey)
        {
            ServiceResponse<GetSurveyDto> response = new ServiceResponse<GetSurveyDto>();
            try
            {
                var survey = await _context.Survey.FindAsync(id);

                if (SurveyExists(id))
                {
                    _mapper.Map(upSurvey, survey);

                    await _context.SaveChangesAsync();

                    response.Data = _mapper.Map<GetSurveyDto>(survey);
                }
                else
                {
                    response.Success = false;
                    response.Message = "ROLE NOT FOUND";
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

        // POST: api/Surveys
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "admin")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetSurveyDto>>>> PostSurvey(AddSurveyDto survey)
        {
            var response = new ServiceResponse<IEnumerable<GetSurveyDto>>();

            Survey sur = _mapper.Map<Survey>(survey);

            _context.Survey.Add(sur);

            await _context.SaveChangesAsync();

            response.Data = await _context.Survey.Select(c => _mapper.Map<GetSurveyDto>(c)).ToListAsync();

            return response;

        }

        // DELETE: api/Surveys/5
        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetSurveyDto>>>> DeleteSurvey(int id)
        {
            ServiceResponse<IEnumerable<GetSurveyDto>> response = new ServiceResponse<IEnumerable<GetSurveyDto>>();

            try
            {
                Survey sur = await _context.Survey.FirstOrDefaultAsync(c => c.IdSurvey == id);

                if (sur != null)
                {
                    _context.Survey.Remove(sur);
                    await _context.SaveChangesAsync();

                    response.Data = _context.Survey.Select(c => _mapper.Map<GetSurveyDto>(c)).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "SURVEY NOT FOUND";

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

        private bool SurveyExists(int id)
        {
            return _context.Survey.Any(e => e.IdSurvey == id);
        }
    }
}
