using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace PM2E2Grupo2.Views;

public partial class Mapa : ContentPage 
{

    double latitud;
    double longitud;
    string descripcion;
    bool mostrarUbicacionUsuario;


    public Mapa(double latitud, double longitud, string descripcion , bool mostrarUbicacionUsuario)
    {
		InitializeComponent();

        this.latitud = latitud;
        this.longitud = longitud;
        this.descripcion = descripcion;
        this.mostrarUbicacionUsuario = mostrarUbicacionUsuario;

        AddPinToMap();

        if (mostrarUbicacionUsuario)
        {
            hola.IsShowingUser = true;
        }
    }
    private void AddPinToMap()
    {
        Pin pin = new Pin
        {
            Label = descripcion,
            Location = new Location(latitud, longitud)
        };


        hola.Pins.Add(pin);
        hola.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Location, Distance.FromMiles(1)));
    }

    private async void btnCompartir_Clicked(object sender, EventArgs e)
    {
        string url = $"https://www.google.com/maps/search/?api=1&query={latitud},{longitud}&query_place_id={descripcion}";

        try
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = "¡Checa Este Sitio Turistico!",
                Uri = url
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo compartir la ubicación: {ex.Message}", "OK");
        }
    }
}