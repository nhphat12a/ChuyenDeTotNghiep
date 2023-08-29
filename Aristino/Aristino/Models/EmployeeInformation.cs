using System;
using System.Collections.Generic;

namespace Aristino.Models;

public partial class EmployeeInformation
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Dob { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? EmployeeAddress { get; set; }

    public string? Avatar { get; set; }
}
