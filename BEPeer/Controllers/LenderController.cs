using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Repositories.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BEPeer.Controllers
{
    [Route("rest/v1/lender/[action]")]
    [ApiController]
    public class LenderController : ControllerBase
    {
        private readonly ILenderServices _lenderServices;

        public LenderController(ILenderServices lenderServices)
        {
            _lenderServices = lenderServices;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaldoById(string id)
        {
            try
            {
                var saldo = await _lenderServices.GetSaldoById(id);
                return Ok(new ResBaseDto<ResGetSaldoByIdDto>
                {
                    Success = true,
                    Message = "Saldo retrieved successfully",
                    Data = saldo
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<ResGetSaldoByIdDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateSaldoLender(string userId, ReqUpdateSaldoLenderDto reqUpdateSaldo)
        {
            try
            {
                var saldo = await _lenderServices.UpdateSaldoLender(userId, reqUpdateSaldo);
                return Ok(new ResBaseDto<decimal?>
                {
                    Success = true,
                    Message = "Saldo updated successfully",
                    Data = saldo
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBorrowers()
        {
            try
            {
                var borrowers = await _lenderServices.GetAllBorrower();
                return Ok(new ResBaseDto<List<ResBorrowerDto>>
                {
                    Success = true,
                    Message = "List of borrowers retrieved",
                    Data = borrowers
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<List<ResBorrowerDto>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPut("{loanId}")]
        public async Task<IActionResult> UpdateStatusLoan(string loanId, ReqUpdateLoanDto reqUpdateLoan)
        {
            try
            {
                var status = await _lenderServices.UpdateStatusLoan(loanId, reqUpdateLoan);
                return Ok(new ResBaseDto<string>
                {
                    Success = true,
                    Message = "Loan status updated successfully",
                    Data = status
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPut("{lenderId}")]
        public async Task<IActionResult> UpdateSaldoTransaksiLender(string lenderId, ReqUpdateAmountDto reqUpdateAmount)
        {
            try
            {
                var saldo = await _lenderServices.UpdateSaldoTransaksiLender(lenderId, reqUpdateAmount);
                return Ok(new ResBaseDto<decimal?>
                {
                    Success = true,
                    Message = "Lender transaction updated successfully",
                    Data = saldo
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPut("{loanId}")]
        public async Task<IActionResult> UpdateSaldoTransaksiBorrower(string loanId, ReqUpdateAmountDto reqUpdateAmount)
        {
            try
            {
                var saldo = await _lenderServices.UpdateSaldoTransaksiBorrower(loanId, reqUpdateAmount);
                return Ok(new ResBaseDto<decimal?>
                {
                    Success = true,
                    Message = "Borrower transaction updated successfully",
                    Data = saldo
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
    }
}
