using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MVC_Cotroll_Group.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
