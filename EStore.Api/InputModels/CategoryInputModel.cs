using EStore.Api.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.InputModels
{
    public class CategoryInputModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
