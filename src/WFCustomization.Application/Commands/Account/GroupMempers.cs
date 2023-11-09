using MediatR;
using System.Collections.Generic;
using WFCustomization.Application.DTOs.Account;

namespace WFCustomization.Application.Commands.Account
{
    public class GroupMempers : IRequest<List<GroupMembersDto>>
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
