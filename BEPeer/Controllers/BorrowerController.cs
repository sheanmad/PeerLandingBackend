using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Repositories.Services;
using DAL.Repositories.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BEPeer.Controllers
{
    [Route("rest/v1/borrower/[action]")]
    [ApiController]
    public class BorrowerController : ControllerBase
    {
        private readonly IBorrowerServices _borrowerServices;

        public BorrowerController(IBorrowerServices borrowerServices)
        {
            _borrowerServices = borrowerServices;
        }

        [HttpPost]
        public async Task<IActionResult> AddRequestLoan([FromBody] ReqAddRequestLoanDto requestLoan)
        {
            try
            {
                var amount = await _borrowerServices.AddRequestLoan(requestLoan);
                return Ok(new ResBaseDto<decimal>
                {
                    Success = true,
                    Message = "Loan request added successfully",
                    Data = amount
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

        [HttpGet("{borrowerId}")]
        public async Task<IActionResult> GetAllRequestLoan(string borrowerId)
        {
            try
            {
                var loans = await _borrowerServices.GetAllRequestLoan(borrowerId);
                return Ok(new ResBaseDto<List<ResGetRequestLoanDto>>
                {
                    Success = true,
                    Message = "Request loans retrieved successfully",
                    Data = loans
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

        [HttpGet("{loanId}")]
        public async Task<IActionResult> GetLoanByLoanId(string loanId)
        {
            try
            {
                var loan = await _borrowerServices.GetLoanByLoanId(loanId);
                return Ok(new ResBaseDto<ResGetLoanByLoanIdDto>
                {
                    Success = true,
                    Message = "Loan retrieved successfully",
                    Data = loan
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

        [HttpGet("{loanId}")]
        public async Task<IActionResult> GetLoanRepaidByLoanId(string loanId)
        {
            try
            {
                var repaidLoan = await _borrowerServices.GetLoanRepaidByLoanId(loanId);
                return Ok(new ResBaseDto<ResGetLoanRepaidByLoanIdDto>
                {
                    Success = true,
                    Message = "Repaid loan details retrieved successfully",
                    Data = repaidLoan
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

        [HttpGet("{loanId}")]
        public async Task<IActionResult> GetLastPaymentByLoanId(string loanId)
        {
            try
            {
                var lastPayment = await _borrowerServices.GetLastPaymentByLoanId(loanId);
                return Ok(new ResBaseDto<ResGetLastPaymentByLoanIdDto>
                {
                    Success = true,
                    Message = "Last payment retrieved successfully",
                    Data = lastPayment
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

        [HttpPost("{loanId}")]
        public async Task<IActionResult> AddPayment(string loanId, [FromBody] ReqAddPaymentDto reqAddPayment)
        {
            try
            {
                var payment = await _borrowerServices.AddPayment(loanId, reqAddPayment);
                return Ok(new ResBaseDto<ReqAddPaymentDto>
                {
                    Success = true,
                    Message = "Payment added successfully",
                    Data = payment
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
        [HttpPut("{borrowerId}")]
        public async Task<IActionResult> UpdateSaldoPaymentBorrower(string borrowerId, ReqUpdateAmountDto reqUpdateAmount)
        {
            try
            {
                var saldo = await _borrowerServices.UpdateSaldoPaymentBorrower(borrowerId, reqUpdateAmount);
                return Ok(new ResBaseDto<decimal?>
                {
                    Success = true,
                    Message = "Borrower saldo updated successfully",
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
        public async Task<IActionResult> UpdateSaldoPaymentLender(string loanId, ReqUpdateAmountDto reqUpdateAmount)
        {
            try
            {
                var saldo = await _borrowerServices.UpdateSaldoPaymentLender(loanId, reqUpdateAmount);
                return Ok(new ResBaseDto<decimal?>
                {
                    Success = true,
                    Message = "Lender saldo updated successfully",
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
        public async Task<IActionResult> UpdateStatusRepay(string loanId, ReqUpdateStatusRepayDto reqUpdateStatus)
        {
            try
            {
                var loan = await _borrowerServices.UpdateStatusRepay(loanId, reqUpdateStatus);
                return Ok(new ResBaseDto<string?>
                {
                    Success = true,
                    Message = "Loan status updated successfully",
                    Data = loan
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
