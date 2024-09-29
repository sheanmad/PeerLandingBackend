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
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services
{
    public class BorrowerServices : IBorrowerServices
    {
        private readonly PeerlandingContext _context;
        private readonly IConfiguration _configuration;
        public BorrowerServices(PeerlandingContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<decimal> AddRequestLoan(ReqAddRequestLoanDto requestLoan)
        {
            var newLoan = new MstLoans
            {
                BorrowerId = requestLoan.BorrowerId,
                Amount = requestLoan.Amount,
                InterestRate = requestLoan.InterestRate,
                Duration = requestLoan.Duration,
                Status = requestLoan.Status,
                CreatedAt = requestLoan.CreatedAt,
                UpdatedAt = requestLoan.UpdatedAt
            };
            await _context.MstLoans.AddAsync(newLoan);
            await _context.SaveChangesAsync();

            return newLoan.Amount;
        }

        public async Task<List<ResGetRequestLoanDto>> GetAllRequestLoan(string Id)
        {
            return await _context.MstLoans
                .Where(loan => loan.BorrowerId == Id)
                .Select(loan => new ResGetRequestLoanDto
                {
                    loanId = loan.Id,
                    Amount = loan.Amount,
                    InterestRate = loan.InterestRate,
                    Duration = loan.Duration,
                    Status = loan.Status
                }).ToListAsync();
        }

        public async Task<ResGetLoanByLoanIdDto> GetLoanByLoanId(string Id)
        {
            var loans = await _context.MstLoans
            .Where(loan => loan.Id == Id)
            .Select(loan => new ResGetLoanByLoanIdDto
            {
                BorrowerId = loan.BorrowerId,
                Amount = loan.Amount,
                InterestRate = loan.InterestRate,
                Duration = loan.Duration,
                Status = loan.Status,
                CreatedAt = loan.CreatedAt,
                UpdatedAt = loan.UpdatedAt
            }).FirstOrDefaultAsync();

            return loans;
        }

        public async Task<ResGetLoanRepaidByLoanIdDto> GetLoanRepaidByLoanId(string Id)
        {
            decimal monthlyInterestRate = 2.5m / 12 / 100;
            decimal percentage = (decimal)(1 - Math.Pow((double)(1 + monthlyInterestRate), -12));
            var loans = await _context.MstLoans
            .Where(loan => loan.Id == Id)
            .Select(loan => new ResGetLoanRepaidByLoanIdDto
            {
                BorrowerId = loan.BorrowerId,
                PaidAmount = Math.Round((monthlyInterestRate * loan.Amount) / percentage,3),
                InterestRate = loan.InterestRate,
                Duration = loan.Duration,
                Status = loan.Status,
                CreatedAt = loan.CreatedAt,
                UpdatedAt = loan.UpdatedAt
            }).FirstOrDefaultAsync();

            return loans;
        }

        public async Task<ResGetLastPaymentByLoanIdDto> GetLastPaymentByLoanId(string Id)
        {
            var payment = await _context.TrnRepayment
            .Where(p => p.LoanId == Id)
            .OrderByDescending(p => p.PaidAt)
            .Select(payment => new ResGetLastPaymentByLoanIdDto
            {
                Amount = payment.Amount,
                RepaidAmount = payment.RepaidAmount,
                BalanceAmount = payment.BalanceAmount
            }).FirstOrDefaultAsync();
            return payment;
        }

        public async Task<ReqAddPaymentDto> AddPayment(string Id, ReqAddPaymentDto reqAddPayment)
        {
            var newPayment = new TrnRepayment
            {
                LoanId = Id,
                Amount = reqAddPayment.Amount,
                RepaidAmount = reqAddPayment.RepaidAmount,
                BalanceAmount = reqAddPayment.BalanceAmount,
                RepaidStatus = reqAddPayment.RepaidStatus,
                PaidAt = reqAddPayment.PaidAt
            };
            await _context.TrnRepayment.AddAsync(newPayment);
            await _context.SaveChangesAsync();
            return reqAddPayment;
        }
        public async Task<decimal?> UpdateSaldoPaymentBorrower(string borrowerId, ReqUpdateAmountDto reqUpdateAmount)
        {
            var borrower = await _context.MstUsers
            .Where(user => user.Id == borrowerId)
            .FirstOrDefaultAsync();

            borrower.Balance -= reqUpdateAmount.Amount;

            _context.MstUsers.Update(borrower);
            await _context.SaveChangesAsync();
            return borrower.Balance;
        }
        public async Task<decimal?> UpdateSaldoPaymentLender(string loanId, ReqUpdateAmountDto reqUpdateAmount)
        {
            var funding = await _context.TrnFundings
            .Where(funding => funding.LoanId == loanId)
            .FirstOrDefaultAsync();

            var lender = await _context.MstUsers
            .Where(user => user.Id == funding.LenderId)
            .FirstOrDefaultAsync();

            lender.Balance += reqUpdateAmount.Amount;
            _context.MstUsers.Update(lender);
            await _context.SaveChangesAsync();
            return lender.Balance;
        }

        public async Task<string> UpdateStatusRepay(string loanId, ReqUpdateStatusRepayDto reqUpdateStatusRepay)
        {
            var loan = await _context.MstLoans
            .Where(loan => loan.Id == loanId)
            .FirstOrDefaultAsync();

            loan.Status = reqUpdateStatusRepay.Status;
            loan.UpdatedAt = reqUpdateStatusRepay.UpdatedAt;

            _context.MstLoans.Update(loan);
            await _context.SaveChangesAsync();
            return loan.Status;
        }
    }
}
