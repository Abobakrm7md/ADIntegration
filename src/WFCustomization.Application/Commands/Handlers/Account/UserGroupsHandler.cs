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
    public class UserGroupsHandler : IRequestHandler<UserGroups, List<UserGroupsDto>>
    {
        Task<List<UserGroupsDto>> IRequestHandler<UserGroups, List<UserGroupsDto>>.Handle(UserGroups request, CancellationToken cancellationToken)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "10.30.20.46", request.UserName, request.Password);
            UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, request.UserName);
            if (userPrincipal == null)
                throw new ApplicationException("User not exist");
            var x = userPrincipal.GetGroups();
            var userGroups = userPrincipal.GetAuthorizationGroups().Select(x => new UserGroupsDto
            {
                Id = x.Guid,
                Name = x.Name,
            }).ToList();
            return Task.FromResult(userGroups);
        }
    }
}
