using VidreanAndreeaLab7.Models;
using VidreanAndreeaLab7.Data;

namespace VidreanAndreeaLab7;

public partial class ProductPage : ContentPage
{
    private readonly ShopList sl;

    public ProductPage(ShopList slist)
    {
        InitializeComponent();
        sl = slist;

        // Setează BindingContext-ul pentru această pagină
        BindingContext = new Product(); // Sau direct un ViewModel dacă ai unul
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        listView.ItemsSource = await App.Database.GetProductsAsync();
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var product = (Product)BindingContext;
        await App.Database.SaveProductAsync(product);

        // Reîmprospătăm sursa de date
        listView.ItemsSource = await App.Database.GetProductsAsync();

        // Resetăm BindingContext pentru un produs nou
        BindingContext = new Product();
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        if (listView.SelectedItem is Product selectedProduct)
        {
            await App.Database.DeleteProductAsync(selectedProduct);
            listView.ItemsSource = await App.Database.GetProductsAsync();
        }
        else
        {
            await DisplayAlert("Error", "Please select a product to delete.", "OK");
        }
    }

    async void OnAddButtonClicked(object sender, EventArgs e)
    {
        if (listView.SelectedItem is Product selectedProduct)
        {
            var listProduct = new ListProduct
            {
                ShopListID = sl.ID,
                ProductID = selectedProduct.ID
            };
            await App.Database.SaveListProductAsync(listProduct);
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "Please select a product to add to the shop list.", "OK");
        }
    }
}
