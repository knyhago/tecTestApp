using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required(ErrorMessage = "Forename is required")]
    [MaxLength(25, ErrorMessage = "Forename cannot exceed 25 characters")]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Forename can only contain letters")]
    public string Forename { get; set; } = string.Empty;

    [Required(ErrorMessage = "Surname is required")]
    [MaxLength(25, ErrorMessage = "Surname cannot exceed 25 characters")]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Surname can only contain letters")]
    public string Surname { get; set; } = string.Empty;


    [Required(ErrorMessage = "Date of Birth is required")]

    public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.Today);


    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email format")]
    public string Email { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
