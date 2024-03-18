using PM2E2Grupo2.Models;


namespace PM2E2Grupo2.servicios
{
public interface siteService
{
	public Task<List<Sitios>> Obtener();
    public Task<bool> AgregarSitio(Sitios sitio);
    public Task<bool> ActualizarSitio(int id, Sitios sitio);
    public Task<bool> EliminarSitio(int id);

    }
}