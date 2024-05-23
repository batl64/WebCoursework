using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppShares.Data.Identity;

namespace WebAppShares.Pages.Admin
{
    [Authorize(Policy = "AdminPolicy")]
    public class UsersModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public List<UserDto> Users { get; set; }

        public void OnGet()
        {
            Users = _userManager.Users.Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                EmailConfirmed = u.EmailConfirmed,
                PhoneNumber= u.PhoneNumber
            }).ToList();
        }
    }
}
