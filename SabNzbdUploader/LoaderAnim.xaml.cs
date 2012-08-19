using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace SabNzbdUploader
{
	/// <summary>
	/// Interaction logic for LoaderAnim.xaml
	/// </summary>
	public partial class LoaderAnim : UserControl
	{
		public LoaderAnim()
		{
			this.InitializeComponent();
		}

        private void Animate()
        {
            var anim = new DoubleAnimation
            {
                From = 0,
                To = 360,
                By = 30,
                Duration = TimeSpan.FromSeconds(5),
                RepeatBehavior = RepeatBehavior.Forever
            };

            var transform = new RotateTransform();
            loader.RenderTransform = transform;
            transform.BeginAnimation(RotateTransform.AngleProperty, anim);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Animate();
        }

    }

}