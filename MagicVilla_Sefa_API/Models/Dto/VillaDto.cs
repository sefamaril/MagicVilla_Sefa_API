using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Sefa_API.Models.Dto
{
    public class VillaDto
    {
        public int Id { get; set; }
        [Required]//Backend tarafında bir prop'un boş geçilmemesini istiyorsak bunu kullanmalıyız.
        [MaxLength(30)]
        public string Name { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }
    }
}
