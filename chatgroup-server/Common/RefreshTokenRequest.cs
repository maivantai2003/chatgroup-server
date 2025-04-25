﻿using System.ComponentModel.DataAnnotations;

namespace chatgroup_server.Common
{
    public class RefreshTokenRequest
    {
        [Required]
        public string? ExpiredToken { get; set; }
        [Required]
        public string? RefreshToken { get; set; }
    }
}
