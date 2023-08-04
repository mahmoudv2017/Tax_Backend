using Api.Dtos;
using Api.Error;
using Api.Extensions;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork<TaxPayer> _taxpayerrepo;
        private readonly IUnitOfWork<Admin> _Adminrepo;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenProvider _tokenProvider;
        public AccountController(IUnitOfWork<TaxPayer> taxpayerrepo , IUnitOfWork<Admin> Adminrepo , UserManager<User> userManager, SignInManager<User> signInManager , ITokenProvider tokenProvider )
        {
            _userManager = userManager;
            _Adminrepo = Adminrepo;
            _signInManager = signInManager;
            _tokenProvider = tokenProvider;
            _taxpayerrepo = taxpayerrepo;

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            var founduser = await _userManager.FindByEmailAsync(login.Email);
            if (founduser is null) { return BadRequest(new ApiReponse(400, "Invalid Email Or Password")); }

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(founduser, login.Password,false);
            if (!passwordCheck.Succeeded) { return BadRequest(new ApiReponse(400, "Invalid Email Or Password")); }

            return Ok(new UserDto
            {
                DisplayName=founduser.DisplayName,
                Email= founduser.Email,
                Role = founduser.Role,
                Token = await _tokenProvider.createToken(founduser)
            });
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmail([FromQuery]string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpGet("Checkssn")]
        public async Task<ActionResult<bool>> CheckSSN([FromQuery] string ssn)
        {
            return await _userManager.Users.FirstOrDefaultAsync(user => user.SSN == ssn) != null;
        }

      

        [HttpGet("getuser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> LoadUserFromClaims()
        {
            var FoundUser = await _userManager.GetUserFromClaimsEmail(User);
            if(FoundUser is null) { return BadRequest(new ApiReponse(401)); }
            return new UserDto
            {
                DisplayName = FoundUser.DisplayName,
                Email = FoundUser.Email,
                Role=FoundUser.Role,
                Token = await _tokenProvider.createToken(FoundUser)
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto register)
        {
            var checkssn = await _userManager.Users.FirstOrDefaultAsync(user => user.SSN== register.SSN);
            if (checkssn is not null) { return BadRequest(new ApiReponse(401 , "This Social Security Number Already Exists")); }
            var newUser = new User {
                Email = register.Email,
                DisplayName = register.DisplayName,
                SSN = register.SSN,
                Role= register.Role,
                PhoneNumber=register.PhoneNumber,
                UserName = register.Username,
                Address = new Address
                {
                    City = register.City,
                    PostalCode = register.PostalCode,
                    Country = register.Country,
                    State = register.State
                }
            };

         


            var CreatedUser = await _userManager.CreateAsync(newUser , register.Password);
            if (!CreatedUser.Succeeded) { return BadRequest( new ApiReponse(400) ); }
           await _userManager.AddToRoleAsync(newUser, register.Role);
            if (!CreatedUser.Succeeded) { return BadRequest(new ApiReponse(400)); }

            if (register.Role == "TaxPayer")
            {
                var newTaxpayer = new TaxPayer
                {
                    User = newUser,
                };
               await _taxpayerrepo.Add(newTaxpayer);
            }
            else
            {
                var newAdmin = new Admin
                {
                    User = newUser,
                };
                await _Adminrepo.Add(newAdmin);
            }
            await _taxpayerrepo.SaveChangeAsync();

            return Ok(new UserDto
            {
                DisplayName = newUser.DisplayName,
                Email = newUser.Email,
                Role = newUser.Role,
                Token = await _tokenProvider.createToken(newUser)
            });
        }
    }
}
