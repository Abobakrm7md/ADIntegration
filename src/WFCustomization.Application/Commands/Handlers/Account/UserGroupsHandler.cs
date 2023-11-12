using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WFCustomization.Application.Commands.Account;
using WFCustomization.Application.DTOs.Account;
using WFCustomization.Shared;

namespace WFCustomization.Application.Commands.Handlers.Account
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class UserGroupsHandler : IRequestHandler<UserGroups, List<UserGroupsDto>>
    {
        Task<List<UserGroupsDto>> IRequestHandler<UserGroups, List<UserGroupsDto>>.Handle(UserGroups request, CancellationToken cancellationToken)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "10.30.20.46", request.UserName, request.Password);
            UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, request.UserName);
            if (userPrincipal == null)
                throw new ApplicationException("User not exist");
            var userGroups = userPrincipal.GetGroups(ctx).Select(x => new UserGroupsDto
            {
                Id = x.Guid,
                Name = x.Name,
            }).ToList();
            return Task.FromResult(userGroups);
        }
    }
}
