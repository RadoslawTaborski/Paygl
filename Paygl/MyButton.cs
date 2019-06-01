using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Paygl
{
    public class MyButton : Button
    {
        private bool _isClickEventAttached;

        protected override void OnClick()
        {
            if (!_isClickEventAttached)
            {
                this.Click += MyButton_Click;
                _isClickEventAttached = true;
            }

            base.OnClick();
        }

        private async void MyButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                button.IsEnabled = false;
                await Task.Delay(200); 
                button.IsEnabled = true;
            }
        }
    }
}
