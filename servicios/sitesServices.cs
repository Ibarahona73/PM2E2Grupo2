using PM2E2Grupo2.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PM2E2Grupo2.servicios
{
    public class sitesServices : siteService
    {
        private string uriApi = "URL_DE_TU_API";

        public async Task<List<Sitios>> Obtener()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(uriApi);
            var responseBody = await response.Content.ReadAsStringAsync();

            // Deserializar la respuesta en un JsonDocument
            using var doc = JsonDocument.Parse(responseBody);

            // Obtener la raíz del documento
            JsonElement root = doc.RootElement;

            // Crear una lista para almacenar los sitios
            List<Sitios> sitios = new List<Sitios>();

            // Iterar a través de los elementos del array de sitios
            foreach (JsonElement sitioElement in root.EnumerateArray())
            {
                // Obtener los datos de cada sitio
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
                    desc = descr,
                    foto = fotos,
                    audio = audios,
                };

                sitios.Add(sitio);
            }

            return sitios;
        }

        public async Task<bool> AgregarSitio(Sitios sitio)
        {
            try
            {
                var client = new HttpClient();
                var json = JsonSerializer.Serialize(sitio);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uriApi, content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error al agregar el sitio: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ActualizarSitio(int id, Sitios sitio)
        {
            try
            {
                var client = new HttpClient();
                var json = JsonSerializer.Serialize(sitio);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var url = $"{uriApi}/{id}";
                var response = await client.PutAsync(url, content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error al actualizar el sitio: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarSitio(int id)
        {
            try
            {
                var client = new HttpClient();
                var url = $"{uriApi}/{id}";
                var response = await client.DeleteAsync(url);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error al eliminar el sitio: {ex.Message}");
                return false;
            }
        }
    }
}
