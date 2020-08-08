using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.ViewModel
{
    public class GenomViewModel : ViewModelBase
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public string Genom { get; set; }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public GenomViewModel(int column, int row, bool isSelected, string genom)
        {
            Column = column;
            Row = row;
            IsSelected = isSelected;
            Genom = genom;
        }
    }
}
