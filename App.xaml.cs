namespace Run
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Login());
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);

            if (isLoggedIn)
            {
                // User is already logged in
                MainPage = new NavigationPage(new Menu());
            }
            else
            {
                MainPage = new NavigationPage(new Login());
            }
            //   101988493212
        }
    }
}
// AIzaSyAcelOj2e9qKfljoS1hCIrYIY8cEsfiDSA
// https://run2303app-default-rtdb.firebaseio.com/:null