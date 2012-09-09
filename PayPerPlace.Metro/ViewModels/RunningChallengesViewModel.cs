using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using ServicesLib;
using ServicesLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPerPlace.Metro.ViewModels
{
    public class RunningChallengesViewModel : ViewModelBase
    {
        private bool _IsLoading = false;
        public bool IsLoading 
        { 
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                RaisePropertyChanged(() => this.IsLoading);
            } 
        }

        private ObservableCollection<Challenge> _Items;
        public ObservableCollection<Challenge> Items 
        {
            get { return _Items; }
            set
            {
                _Items = value;
                RaisePropertyChanged(() => this.Items);
            }
        }

        public async void StartLoadingChallengesAsync()
        {
            IsLoading = true;

            PayPerPlaceServiceClient serviceClient = SimpleIoc.Default.GetInstance<PayPerPlaceServiceClient>();

            List<Challenge> runningChallenges = await serviceClient.GetRunningChallengesAsync();

            Items = new ObservableCollection<Challenge>();

            foreach (var c in runningChallenges)
            {
                Items.Add(c);
            }

            IsLoading = false;
        }
    }
}
