using System;
using Newtonsoft.Json;

namespace refactor_me.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DeliveryPrice { get; set; }

        [JsonIgnore]
        public bool IsNew { get; set; }
    }
}