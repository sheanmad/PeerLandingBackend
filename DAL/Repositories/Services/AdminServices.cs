using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Models;
using DAL.Repositories.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly PeerlandingContext _context;
        private readonly IConfiguration _configuration;
        public AdminServices(PeerlandingContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<string> AddUser(ReqRegisterUserDto register)
        {
            var isAnyEmail = await _context.MstUsers.SingleOrDefaultAsync(e => e.Email == register.Email);
            if (isAnyEmail != null)
            {
                throw new Exception("Email already used");
            }

            var newUser = new MstUser
            {
                Name = register.Name,
                Email = register.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(register.Password),
                Role = register.Role,
                Balance = register.Balance,
            };
            await _context.MstUsers.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return newUser.Name;
        }

        public async Task<List<ResUserDto>> GetAllUsers()
        {
            return await _context.MstUsers
                .Where(user => user.Role != "admin")
                .Select(user => new ResUserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    Balance = user.Balance
                }).ToListAsync();
        }

        public async Task<string> Delete(string userId)
        {
            var user = await _context.MstUsers.SingleOrDefaultAsync(user => user.Id == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            _context.MstUsers.Remove(user);
            await _context.SaveChangesAsync();
            var username = user.Name;
            return username;
        }

        public async Task<string> UpdateUser(string userId, ReqUpdateUserDto reqUpdate)
        {
            var existingUser = await _context.MstUsers.SingleOrDefaultAsync(user => user.Id == userId);

            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            existingUser.Name = reqUpdate.Name ?? existingUser.Name;
            existingUser.Role = reqUpdate.Role ?? existingUser.Role;
            existingUser.Balance = reqUpdate.Balance ?? existingUser.Balance;

            _context.MstUsers.Update(existingUser);
            await _context.SaveChangesAsync();

            return reqUpdate.Name;

        }

        public async Task<ResGetUserByIdDto> GetUserById(string Id)
        {
            var users = await _context.MstUsers
            .Where(user => user.Id == Id)
            .Select(user => new ResGetUserByIdDto
            {
                Id = user.Id,
                Name = user.Name,
                Role = user.Role,
                Balance = user.Balance
            }).FirstOrDefaultAsync();

            return users;
        }
    }
}
