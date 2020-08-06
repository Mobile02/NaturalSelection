using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace NaturalSelection.ViewModel.Behaviors
{
    public class MouseWheelBehavior : Behavior<Slider>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseWheel += MouseWheel;
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.MouseWheel -= MouseWheel;
            base.OnDetaching();
        }

        private void MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var slider = (Slider)sender;
            int value = (int)slider.Value;

            if (e.Delta < 0)
            {
                if (++value > slider.Maximum)
                    value = (int)slider.Maximum;
            }
            else
            {
                if (--value <= slider.Minimum)
                    value = (int)slider.Minimum;
            }

            slider.Value = value;
        }
    }
}
