using Microsoft.Maui.Controls.Maps;
using PM2E2Grupo2.servicios;
using Microsoft.Maui.Maps;
using PM2E2Grupo2.Models;
using static PM2E2Grupo2.Views.Mapa;

namespace PM2E2Grupo2.Views;


public partial class MapaLista : ContentPage
{

    private readonly sitesServices _sitesService;

    public MapaLista()
	{
		InitializeComponent();
        _sitesService = new sitesServices();

    }

    private async void ubicaciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            if (e.CurrentSelection.FirstOrDefault() is Sitios selectedLocation)
            {
                bool goToLocation = await DisplayAlert("Confirmación", $"¿Desea ir a la localización {selectedLocation.desc}?", "Sí", "No");

                if (goToLocation)
                {
                    await Navigation.PushAsync(new Mapa(selectedLocation.latitud, selectedLocation.longitud, selectedLocation.desc, true)); // True para mostrar la ubicación del usuario
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción aquí
            Console.WriteLine($"Error en ubicaciones_SelectionChanged: {ex.Message}");
        }
    }

    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();
            var sitios = await _sitesService.Obtener();
            if (sitios != null)
            {
                ubicaciones.ItemsSource = sitios;
            }
            else
            {
                // Manejar el caso donde el servicio no devuelva datos
                Console.WriteLine("El servicio no devolvió datos");
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción aquí
            Console.WriteLine($"Error en OnAppearing: {ex.Message}");
        }
    }

    private async void btnBorrar_Clicked(object sender, EventArgs e)
    {
       
    }



    private async void btnActua_Clicked(object sender, EventArgs e)
    {
        
    }

}

