using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.DTOS;

public record class AddUserDTO(
     long Id ,
     [Required] string  Forename ,
     [Required]string Surname,
     DateOnly DateOfBirth ,
     [Required][EmailAddress]
     string Email ,
     bool IsActive 
);
