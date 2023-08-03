using Api.Dtos;
using Api.Error;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork<TaxReturn> _taxReturnRepo;
        private readonly IMapper _mapper;
        public AdminController(IMapper mapper, UserManager<User> userManager , IUnitOfWork<TaxReturn> taxReturnRepo)
        {
            _userManager= userManager;
            _mapper = mapper;
            _taxReturnRepo= taxReturnRepo;
        }
        [HttpGet("allinfo/{ssn}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<UserInfoDto>> GetAllReturns(string ssn)
        {
            var foundUser = await _userManager.Users.FirstOrDefaultAsync(user => user.SSN == ssn);

            if(foundUser is null ) { return BadRequest(new ApiReponse(401 , "No Such User Exists")); }

            return Ok(

                new UserInfoDto
                {
                    UserName = foundUser.UserName,
                    PhoneNumber = foundUser.PhoneNumber,
                    Email = foundUser.Email,
                    UserId = foundUser.Id,
                    taxReturnDtoReponses = foundUser.taxPayer.TaxReturns.Select(tr => _mapper.Map<TaxReturn, TaxReturnDtoReponse>(tr)  ).ToList()
                });
                
                

        }

        [HttpPatch("changestatus/{taxReturnID}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<List<TaxReturnDto>>> ChangeStatus(StatusDto Status , int taxReturnID  )
        {
            var taxReturn = await _taxReturnRepo.GetOneEntityByExpression(tr => tr.Id== taxReturnID);

            if (taxReturn is null) { return BadRequest(new ApiReponse(401)); }
            taxReturn.Status = Status.NewStatus;
            taxReturn.taxHistories.Add(new TaxHistory
            {
                Status = Status.NewStatus,
                Timestamp= DateTime.Now,
            });
            await _taxReturnRepo.SaveChangeAsync();
            return Ok(new TaxReponseDto
            {
                status="Successeded",
                message="Tax Histroy updated"
            });

        }
    }
}
