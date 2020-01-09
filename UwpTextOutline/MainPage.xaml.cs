using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.Geometry;

namespace UwpTextOutline
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool needsResourceRecreation;
        CanvasTextLayout textLayout;
        CanvasGeometry textGeometry;
        const string String1 = "Lorem fistrum hasta luego Lucas ese hombree por la gloria de mi madre condemor por la gloria de mi madre no puedor.";
        const string String2 = "Te va a hasé pupitaa me cago en tus muelas ese hombree llevame al sircoo hasta luego Lucas fistro ese pedazo de a gramenawer. De la pradera llevame al sircoo tiene musho peligro benemeritaar ese hombree a wan.";

        string content = String1;

        public MainPage()
        {
            this.InitializeComponent();
            btnChangeText.Tapped += BtnChangeText_Tapped;
        }

        private void BtnChangeText_Tapped(object sender, TappedRoutedEventArgs e)
        {
            content = content == String1 ? String2 : String1;

            needsResourceRecreation = true;
            canvas.Invalidate();
        }

        private void Canvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            EnsureResources(sender, sender.Size);

            float strokeWidth = 5.0f;

            args.DrawingSession.DrawGeometry(textGeometry, Colors.Black, strokeWidth);

            var showNonOutlineText = true;
            if (showNonOutlineText)
            {
                Color semitrans = Colors.White;
                args.DrawingSession.DrawTextLayout(textLayout, 0, 0, semitrans);
            }
        }

        private void Canvas_CreateResources(CanvasControl sender, object args)
        {
            needsResourceRecreation = true;
        }

        void EnsureResources(ICanvasResourceCreatorWithDpi resourceCreator, Size targetSize)
        {
            if (!needsResourceRecreation)
                return;

            if (textLayout != null)
            {
                textLayout.Dispose();
                textGeometry.Dispose();
            }

            textLayout = CreateTextLayout(resourceCreator, (float)targetSize.Width, (float)targetSize.Height);
            textGeometry = CanvasGeometry.CreateText(textLayout);

            needsResourceRecreation = false;
        }

        private CanvasTextLayout CreateTextLayout(ICanvasResourceCreator resourceCreator, float canvasWidth, float canvasHeight)
        {
            CanvasTextFormat textFormat = new CanvasTextFormat()
            {
                FontSize = 32f,
                VerticalAlignment = CanvasVerticalAlignment.Bottom,
                Direction = CanvasTextDirection.LeftToRightThenTopToBottom
            };

            CanvasTextLayout textLayout = new CanvasTextLayout(resourceCreator, content, textFormat, canvasWidth, canvasHeight);

            textLayout.TrimmingGranularity = CanvasTextTrimmingGranularity.Character;
            textLayout.TrimmingSign = CanvasTrimmingSign.Ellipsis;

            textLayout.VerticalGlyphOrientation = CanvasVerticalGlyphOrientation.Default;

            return textLayout;
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            needsResourceRecreation = true;
        }

        private void control_Unloaded(object sender, RoutedEventArgs e)
        {
            // Explicitly remove references to allow the Win2D controls to get garbage collected
            canvas.RemoveFromVisualTree();
            canvas = null;
        }
    }
}