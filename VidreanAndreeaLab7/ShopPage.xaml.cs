namespace VidreanAndreeaLab7;
using VidreanAndreeaLab7.Models;
using VidreanAndreeaLab7.Data;
using Microsoft.Maui.Devices.Sensors;
using Plugin.LocalNotification;

public partial class ShopPage : ContentPage
{
    public ShopPage()
    {
        InitializeComponent();


    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;

        if (string.IsNullOrWhiteSpace(shop.Adress))
        {
            await DisplayAlert("Eroare", "Adresa magazinului este obligatorie.", "OK");
            return;
        }

        try
        {
            await App.Database.SaveShopAsync(shop);
            await DisplayAlert("Succes", "Magazinul a fost salvat cu succes!", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"A apărut o problemă la salvare: {ex.Message}", "OK");
        }
    }

    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        var address = shop.Adress;

        if (string.IsNullOrWhiteSpace(address))
        {
            await DisplayAlert("Eroare", "Adresa magazinului nu este completată.", "OK");
            return;
        }

        try
        {
            var locations = await Geocoding.GetLocationsAsync(address);

            if (locations == null || !locations.Any())
            {
                await DisplayAlert("Eroare", "Nu am putut găsi locația pentru adresa specificată.", "OK");
                return;
            }

            var shoplocation = locations.FirstOrDefault();

            var myLocation = await Geolocation.GetLocationAsync();

            if (myLocation == null)
            {
                await DisplayAlert("Eroare", "Nu am putut obține locația curentă. Verifică setările GPS.", "OK");
                return;
            }

            var distance = myLocation.CalculateDistance(shoplocation, DistanceUnits.Kilometers);

            if (distance < 5)
            {
                var request = new NotificationRequest
                {
                    Title = "Ai de făcut cumpărături în apropiere!",
                    Description = address,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(1)
                    }
                };
                LocalNotificationCenter.Current.Show(request);
            }

            var options = new MapLaunchOptions
            {
                Name = "Magazinul meu preferat"
            };

            await Map.OpenAsync(shoplocation, options);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"A apărut o problemă: {ex.Message}", "OK");
        }
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        // Verificăm dacă un magazin este selectat
        if (listView.SelectedItem is Shop selectedShop)
        {
            // Confirmarea ștergerii de la utilizator
            bool confirm = await DisplayAlert("Confirmare",
                                              $"Ești sigur că vrei să ștergi magazinul '{selectedShop.ShopName}'?",
                                              "Da", "Nu");
            if (confirm)
            {
                // Ștergem magazinul din baza de date
                await App.Database.DeleteShopAsync(selectedShop);

                // Reîmprospătăm lista de magazine
                listView.ItemsSource = await App.Database.GetShopsAsync();

                await DisplayAlert("Succes", "Magazinul a fost șters.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Eroare", "Te rog să selectezi un magazin pentru ștergere.", "OK");
        }
    }

    // Metodă pentru încărcarea datelor în pagină
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Preluăm lista de magazine din baza de date și o setăm ca sursă pentru ListView
        listView.ItemsSource = await App.Database.GetShopsAsync();
    }
}


