using System.ComponentModel.DataAnnotations;

namespace UserManagement.Contracts.DTOS;

public record class UserDto(
     long Id ,

     [Required(ErrorMessage = "Forename is required")] string  Forename ,

     [Required(ErrorMessage = "Surname is required")]string Surname,

     [Required(ErrorMessage = "Date Of Birth is required")]DateOnly DateOfBirth ,

     [Required(ErrorMessage = "Email is required")]
     [EmailAddress(ErrorMessage = "Enter Valid Email")] string Email ,

     [Required(ErrorMessage = "Forename is required")]bool IsActive
      
){
    public UserDto() : this(
        0,               // Id
        string.Empty,    // Forename
        string.Empty,    // Surname
        DateOnly.MinValue, // DateOfBirth
        string.Empty,    // Email
        true        // IsActive default
    ) { }
};

