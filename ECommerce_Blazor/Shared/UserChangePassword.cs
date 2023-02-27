using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce_Blazor.Shared
{
    public class UserChangePassword
    {
        [Required(ErrorMessage = "Your current password do not match")]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
        [Compare("Password", ErrorMessage = "The password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
