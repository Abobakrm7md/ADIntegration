using MediatR;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WFCustomization.Application.Commands.Account;
using WFCustomization.Application.DTOs.Account;

namespace WFCustomization.Application.Commands.Handlers.Account
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class UserCredentialsHandler : IRequestHandler<UserCredentials, GroupsListDto>
    {
        public async Task<GroupsListDto> Handle(UserCredentials request, CancellationToken cancellationToken)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "10.30.20.46", request.UserName, request.Password);
            GroupPrincipal qbeGroup = new GroupPrincipal(ctx);
            PrincipalSearcher srch = new PrincipalSearcher(qbeGroup);
            var groupsResult = srch.FindAll();
            var groups = groupsResult.Select(x=>x.Name).ToList();
            await Task.Delay(0);
            return new GroupsListDto { Groups = groups };
        }
    }
}
