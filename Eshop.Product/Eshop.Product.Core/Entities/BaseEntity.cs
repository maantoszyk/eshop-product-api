using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Product.Core.Entities
{
    public class BaseEntity : IEntity
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
    }
}
