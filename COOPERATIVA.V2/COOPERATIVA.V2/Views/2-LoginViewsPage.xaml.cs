namespace COOPERATIVA.V2.Views
{
    public partial class LoginViewsPage : ContentPage
    {
        public LoginViewsPage()
        {
            InitializeComponent();
        }

        private async void LoginNav_Clicked(object sender, EventArgs e)
        {
            string usuario = TxtEmail.Text;
            string clave = TxtClave.Text;

            if (usuario != "lmorales@gmail.com" || clave != "1234")
            {

                await DisplayAlert("Error", "Por favor, ingrese usuario y contraseña.", "OK");
            }
            else
            {
                await Navigation.PushAsync(new MenuViewsPage());

            }
        }
    }
}
