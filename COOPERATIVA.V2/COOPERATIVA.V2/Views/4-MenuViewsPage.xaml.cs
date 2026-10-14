using COOPERATIVA.V2.Views;

namespace COOPERATIVA.V2;

public partial class MenuViewsPage : ContentPage
{
	public MenuViewsPage()
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