namespace COOPERATIVA.V2.Views;

public partial class InicioViewsPage : ContentPage
{

    public InicioViewsPage()
    {
        InitializeComponent();
    }

    private async void OnComenzarClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginViewsPage());
    }

    private async void OnRegistroClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegistroViewsPage());

    }
    //private async void OnSalirClicked(object sender, EventArgs e)
    //{
    //    bool salir = await DisplayAlertAsync(
    //        "Salir",
    //        "¿Desea cerrar la aplicación?",
    //        "Sí",
    //        "No");

    //    if (salir)
    //    {
    //        Application.Current?.Quit();
    //    }
    //}
}