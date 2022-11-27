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
using FinalSurvey.DTOs.Role;
using FinalSurvey.DTOs.Category;

namespace FinalSurvey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RolesController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetRoleDto>>>> GetRole()
        {
            var response = new ServiceResponse<IEnumerable<GetRoleDto>>();

            var role = await _context.Category.ToListAsync();

            response.Data = role.Select(c => _mapper.Map<GetRoleDto>(c)).ToList();

            return Ok(response);
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetRoleDto>>> GetRole(Guid id)
        {
            var response = new ServiceResponse<GetRoleDto>();
            var role = await _context.Role.FirstOrDefaultAsync(c => c.IdRole.ToString().ToUpper() == id.ToString().ToUpper());

            if (role != null)
            {
                response.Data = _mapper.Map<GetRoleDto>(role);
            }
            else
            {
                response.Success = false;
                response.Message = "ROLE NOT FOUND";

                return NotFound(response);
            }

            return Ok(response);
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetRoleDto>>> PutRole(UpdateRoleDto updatedRole, Guid id)
        {
            ServiceResponse<GetRoleDto> response = new ServiceResponse<GetRoleDto>();
            try
            {
                var role = await _context.Role.FindAsync(id);

                if (RoleExists(id))
                {
                    _mapper.Map(updatedRole, role);

                    await _context.SaveChangesAsync();

                    response.Data = _mapper.Map<GetRoleDto>(role);
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

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ServiceResponse<IEnumerable<GetRoleDto>>> PostRole(AddRoleDto role)
        {
            var response = new ServiceResponse<IEnumerable<GetRoleDto>>();

            Role rol = _mapper.Map<Role>(role);

            _context.Role.Add(rol);

            await _context.SaveChangesAsync();

            response.Data = await _context.Role.Select(c => _mapper.Map<GetRoleDto>(c)).ToListAsync();

            return response;

        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetRoleDto>>>> DeleteRole(Guid id)
        {
            ServiceResponse<IEnumerable<GetRoleDto>> response = new ServiceResponse<IEnumerable<GetRoleDto>>();

            try
            {
                Role rol = await _context.Role.FirstOrDefaultAsync(c => c.IdRole.ToString().ToUpper() == id.ToString().ToUpper());

                if (rol != null)
                {
                    _context.Role.Remove(rol);
                    await _context.SaveChangesAsync();

                    response.Data = _context.Role.Select(c => _mapper.Map<GetRoleDto>(c)).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "ROLE NOT FOUND";

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

        private bool RoleExists(Guid id)
        {
            return _context.Role.Any(e => e.IdRole == id);
        }
    }
}
