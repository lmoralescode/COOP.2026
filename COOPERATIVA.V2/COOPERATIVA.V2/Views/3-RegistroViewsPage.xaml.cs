namespace COOPERATIVA.V2.Views;

public partial class RegistroViewsPage : ContentPage
{
	public RegistroViewsPage()
	{
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        NombreEntry.Text = string.Empty;
        ApellidosEntry.Text = string.Empty;
        CedulaEntry.Text = string.Empty;
        TelefonoEntry.Text = string.Empty;
        DireccionEntry.Text = string.Empty;
        FechaNacimientoPicker.Date = DateTime.Now;
        CorreoEntry.Text = string.Empty;
        ContrasenaEntry.Text = string.Empty;

    }

}