using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IdentifyMe.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage2 : TabbedPage, IRootView
    {
        public MainPage2()
        {
            InitializeComponent();
        }
    }
}