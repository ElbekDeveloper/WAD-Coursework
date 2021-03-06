﻿using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Resources
{
    public class ArticleResource
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [Required]
        public string Body { get; set; }
        public AuthorResource Author { get; set; }

    }
}
