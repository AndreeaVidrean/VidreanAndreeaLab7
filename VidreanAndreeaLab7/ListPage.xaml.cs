using VidreanAndreeaLab7.Models;
using VidreanAndreeaLab7.Data;
using System;
using System.IO;


namespace VidreanAndreeaLab7;



public partial class ListPage : ContentPage
{
    public Product SelectedProduct { get; set; }
    public ListPage()
    {
        InitializeComponent();
    }
    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        if (listView.SelectedItem is Product selectedProduct)
        {
            // Șterge produsul selectat
            await App.Database.DeleteProductAsync(selectedProduct);

            // Actualizează lista de produse din UI
            var shopList = (ShopList)BindingContext;
            listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);

            await DisplayAlert("Succes", "Produsul a fost șters.", "OK");
        }
        else
        {
            await DisplayAlert("Eroare", "Te rog să selectezi un produs pentru ștergere.", "OK");
        }
    }


    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        Shop selectedShop = (ShopPicker.SelectedItem as Shop);
        slist.ID = selectedShop.ID;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;

        if (ShopPicker.SelectedItem is not Shop selectedShop)
        {
            await DisplayAlert("Eroare", "Te rog să selectezi un magazin.", "OK");
            return;
        }

        slist.ID = selectedShop.ID;

        try
        {
            await App.Database.DeleteShopListAsync(slist);
            await DisplayAlert("Succes", "Lista a fost ștearsă.", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", $"A apărut o problemă: {ex.Message}", "OK");
        }
    }

    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)this.BindingContext)
        {
            BindingContext = new Product()
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var items = await App.Database.GetShopsAsync();
        ShopPicker.ItemsSource = (System.Collections.IList)items;
        ShopPicker.ItemDisplayBinding = new Binding("ShopDetails");

        var shopl = (ShopList)BindingContext;
        listView.ItemsSource = await
       App.Database.GetListProductsAsync(shopl.ID);
    }
}