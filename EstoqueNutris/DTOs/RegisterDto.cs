﻿namespace EstoqueNutris.DTOs
{
    public class RegisterDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;

    }
}
