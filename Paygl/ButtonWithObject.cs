using System.Windows.Controls;

namespace Paygl
{
    public class ButtonWithObject: MyButton
    {
        public object Object { get; set; }
        public object Context { get; set; }
    }
}
