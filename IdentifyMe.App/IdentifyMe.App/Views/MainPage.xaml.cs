using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IdentifyMe.App.ViewModels;

namespace IdentifyMe.App.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage, IRootView
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void CurrentPageChanged(object sender, System.EventArgs e) => Title = GetPageName(CurrentPage);

        private void Appearing(object sender, System.EventArgs e) => Title = GetPageName(CurrentPage);

        private string GetPageName(Page page)
        {
            if (page.BindingContext is ABaseViewModel vmBase)
                return vmBase.Name;
            return null;
        }

    }
}