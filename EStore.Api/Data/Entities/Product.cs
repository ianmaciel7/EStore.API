using EStore.Api.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EStore.Api.Data
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}