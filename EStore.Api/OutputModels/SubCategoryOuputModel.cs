using EStore.Api.ViewModel;
using EStore.Api.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.ViewModel
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
