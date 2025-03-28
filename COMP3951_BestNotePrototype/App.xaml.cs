using Markdig;

namespace BestNote_3951
{
    public partial class App : Application
    {
        public static MarkdownPipeline Pipeline { get; }
        static App()
        {
            Pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseAutoIdentifiers()
                .Build();
        }
        public App()
        {
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF1cXmhKYVJ0WmFZfVtgdVRMYltbQHJPIiBoS35Rc0VgWXpcc3ZSQmRYV0d/");
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXxeeXVRRGZYUEJ/W0U=");
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage());
        }
    }
}