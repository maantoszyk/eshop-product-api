using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Product.Core.Entities
{
    [Table("Product")]
    public class ProductEntity : BaseEntity
    {
        [Column("Name")]
        [StringLength(250)]
        public string Name { get; set; }

        [Column("ImgUri")]
        [StringLength(500)]
        public string ImgUri { get; set; }

        [Column("Price", TypeName = "decimal(12, 3)")]
        public decimal Price { get; set; }

        [Column("Description")]
        [StringLength(4000)]
        public string Description { get; set; }
    }
}
