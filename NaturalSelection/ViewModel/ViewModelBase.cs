using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanging
    {
        public event PropertyChangingEventHandler PropertyChanging;

        protected void RaisePropertyChanged(string propertyName)
        {
            var e = PropertyChanging;
            if (e != null)
            {
                e(this, new PropertyChangingEventArgs(propertyName));
            }
        }
    }
}
