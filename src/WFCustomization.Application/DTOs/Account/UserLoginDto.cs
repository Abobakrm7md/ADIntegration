using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFCustomization.Application.DTOs.Account
{
    public class UserLoginDto
    {
        public string Name { get; set; }
        public List<string> Groups { get; set; }
    }
}
