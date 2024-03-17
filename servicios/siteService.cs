using PM2E2Grupo2.Models;


namespace PM2E2Grupo2.servicios
{
public interface siteService
{
	public Task<List<Sitios>> Obtener();

}
}