using NaturalSelection.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelection.ViewModel
{
    public class BrainViewModel : ViewModelBase
    {
        private ObservableCollection<GenomViewModel> genomViewModels;
        private ViewModelBio selectedBio;
        private Constants constants;
        private int prevIndexBrain = 0;
        private int countColumns;
        private int countRows;

        public ObservableCollection<GenomViewModel> GenomViewModels
        {
            get { return genomViewModels; }
            set
            {
                genomViewModels = value;
                RaisePropertyChanged("GenomViewModels");
            }
        }
        public int CountColumns
        {
            get { return countColumns; }
            set
            {
                countColumns = value;
                RaisePropertyChanged("CountColumns");
            }
        }
        public int CountRows
        {
            get { return countRows; }
            set
            {
                countRows = value;
                RaisePropertyChanged("CountRows");
            }
        }

        public BrainViewModel()
        {
            constants = new Constants();
            CountColumns = 8;
            CountRows = constants.SizeBrain / 8;
        }

        public void SetSelectedBio(ViewModelBio viewModelBio)
        {
            selectedBio = viewModelBio;
            GenomViewModels = new ObservableCollection<GenomViewModel>();

            if (viewModelBio == null)
                return;

            for (int i = 0; i < constants.SizeBrain; i++)
            {
                GenomViewModels.Add(new GenomViewModel(i % 8, i / (constants.SizeBrain / 8), selectedBio.Pointer == i, selectedBio.Brain[i].ToString()));
            }

            selectedBio.Dead += SelectedBio_Dead;
            selectedBio.ChangePointer += SelectedBio_ChangePointer;
        }

        private void SelectedBio_Dead(object sender, bool e)
        {
            selectedBio.ChangePointer -= SelectedBio_ChangePointer;
            selectedBio.Dead -= SelectedBio_Dead;
            selectedBio.IsSelected = false;
            selectedBio = null;
            GenomViewModels = null;
        }

        private void SelectedBio_ChangePointer(object sender, int e)
        {
            GenomViewModels[prevIndexBrain].IsSelected = false;
            GenomViewModels[e].IsSelected = true;
            prevIndexBrain = e;
        }
    }
}
