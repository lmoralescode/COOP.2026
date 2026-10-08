namespace COOPERATIVA.V2.Views;

public partial class InicioViewsPage : ContentPage
{
	public InicioViewsPage()
	{
        InitializeComponent();
	}

    private async void IniciarSesion_Clicked(object sender, EventArgs e)
    {
       await Navigation.PushAsync(new LoginViewsPage());
    }
    private async void CrearNav_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SingUpViewsPage());
    }

}