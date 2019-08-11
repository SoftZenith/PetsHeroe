using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PetsHeroe
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {               
        public MainPage()
        {
            InitializeComponent();
            DependencyService.Get<IAndroid>().getCAM_busca();
            _ = DependencyService.Get<IAndroid>().CAM_Busca;
        }
    }
}
