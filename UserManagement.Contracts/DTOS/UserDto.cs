using System.ComponentModel.DataAnnotations;

namespace UserManagement.Contracts.DTOS;

public record class UserDto(
     long Id ,
     [Required] string  Forename ,
     [Required]string Surname,
     DateOnly DateOfBirth ,
     [Required][EmailAddress]
     string Email ,
     bool IsActive 
);

