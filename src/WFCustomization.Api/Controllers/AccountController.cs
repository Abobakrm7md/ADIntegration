using inventory.Engines.LdapAuth;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WFCustomization.Application.Commands.Account;
using WFCustomization.Application.DTOs;
using WFCustomization.Application.DTOs.Account;

namespace WFCustomization.Api.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IMediator mediator)
            : base(mediator) { }

        /// <summary>
        /// Integrate with active directory
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin model)
        {
            return Ok(await Mediator.Send(model));
        }
        /// <summary>
        /// Gel all groups
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(GroupsListDto) ,StatusCodes.Status200OK)]
        [HttpPost("groupsList")]
        public async Task<IActionResult> GroupsList(UserCredentials credentials)
        {
            return Ok(await Mediator.Send(credentials));
        }

        [ProducesResponseType(typeof(List<GroupMembersDto>), StatusCodes.Status200OK)]
        [HttpPost("groupmembers")]
        public async Task<IActionResult> GroupMembers(GroupMempers group) => 
            Select(await Mediator.Send(group));

        [ProducesResponseType(typeof(List<UserGroupsDto>), StatusCodes.Status200OK)]
        [HttpPost("usergroups")]
        public async Task<IActionResult> GetUserGroups(UserGroups query) => 
            Select(await Mediator.Send(query));


        [HttpPost("loginvianovell")]
        public async Task<IActionResult> LoginViaNovell(UserLogin userLogin)
        {
            LdapAuthenticationService ldapAuthenticationService = new LdapAuthenticationService();
            var user = ldapAuthenticationService.Login(userLogin.UserName, userLogin.Password);
            return Ok(user);
        }
    }
}

