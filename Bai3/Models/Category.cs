using System.ComponentModel.DataAnnotations;

namespace Bai3.Models
{
    public class Category
    {
        public byte Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }    
    }
}