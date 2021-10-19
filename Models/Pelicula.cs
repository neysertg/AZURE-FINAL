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

        public String Nombre { get; set; }
         [Display(Name="APE_PATERNO")]
        public String Ape_paterno { get; set; }
         [Display(Name="APE_MATERNO")]
        public String Ape_materno { get; set; }
         [Display(Name="CURSO")]
        public String Curso { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
