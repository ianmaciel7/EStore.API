using System.ComponentModel.DataAnnotations;

namespace EStore.Api.InputModels
{
    public class SubCategoryInputModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
