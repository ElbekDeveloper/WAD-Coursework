using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("Tags")]
    public class Tag : IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Article> Articles { get; set; }
    }
}
