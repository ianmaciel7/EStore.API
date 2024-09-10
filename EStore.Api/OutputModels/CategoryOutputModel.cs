using EStore.Api.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.OutputModels
{
    public class CategoryOutputModel
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
