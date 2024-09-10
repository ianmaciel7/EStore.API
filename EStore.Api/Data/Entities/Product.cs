﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EStore.Api.Data.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}