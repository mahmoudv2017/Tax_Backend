using Api.Dtos;
using Api.Error;
using Api.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxPayerController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork<TaxPayer> _taxpayerrepo;
        private readonly IUnitOfWork<TaxReturn> _taxReturnrepo;
        private readonly IMapper _mapper;


        public TaxPayerController(IMapper mapper  ,IUnitOfWork<TaxReturn> taxReturnrepo , UserManager<User> userManager , IUnitOfWork<TaxPayer> taxpayerrepo)
        {
            _taxpayerrepo = taxpayerrepo;
            _mapper = mapper;
            _userManager = userManager;
            _taxReturnrepo = taxReturnrepo;
        }
        [HttpPost("TaxReturn")]
        [Authorize]
        public async Task<ActionResult<TaxReponseDto>> AddTaxReturn(TaxReturnDto taxReturnDto)
        {
            var foundUser =  await _userManager.GetUserFromClaimsEmail(User);
            if (foundUser is null)
            {
                return BadRequest(new ApiReponse(401));
            }
            TaxReturn newTaxReturn = _mapper.Map<TaxReturnDto, TaxReturn>(taxReturnDto);

            newTaxReturn.taxHistories = new List<TaxHistory> { new TaxHistory {
                    Status=ActionStatus.UnderReview,
                    Timestamp=DateTime.Now,

                } };

            foundUser.taxPayer.TaxReturns.Add(newTaxReturn);

            await _taxReturnrepo.Add(newTaxReturn);

            await _taxpayerrepo.SaveChangeAsync();
            return new TaxReponseDto
            {
                status = "success",
                message = "Tax was added successfully"
            };
        }

        [HttpGet("checkmonth")]
        [Authorize]

        public async Task<ActionResult<bool>> CheckMonth(int month )
        {
            var foundUser = await  _userManager.GetUserFromClaimsEmail(User);
            if (foundUser is null)
            {
                return BadRequest(new ApiReponse(401));
            }
            var result = await _taxReturnrepo.GetOneEntityByExpression(tr => tr.FilingDate.Month == month && tr.TaxPayer == foundUser.taxPayer);


            return Ok(result == null);
         }


        [HttpGet("gethistory")]
        [Authorize]

        public async Task<ActionResult<List<TaxReturnDtoReponse>>> CheckMonth( int? month)
        {
            var foundUser = await _userManager.GetUserFromClaimsEmail(User);
            if(foundUser is null)
            {
                return BadRequest(new ApiReponse(401));
            }
            var TaxReturns = foundUser.taxPayer.TaxReturns;
            if (month is not null)
            {
                TaxReturns = (ICollection<TaxReturn>)TaxReturns.Where(tr => tr.FilingDate.Month == month).AsQueryable();
            }
            var result = TaxReturns.Select(tr => _mapper.Map<TaxReturn, TaxReturnDtoReponse>(tr)).ToList();

            return Ok(result);
        }
    }
}
