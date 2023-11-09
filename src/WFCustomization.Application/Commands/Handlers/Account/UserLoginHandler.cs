using MediatR;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Threading;
using System.Threading.Tasks;
using WFCustomization.Application.Commands.Account;
using WFCustomization.Application.DTOs.Account;

namespace WFCustomization.Application.Commands.Handlers.Account
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class UserLoginHandler : IRequestHandler<UserLogin, UserLoginDto>
    {
        public async Task<UserLoginDto> Handle(UserLogin request, CancellationToken cancellationToken)
        {
            string ldapPath = "LDAP://10.30.20.46:389";
            using (DirectorySearcher searcher =
                new DirectorySearcher(
                    new DirectoryEntry(ldapPath, request.UserName, request.Password)))
            {
                searcher.Filter = $"(&(objectCategory=person)(objectClass=user)(sAMAccountName={request.UserName}))";
                searcher.SearchScope = SearchScope.Subtree;
                SearchResult result = searcher.FindOne();
                if (result == null)
                    throw new UnauthorizedAccessException();
                foreach (string propertyName in result.Properties.PropertyNames)
                {
                    // Print out attributes
                    foreach (var property in result.Properties[propertyName])
                    {
                        Console.WriteLine($"{propertyName}: {property}");
                    }
                }
                return new UserLoginDto
                {
                    Name = result.Properties["displayname"][0].ToString(),
                    Groups = GetGroups(result)

                };
            }
        }
        private static List<string> GetGroups(SearchResult result)
        {
            List<string> groups = new List<string>();
            for (int i = 0; i < result.Properties["memberof"].Count; i++)
            {
                groups.Add(result.Properties["memberof"][i].ToString().Trim());
            }

            return groups;
        }

    }
}
