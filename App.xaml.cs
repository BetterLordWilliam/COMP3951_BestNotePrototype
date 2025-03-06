namespace BestNote_3951
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF1cXmhKYVJ0WmFZfVtgdVRMYltbQHJPIiBoS35Rc0VgWXpcc3ZSQmRYV0d/");
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage());
        }
    }
}