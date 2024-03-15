namespace PM2E2Grupo2.Views;
using PM2E2Grupo2.Controllers;

public partial class Inicio : ContentPage

{
    SitiosController sitiosController = new SitiosController();
    string audioFilePath;
    FileResult photo;
    public Inicio()
    {
        InitializeComponent();
        
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Verificar y solicitar permisos de ubicación
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Error", "Los permisos de ubicación no están habilitados.", "OK");
                return;
            }
        }

        // Obtener la ubicación actual
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
                await DisplayAlert("Error", "No se pudo obtener la ubicación actual.", "OK");
            }
        }
        catch (FeatureNotSupportedException)
        {
            await DisplayAlert("Error", "La geolocalización no es compatible con este dispositivo.", "OK");
        }
        catch (PermissionException)
        {
            await DisplayAlert("Error", "Los permisos de ubicación no están habilitados.", "OK");
        }
        catch (Exception)
        {
            await DisplayAlert("Error", "Se produjo un error al obtener la ubicación.", "OK");
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

        /* // Verificar si se ha tomado la foto y si todos los campos obligatorios están completos
         if (photo == null ||
             string.IsNullOrWhiteSpace(Latitud.Text) ||
             string.IsNullOrWhiteSpace(Longitud.Text)) 
             //string.IsNullOrWhiteSpace(Descripcion.Text))
         {
             // Mostrar alerta si algún campo obligatorio está vacío o si no se ha tomado la foto
             await DisplayAlert("Error", "Por favor, complete todos los campos y tome una foto.", "Aceptar");
             return;
         }



         if (double.TryParse(Latitud.Text, out double latitud) && double.TryParse(Longitud.Text, out double longitud))
         {
             var lugar = new Models.Sitios
             {
                 Latitud = latitud,
                 Longitud = longitud,
                 Desc = Descripcion.Text,
                 foto = GetImage64()
             };

             if (await App.Database.StoreSitios(lugar) > 0)
             {
                 await DisplayAlert("Aviso", "Registro ingresado con éxito!!", "OK");
             }
         }
         else
         {
             // Manejo de error si la entrada de Latitud o Longitud no es un número válido
             await DisplayAlert("Error", "La Latitud y la Longitud deben ser valores numéricos.", "OK");
         } */

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
                foto = GetImage64()
            };

            if (await sitiosController.StoreSitios(lugar) > 0)
            {
                await DisplayAlert("Aviso", "Registro ingresado con éxito!!", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "La Latitud y la Longitud deben ser valores numéricos.", "OK");
        }
    }

    private async void btnSitios_Clicked(object sender, EventArgs e)
    {
        var Sitios = new MapaLista();
        await Navigation.PushAsync(Sitios);

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
