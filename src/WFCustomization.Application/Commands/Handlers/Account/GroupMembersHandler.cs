using MediatR;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WFCustomization.Application.Commands.Account;
using WFCustomization.Application.DTOs.Account;

namespace WFCustomization.Application.Commands.Handlers.Account
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class GroupMembersHandler : IRequestHandler<GroupMempers, List<GroupMembersDto>>
    {
        Task<List<GroupMembersDto>> IRequestHandler<GroupMempers, List<GroupMembersDto>>.Handle(GroupMempers request, CancellationToken cancellationToken)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "10.30.20.46", request.UserName, request.Password);
            GroupPrincipal qbeGroup = GroupPrincipal.FindByIdentity(ctx, IdentityType.Name, request.Name);
            if (qbeGroup == null)
                throw new ApplicationException("Group not exist");

            var member = qbeGroup.GetMembers(true).Select(x =>
            new GroupMembersDto {Id = x.Guid, SamAccountName = x.SamAccountName, DisplayName = x.DisplayName }).ToList();
            qbeGroup.Dispose();
            ctx.Dispose();
            Task.Delay(0);
            return Task.FromResult(member);
        }
    }
}
