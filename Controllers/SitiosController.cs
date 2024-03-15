using PM2E2Grupo2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace PM2E2Grupo2.Controllers
{
    

    public class SitiosController
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl2 = "http://localhost:8000/api/";        
        private const string ApiBaseUrl = "http://www.juntadeaguapueblonuevo.org/api/";

        //readonly SQLiteAsyncConnection _connection;

        /* public SitiosController()
         {
             SQLite.SQLiteOpenFlags extensiones = SQLite.SQLiteOpenFlags.ReadWrite |
                                                 SQLite.SQLiteOpenFlags.Create |
                                                 SQLite.SQLiteOpenFlags.SharedCache;

            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, "DBSitios.db3"), extensiones);

             _connection.CreateTableAsync<Sitios>();
         }

         public async Task<int> StoreSitios(Sitios sitios)
         {

             if (sitios.Id == 0)
             {
                 return await _connection.InsertAsync(sitios);
             }
             else
             {
                 return await _connection.UpdateAsync(sitios);
             }
         }

         // Read
         public async Task<List<Models.Sitios>> GetListSitios()
         {

             return await _connection.Table<Sitios>().ToListAsync();
         }

         // Read Element
         public async Task<Models.Sitios> GetSitio(int pid)
         {

             return await _connection.Table<Sitios>().Where(i => i.Id == pid).FirstOrDefaultAsync();
         }

         // Delete Element
         public async Task<int> DeleteSitios(Sitios sitios)
         {

             return await _connection.DeleteAsync(sitios);*/
            public SitiosController()
            {
                _httpClient = new HttpClient();
            }

            public async Task<int> StoreSitios(Sitios sitio)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(sitio);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync(ApiBaseUrl + "SitioEx", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var insertedId = JsonConvert.DeserializeObject<int>(responseContent);
                        return insertedId;
                    }

                    return 0; // Indica que no se pudo insertar
                }
                catch (Exception ex)
                {
                    // Manejar excepciones
                    
                    return 0;
                }
            }

            // Implementa los métodos GetListSitios, GetSitio y DeleteSitios de manera similar a StoreSitios
        }
    }