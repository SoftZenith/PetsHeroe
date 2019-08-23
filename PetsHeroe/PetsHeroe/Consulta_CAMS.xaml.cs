using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PetsHeroe
{
    public partial class Consulta_CAMS : ContentPage
    {
        public Consulta_CAMS()
        {
            InitializeComponent();
            CAM lista = new CAM();
            lsvCAMS.ItemsSource = lista.getCAMS();
            mapCAMS.MoveToRegion(new MapSpan(new Position(21.512188, -104.809230), 0.5, 0.5));
        }
    }
}