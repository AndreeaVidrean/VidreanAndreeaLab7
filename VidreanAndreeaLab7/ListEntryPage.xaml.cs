
using System;
using VidreanAndreeaLab7.Data;
using System.IO;
using VidreanAndreeaLab7.Models;

namespace VidreanAndreeaLab7;

public partial class ListEntryPage : ContentPage
{
    public ListEntryPage()
    {
        InitializeComponent(); 
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Asigură-te că `listView` este corect definit în fișierul XAML și este legat de cod
        listView.ItemsSource = await App.Database.GetShopListsAsync();
    }

    // Event handler pentru adăugarea unui nou ShopList
    async void OnShopListAddedClicked(object sender, EventArgs e)
    {
        // Navighează către o pagină nouă pentru adăugarea unui ShopList
        await Navigation.PushAsync(new ListPage
        {
            BindingContext = new ShopList() // Creează un nou obiect ShopList pentru legare
        });
    }

    // Event handler pentru selectarea unui element din listView
    async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            // Navighează către pagina ListPage cu ShopList-ul selectat
            await Navigation.PushAsync(new ListPage
            {
                BindingContext = e.SelectedItem as ShopList
            });
        }
    }
}
