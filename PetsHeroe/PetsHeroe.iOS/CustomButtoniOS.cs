using System;
using PetsHeroe;
using PetsHeroe.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtoniOS))]
namespace PetsHeroe.iOS
{
    public class CustomButtoniOS : ButtonRenderer
    {
        public CustomButtoniOS()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
                Control.TitleLabel.Lines = 0;
                //Control.TitleLabel.TextAlignment = UITextAlignment.Center;
                Control.TitleLabel.TextAlignment = UITextAlignment.Natural;
                //Control.ImageEdgeInsets = new UIEdgeInsets(0, (System.nfloat)65.0, (System.nfloat)40.0, 0);
                float ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height / 12; //35.0 //667
                float ScreenWidth = (int) UIScreen.MainScreen.Bounds.Width / 9; //375, 414
                if ((int)UIScreen.MainScreen.Bounds.Width > 370 && (int)UIScreen.MainScreen.Bounds.Width < 413) {
                    ScreenWidth += 10;
                }
                if ((int)UIScreen.MainScreen.Bounds.Width > 413)
                {
                    ScreenWidth += 15;
                }
                Control.ImageEdgeInsets = new UIEdgeInsets(0, (System.nfloat)ScreenWidth, (System.nfloat)40.0, 0);
            }
        }
    }
}
