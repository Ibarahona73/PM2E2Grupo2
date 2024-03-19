namespace PM2E2Grupo2.Views;
using PM2E2Grupo2.servicios;

public partial class Inicio : ContentPage

{
    
    FileResult photo;
    public Inicio()
    {
        InitializeComponent();
        
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Verificar y solicitar permisos de ubicaci�n
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Error", "Los permisos de ubicaci�n no est�n habilitados.", "OK");
                return;
            }
        }

        // Obtener la ubicaci�n actual
        try
        {
            var location = await Geolocation.GetLocationAsync();
            if (location != null)
            {
                Latitud.Text = location.Latitude.ToString();
                Longitud.Text = location.Longitude.ToString();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo obtener la ubicaci�n actual.", "OK");
            }
        }
        catch (FeatureNotSupportedException)
        {
            await DisplayAlert("Error", "La geolocalizaci�n no es compatible con este dispositivo.", "OK");
        }
        catch (PermissionException)
        {
            await DisplayAlert("Error", "Los permisos de ubicaci�n no est�n habilitados.", "OK");
        }
        catch (Exception)
        {
            await DisplayAlert("Error", "Se produjo un error al obtener la ubicaci�n.", "OK");
        }
    }

    public String GetImage64()
    {
        if (photo != null)
        {
            using (MemoryStream ms = new MemoryStream())
            {

                Stream stream = photo.OpenReadAsync().Result;
                stream.CopyTo(ms);
                byte[] data = ms.ToArray();

                String Base64 = Convert.ToBase64String(data);

                return Base64;
            }
        }
        return null;
    }

    public byte[] GetImageArray()
    {
        if (photo == null)
        {
            using (MemoryStream ms = new MemoryStream())
            {

                Stream stream = photo.OpenReadAsync().Result;
                stream.CopyTo(ms);
                byte[] data = ms.ToArray();
                return data;
            }
        }
        return null;
    }


    private async void btnAgregar_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (photo == null ||
                string.IsNullOrWhiteSpace(Latitud.Text) ||
                string.IsNullOrWhiteSpace(Longitud.Text))
            {
                await DisplayAlert("Error", "Por favor, complete todos los campos y tome una foto.", "Aceptar");
                return;
            }

            if (double.TryParse(Latitud.Text, out double lat) && double.TryParse(Longitud.Text, out double Long))
            {
                var lugar = new Models.Sitios
                {
                    
                    latitud = lat,
                    longitud = Long,
                    desc = Descripcion.Text,
                    foto = GetImage64(),
                    audio = null // No se pudo jacer 
                };

                // Instancio para llamar a la Api
                var sitiosService = new sitesServices();

                // Llamar al m�todo para agregar el sitio
                if (await sitiosService.AgregarSitio(lugar))
                {
                    await DisplayAlert("Aviso", "Registro ingresado con �xito!!", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Error al agregar el sitio.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "La Latitud y la Longitud deben ser valores num�ricos.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Se produjo un error al agregar el sitio: {ex.Message}", "OK");
        }
    }

    private void btnSitios_Clicked(object sender, EventArgs e)
    {
        var Sitios = new MapaLista();
        Navigation.PushAsync(Sitios);





    }

    private async void btnSalir_Clicked(object sender, EventArgs e)
    {
        Application.Current.Quit();
    }

    private async void btnFoto_Clicked(object sender, EventArgs e)
    {
        photo = await MediaPicker.CapturePhotoAsync();

        if (photo != null)
        {
            string path = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using Stream sourcephoto = await photo.OpenReadAsync();
            using FileStream Streamlocal = File.OpenWrite(path);


            foto.Source = ImageSource.FromStream(() => photo.OpenReadAsync().Result);

            await sourcephoto.CopyToAsync(Streamlocal);
        }
    }      
}
