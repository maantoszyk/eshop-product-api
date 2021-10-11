using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Product.Core.Dto
{
    [Serializable]
    public class ProductDto
    {
        /// <summary>
        /// Name of the product
        /// </summary>
        [Required]
        [StringLength(250)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Image URI of the product
        /// </summary>
        [Required]
        [StringLength(500)]
        [JsonProperty(PropertyName = "imgUri")]
        public string ImgUri { get; set; }

        /// <summary>
        /// Price of the product
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Descripiton of the product
        /// </summary>
        [StringLength(4000)]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}
