﻿using System.ComponentModel.DataAnnotations;

public class RegisterModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
    [Required]
    public string Name { get; set; }
}