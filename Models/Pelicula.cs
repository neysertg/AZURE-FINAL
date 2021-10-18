using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AZURE_FINAL.Models
{
    public class Pelicula
    {
        [JsonProperty(PropertyName = "id")]
          [Display(Name="ID")]
        public String Id { get; set; }

         [Display(Name="NOMBRE")]
        public String Categoria { get; set; }
         [Display(Name="APE_PATERNO")]
        public String Titulo { get; set; }
         [Display(Name="APE_MATERNO")]
        public String Director { get; set; }
         [Display(Name="CURSO")]
        public String Estreno { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
