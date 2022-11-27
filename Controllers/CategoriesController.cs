using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FinalSurvey.Data;
using FinalSurvey.DTOs.Category;
using FinalSurvey.Models;
using Azure;

namespace FinalSurveyNTTDATA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetCategoryDto>>>> GetCategory()
        {
            var response = new ServiceResponse<IEnumerable<GetCategoryDto>>();

            var category = await _context.Category.ToListAsync();

            response.Data = category.Select(c => _mapper.Map<GetCategoryDto>(c)).ToList();

            return Ok(response);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCategoryDto>>> GetCategory(Guid id)
        {
            var response = new ServiceResponse<GetCategoryDto>();
            var category = await _context.Category.FirstOrDefaultAsync(c => c.IdCategory.ToString().ToUpper() == id.ToString().ToUpper());

            if (category != null)
            {
                response.Data = _mapper.Map<GetCategoryDto>(category);
            }
            else
            {
                response.Success = false;
                response.Message = "CATEGORY NOT FOUND";

                return NotFound(response);
            }

            return Ok(response);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCategoryDto>>> PutCategory(UpdateCategoryDto category, Guid id)
        {
            ServiceResponse<GetCategoryDto> response = new ServiceResponse<GetCategoryDto>();
            try
            {
                var categ = await _context.Category.FindAsync(id);

                if (CategoryExist(id))
                {
                    _mapper.Map(category, categ);

                    await _context.SaveChangesAsync();

                    response.Data = _mapper.Map<GetCategoryDto>(categ);
                }
                else
                {
                    response.Success = false;
                    response.Message = "CATEGORY NOT FOUND";
                }
            }
            catch (DbUpdateException ex)
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

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetCategoryDto>>>> PostCategory(AddCategoryDto category)
        {
            var response = new ServiceResponse<IEnumerable<GetCategoryDto>>();

            Category catego = _mapper.Map<Category>(category);

            _context.Category.Add(catego);

            await _context.SaveChangesAsync();

            response.Data = await _context.Category.Select(c => _mapper.Map<GetCategoryDto>(c)).ToListAsync();

            return Ok(response);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCategoryDto>>> DeleteCategory(Guid id)
        {
            ServiceResponse<IEnumerable<GetCategoryDto>> response = new ServiceResponse<IEnumerable<GetCategoryDto>>();

            try
            {
                Category category = await _context.Category.FirstOrDefaultAsync(c => c.IdCategory.ToString().ToUpper() == id.ToString().ToUpper());

                if (category != null)
                {
                    _context.Category.Remove(category);
                    await _context.SaveChangesAsync();

                    response.Data = _context.Category.Select(c => _mapper.Map<GetCategoryDto>(c)).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "CATEGORY NOT FOUND";

                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        private bool CategoryExist(Guid id)
        {
            return _context.Category.Any(e => e.IdCategory == id);
        }
    }
}