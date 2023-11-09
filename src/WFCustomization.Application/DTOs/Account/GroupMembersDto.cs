using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFCustomization.Application.DTOs.Account
{
    public class GroupMembersDto
    {
        public Guid? Id { get; set; }
        public string SamAccountName { get; set; }
        public string DisplayName { get; set; }
    }
}
