namespace COOPERATIVA.V2.Views
{
    public partial class LoginViewsPage : ContentPage
    {
        public LoginViewsPage()
        {
            InitializeComponent();
        }

        private async void OnIngresarClicked(object sender, EventArgs e)
        {
            string usuario = UsuarioEntry.Text;
            string clave = ContrasenaEntry.Text;

            if (usuario != "lmorales@gmail.com" || clave != "1234")
            {

                await DisplayAlertAsync("Error", "Por favor, ingrese usuario y contraseña.", "OK");
            }
            else
            {
                await Navigation.PushAsync(new MenuViewsPage());

            }
        }

        private async void OnRegistroClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistroViewsPage());
        }
    }
}
