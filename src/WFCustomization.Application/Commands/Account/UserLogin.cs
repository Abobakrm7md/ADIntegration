using MediatR;
using WFCustomization.Application.DTOs.Account;

namespace WFCustomization.Application.Commands.Account
{
    public class UserLogin : IRequest<UserLoginDto>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

}
