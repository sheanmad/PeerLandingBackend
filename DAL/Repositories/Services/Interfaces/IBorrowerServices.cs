using DAL.DTO.Req;
using DAL.DTO.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Interfaces
{
    public interface IBorrowerServices
    {
        Task<decimal> AddRequestLoan(ReqAddRequestLoanDto requestLoan);
        Task<List<ResGetRequestLoanDto>> GetAllRequestLoan(string Id);
        Task<ResGetLoanByLoanIdDto> GetLoanByLoanId(string Id);
        Task<ResGetLoanRepaidByLoanIdDto> GetLoanRepaidByLoanId(string Id);
        Task<ResGetLastPaymentByLoanIdDto> GetLastPaymentByLoanId(string Id);
        Task<ReqAddPaymentDto> AddPayment(string Id, ReqAddPaymentDto reqAddPayment);
        Task<decimal?> UpdateSaldoPaymentBorrower(string borrowerId, ReqUpdateAmountDto reqUpdateAmount);
        Task<decimal?> UpdateSaldoPaymentLender(string loanId, ReqUpdateAmountDto reqUpdateAmount);
        Task<string> UpdateStatusRepay(string loanId, ReqUpdateStatusRepayDto reqUpdateStatusRepay);

    }
}
