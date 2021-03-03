using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
public class RefreshToken
{
    [Key]
    public string Token {
        get;
        set;
    }
    public string JwtId {
        get;
        set;
    }
    public DateTime CreatedDate {
        get;
        set;
    }
    public DateTime  ExpiryDate {
        get;
        set;
    }
    public bool IsUsed {
        get;
        set;
    }
    public bool IsInvalid {
        get;
        set;
    }
    public string UserId {
        get;
        set;
    }
    [ForeignKey(nameof(UserId))]
    public IdentityUser User {
        get;
        set;
    }

}
}
