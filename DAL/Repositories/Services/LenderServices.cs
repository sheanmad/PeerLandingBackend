using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Models;
using DAL.Repositories.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services
{
    public class LenderServices : ILenderServices
    {
        private readonly PeerlandingContext _context;
        private readonly IConfiguration _configuration;
        public LenderServices(PeerlandingContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ResGetSaldoByIdDto> GetSaldoById(string Id)
        {
            var saldo = await _context.MstUsers
            .Where(user => user.Id == Id)
            .Select(user => new ResGetSaldoByIdDto
            {
                Balance = user.Balance
            }).FirstOrDefaultAsync();

            return saldo;
        }

        public async Task<decimal?> UpdateSaldoLender(string userId, ReqUpdateSaldoLenderDto reqUpdateSaldo)
        {
            var existingUser = await _context.MstUsers.SingleOrDefaultAsync(user => user.Id == userId);

            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            existingUser.Balance = reqUpdateSaldo.Balance ?? existingUser.Balance;

            _context.MstUsers.Update(existingUser);
            await _context.SaveChangesAsync();

            return reqUpdateSaldo.Balance;

        }

        public async Task<List<ResBorrowerDto>> GetAllBorrower()
        {

                var loans = await _context.MstLoans
                .Include(l => l.User)
                .Where(loan => loan.Status == "requested")
                .OrderByDescending(l => l.CreatedAt)
                .Select(loan => new ResBorrowerDto
                {
                    Id = loan.Id,
                    Name = loan.User.Name,
                    Amount = loan.Amount,
                    InterestRate = loan.InterestRate,
                    Duration = loan.Duration,
                    Status = loan.Status,
                }).ToListAsync();
                return loans;

        }

        public async Task<string> UpdateStatusLoan(string Id, ReqUpdateLoanDto reqUpdateLoan)
        {
            var existingLoan = await _context.MstLoans.SingleOrDefaultAsync(loan => loan.Id == Id);

            if (existingLoan == null)
            {
                throw new Exception("Loan not found");
            }

            existingLoan.Status = reqUpdateLoan.Status ?? existingLoan.Status;

            _context.MstLoans.Update(existingLoan);
            await _context.SaveChangesAsync();

            var newFunding = new TrnFunding
            {
                LoanId = Id,
                LenderId = reqUpdateLoan.LenderId,
                Amount = reqUpdateLoan.Amount,
                FundedAt = DateTime.UtcNow
            };
            await _context.TrnFundings.AddAsync(newFunding);
            await _context.SaveChangesAsync();

            return reqUpdateLoan.Status;
        }

        public async Task<decimal?> UpdateSaldoTransaksiLender(string lenderId, ReqUpdateAmountDto reqUpdateAmount)
        {
            var lender = await _context.MstUsers
            .Where(user => user.Id == lenderId)
            .FirstOrDefaultAsync();

            lender.Balance -= reqUpdateAmount.Amount;

            _context.MstUsers.Update(lender);
            await _context.SaveChangesAsync();
            return lender.Balance;
        }

        public async Task<decimal?> UpdateSaldoTransaksiBorrower(string loanId, ReqUpdateAmountDto reqUpdateAmount)
        {
            var loan = await _context.MstLoans
            .Where(loan => loan.Id == loanId)
            .FirstOrDefaultAsync();

            var borrower = await _context.MstUsers
            .Where(user => user.Id == loan.BorrowerId)
            .FirstOrDefaultAsync();
            
            borrower.Balance += reqUpdateAmount.Amount;
            _context.MstUsers.Update(borrower);
            await _context.SaveChangesAsync();
            return borrower.Balance;
        }
    }
}
