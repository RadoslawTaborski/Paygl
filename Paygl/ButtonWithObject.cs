using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Paygl
{
    public class ButtonWithObject: Button
    {
        public object Object { get; set; }
        public object Context { get; set; }
    }
}
