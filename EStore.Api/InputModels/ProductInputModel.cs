﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.InputModel
{
    public class ProductInputModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }       
    }
}
