using NaturalSelection.Model;
using NaturalSelection.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace NaturalSelection.View.Commands
{
    public class SliderMouseWheelCommand : RelayCommand
    {
        public SliderMouseWheelCommand()
         : base(obj =>
         {
             (obj as Slider).MouseWheel -= Obj_MouseWheel;
             (obj as Slider).MouseWheel += Obj_MouseWheel;
         })
        { }

        private static void Obj_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Slider slider = sender as Slider;

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
