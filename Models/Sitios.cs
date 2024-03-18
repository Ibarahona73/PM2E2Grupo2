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
        [JsonPropertyName("Id")]
        public int id { get; set; }

        [JsonPropertyName("Latitud")]
        public double latitud { get; set; }

        [JsonPropertyName("Longitud")]
        public double longitud { get; set; }

        [JsonPropertyName("Desc")]
        public string desc{ get; set; }

        [JsonPropertyName("Foto")]
        public string foto { get; set; }

        [JsonPropertyName("Audio")]
        public string audio { get; set; }

    }
}
