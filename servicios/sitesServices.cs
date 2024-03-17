using PM2E2Grupo2.Models;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PM2E2Grupo2.servicios
{
	public class sitesServices : siteService
	{
		private string uriApi = "";

		public async Task<List<Sitios>> Obtener()
		{
			var client = new HttpClient();
			var response = await client.GetAsync(uriApi);
			var responseBody = await response.Content.ReadAsStringAsync();

			// Deserializar la respuesta en un JsonDocument
			using var doc = JsonDocument.Parse(responseBody);

			// Obtener la raíz del documento
			JsonElement root = doc.RootElement;

			// Crear una lista para almacenar los países
			List<Sitios> sitios = new List<Sitios>();

			// Iterar a través de los elementos del array de países
			  foreach (JsonElement sitioElement in root.EnumerateArray())
            {
                // Obtener la latitud y longitud de cada sitio
                string latitudeStr = sitioElement.GetProperty("latitud").GetString();
                string longitudeStr = sitioElement.GetProperty("longitud").GetString();
                string descr = sitioElement.GetProperty("descripcion").GetString();
                string fotos = sitioElement.GetProperty("foto").GetString();
                string audios = sitioElement.GetProperty("audio").GetString();
                // Convertir los valores de string a double
                double latitude, longitude;
                if (!double.TryParse(latitudeStr, out latitude) ||
                    !double.TryParse(longitudeStr, out longitude))
                {
                    
                    latitude = 0;
                    longitude = 0;
                }

                // Crear un objeto Sitios y agregarlo a la lista
                var sitio = new Sitios
                {
                    latitud = latitude,
                    longitud = longitude,
                    desc=descr,
                    foto=fotos,
                    audio=audios,
                };

                sitios.Add(sitio);
            }

			return sitios;
		}
	}
}
