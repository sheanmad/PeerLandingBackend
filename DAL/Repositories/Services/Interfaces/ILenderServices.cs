using DAL.DTO.Req;
using DAL.DTO.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Interfaces
{
    public interface ILenderServices
    {
        Task<ResGetSaldoByIdDto> GetSaldoById(string Id);
        Task<decimal?> UpdateSaldoLender(string userId, ReqUpdateSaldoLenderDto reqUpdateSaldo);
        Task<List<ResBorrowerDto>> GetAllBorrower();
        Task<string> UpdateStatusLoan(string Id, ReqUpdateLoanDto reqUpdateLoan);
        Task<decimal?> UpdateSaldoTransaksiLender(string lenderId, ReqUpdateAmountDto reqUpdateAmount);
        Task<decimal?> UpdateSaldoTransaksiBorrower(string loanId, ReqUpdateAmountDto reqUpdateAmount);
    }
}
