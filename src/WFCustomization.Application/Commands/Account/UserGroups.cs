using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFCustomization.Application.DTOs.Account;

namespace WFCustomization.Application.Commands.Account
{
    public class UserGroups: IRequest<List<UserGroupsDto>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
