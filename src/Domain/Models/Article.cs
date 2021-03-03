using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
[Table("Articles")]
public class Article : IEntity<int>
{
    [Key]
    public int Id {
        get;
        set;
    }
    public DateTime CreatedDate {
        get;
        set;
    }
    public DateTime? UpdatedDate {
        get;
        set;
    }
    [Required]
    [MaxLength(2000)]
    public string Title {
        get;
        set;
    }
    [Required]
    public string Body {
        get;
        set;
    }
    [ForeignKey("Author")]
    public string AuthorId {
        get;
        set;
    }
    public virtual IdentityUser Author {
        get;
        set;
    }
}
}
