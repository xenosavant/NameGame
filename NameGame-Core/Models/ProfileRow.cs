using System;
using MvvmCross.Core.ViewModels;
using System.Collections.ObjectModel;

namespace WillowTree.NameGame.Core.Models
{
    public class ProfileRow : MvxNotifyPropertyChanged
    {
        private ObservableCollection<ProfileCell> _cells = new ObservableCollection<ProfileCell>();

        public ObservableCollection<ProfileCell> Cells { 
            get
            {
                return _cells;
            }
            set
            {
                SetProperty(ref _cells, value);
                RaisePropertyChanged(() => Cells);
            }
        }
    }
}