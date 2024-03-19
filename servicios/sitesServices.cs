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
        private string uriApi = "http://18.118.217.193:8000/api/SitioEx";

        public async Task<List<Sitios>> Obtener()
         {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync(uriApi);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error al obtener datos: {response.StatusCode}");
                    return null;
                }

                var responseBody = await response.Content.ReadAsStringAsync();

                // Deserializar la respuesta en una lista de sitios
                var sitios = JsonSerializer.Deserialize<List<Sitios>>(responseBody);

                return sitios;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener datos: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error al deserializar JSON: {ex.Message}");
                return null;
            }
        }


        public async Task<bool> AgregarSitio(Sitios sitio)
        {

            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(300);
                    var json = JsonSerializer.Serialize(sitio);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(uriApi, content);

                    return response.IsSuccessStatusCode;
                }
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
                using (var client = new HttpClient())
                {
                    var json = JsonSerializer.Serialize(sitio);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var url = $"{uriApi}/{id}";
                    var response = await client.PutAsync(url, content);

                    return response.IsSuccessStatusCode;
                }
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
                using (var client = new HttpClient())
                {
                    var url = $"{uriApi}/{id}";
                    var response = await client.DeleteAsync(url);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el sitio: {ex.Message}");
                return false;
            }
        }
    }
}
