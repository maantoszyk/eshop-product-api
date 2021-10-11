using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Product.Core.Dto
{
    public class ProductPatchDto
    {
        [StringLength(4000)]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}
