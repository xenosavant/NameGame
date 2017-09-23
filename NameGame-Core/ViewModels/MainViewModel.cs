using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using WillowTree.NameGame.Core.Models;
using WillowTree.NameGame.Core.Services;

namespace WillowTree.NameGame.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private INameGameService _service;

		public MainViewModel(INameGameService service)
        {
            _service = service;
        }

        public override async void Start()
        {
            base.Start();
            Profiles = await _service.GetProfiles();
        }

        private Profile[] _profiles;

        public Profile[] Profiles
		{
			get { return _profiles; }
			set
			{
                _profiles = value;
				RaisePropertyChanged(() => Profiles);
			}
		}


    }
}
