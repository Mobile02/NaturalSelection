using NaturalSelection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace NaturalSelection.ViewModel.Commands
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

            if (e.Delta < 0)
            {
                if (++slider.Value > new Constants().MaxSpeed)
                    slider.Value = new Constants().MaxSpeed;
            }
            else
            {
                if (--slider.Value < 0)
                    slider.Value = 0;
            }
        }
    }
}
