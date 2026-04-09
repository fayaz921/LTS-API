using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LTS.API.Features.UserManangement.Commands.Authentication.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _appDbcontext;
        private readonly IPasswordHasher<User> _passwordHasher;
        public LoginUserCommandHandler(AppDbContext appDbcontext,IPasswordHasher<User> passwordHasher)
        {
            _appDbcontext = appDbcontext;
            _passwordHasher = passwordHasher;
        }
        public async Task<ApiResponse<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user =await _appDbcontext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return ApiResponse<string>.Fail("User Not Found");
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
                return ApiResponse<string>.Fail("User Not Found");
            return ApiResponse<string>.Ok(default!,"Login Success");
        }
    }
}
