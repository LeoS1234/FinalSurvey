using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalSurvey.Data;
using FinalSurvey.Models;
using FinalSurvey.Services.AuthService;
using FinalSurvey.DTOs.AuthUser;
using AutoMapper;
using FinalSurvey.DTOs.Category;
using Azure.Core;

namespace FinalSurvey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AuthController(DataContext context, IAuthService authService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            //Datos insertados por usuario
            var response = await _authService.Register(
                new User
                {
                    Name = request.Name,
                    FirstSurname = request.FirstSurname,
                    LastSurname = request.LastSurname,
                    Status = request.Status,
                    Photo = request.Photo,
                }, request.Password
            );

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("userRole")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> AddUserRole(AddUserRoleDto newUserRole)
        {
            var serviceResp = new ServiceResponse<GetUserDto>();

            try
            {
                var user = await _context.User
                                .Include(r => r.Roles)
                                .FirstOrDefaultAsync(u => u.IdUser == newUserRole.UserId);
                if (user == null)
                {
                    serviceResp.Success = false;
                    serviceResp.Message = "USER NOT FOUND";
                    return NotFound(serviceResp);
                }

                var role = await _context.Role
                                .FirstOrDefaultAsync(r => r.IdRole == newUserRole.RoleId);
                if (role == null)
                {
                    serviceResp.Success = false;
                    serviceResp.Message = "ROLE NOT FOUND";
                    return NotFound(serviceResp);
                }

                user.Roles.Add(role);
                await _context.SaveChangesAsync();
                serviceResp.Data = _mapper.Map<GetUserDto>(user);
            }
            catch (Exception ex)
            {
                serviceResp.Success = false;
                serviceResp.Message = ex.Message;
            }

            return serviceResp;
        }

            [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request)
        {
            var response = await _authService.Login(request.User, request.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        // GET: api/Auth
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetUserDto>>>> GetUser()
        {
            var response = new ServiceResponse<IEnumerable<GetUserDto>>();

            var user = await _context.User.Include(r => r.Roles).ToListAsync();

            response.Data = user.Select(c => _mapper.Map<GetUserDto>(c)).ToList();

            return Ok(response);
        }

        // GET: api/Auth/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetUserDto>>>> GetUser(Guid id)
        { 
            var response = new ServiceResponse<GetUserDto>();
            var user = await _context.User.Include(r => r.Roles).FirstOrDefaultAsync(c => c.IdUser.ToString().ToUpper() == id.ToString().ToUpper());

            if (user != null)
            {
                response.Data = _mapper.Map<GetUserDto>(user);
            }
            else
            {
                response.Success = false;
                response.Message = "CATEGORY NOT FOUND";

                return NotFound(response);
            }

            return Ok(response);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> PutUser(UpdateUserDto request, Guid id)
        {
            var response = await _authService.Register(
               new User
               {
                   Name = request.Name,
                   FirstSurname = request.FirstSurname,
                   LastSurname = request.LastSurname,
                   Status = request.Status,
                   Photo = request.Photo,
               }, request.Password
           );

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        //// PUT: api/Auth/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(int id, User user)
        //{
        //    if (id != user.IdUser)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Auth
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(User user)
        //{
        //    _context.User.Add(user);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.IdUser }, user);
        //}

        // DELETE: api/Auth/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetUserDto>>>> DeleteUser(Guid id)
        {
            ServiceResponse<IEnumerable<GetUserDto>> serviceResponse = new ServiceResponse<IEnumerable<GetUserDto>>();

            try
            {
                User use = await _context.User.FirstOrDefaultAsync(c => c.IdUser.ToString().ToUpper().Equals(id.ToString().ToUpper()));

                if (use != null)
                {
                    _context.User.Remove(use);
                    await _context.SaveChangesAsync();

                    serviceResponse.Data = _context.User.Select(c => _mapper.Map<GetUserDto>(c)).ToList();
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Usuario No Encontrado";

                    return NotFound(serviceResponse);
                }
            }
            catch (Exception ex)
            {

                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}
