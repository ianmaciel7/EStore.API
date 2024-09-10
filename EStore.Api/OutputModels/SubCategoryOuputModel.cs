using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EStore.Api.Data.Entities;

namespace EStore.Api.OutputModels
{
    public class SubCategoryOuputModel
    {
        [Required]
        public int SubCategoryId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
