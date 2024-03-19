using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PM2E2Grupo2.Models
{
    public class Sitios
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("latitud")]
        public double latitud { get; set; }

        [JsonPropertyName("longitud")]
        public double longitud { get; set; }

        [JsonPropertyName("desc")]
        public string desc{ get; set; }

        [JsonPropertyName("foto")]
        public string foto { get; set; }



    }
}
