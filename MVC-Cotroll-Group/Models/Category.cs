using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MVC_Cotroll_Group.Models
{
    public class Category
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "ім`я обов`язкове")]
        [MaxLength(100, ErrorMessage = "Максимальна довжина 100 символів")]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
