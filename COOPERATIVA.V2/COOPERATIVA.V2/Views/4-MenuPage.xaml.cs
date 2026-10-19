namespace COOPERATIVA.V2.Views;

public partial class MenuPage : ContentPage
{
	public MenuPage()
	{
		InitializeComponent();
	}

    private async void OnSociosClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SociosViewsPage());
    }
    private async void OnAhorrosClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AhorrosViewsPage());
    }
    private async void OnPrestamosClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PrestamosViewsPage());
    }
    private async void OnPagosClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PagosViewsPage());
    }
}