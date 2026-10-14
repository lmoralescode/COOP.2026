namespace COOPERATIVA.V2.ViewsModel;

public class _2_LoginViewsModel : ContentPage
{
	public _2_LoginViewsModel()
	{
		Content = new VerticalStackLayout
		{
			Children = {
				new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Welcome to .NET MAUI!"
				}
			}
		};
	}
}