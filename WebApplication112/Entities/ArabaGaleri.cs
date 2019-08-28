using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication112.Entities
{
    public class ArabaGaleri
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        public int Fiyat { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        public string Aciklama { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        public string Model { get; set; }
        public string Resim { get; set; }

    }
}
