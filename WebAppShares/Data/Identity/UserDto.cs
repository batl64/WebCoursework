﻿namespace WebAppShares.Data.Identity
{

    public class UserDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public DateTimeOffset LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

    }


}
