using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using WillowTree.NameGame.Core.Models;
using WillowTree.NameGame.Core.Services;
using System.Collections.ObjectModel;
using System.Net.Http;
using MvvmCross.Platform;

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
            LoadImages();
        }

        private async Task LoadImages(){
            if (Loading)
                return;
            
			Loading = true;

			try
			{

                TopRow = new ObservableCollection<ProfileCell>();
                BottomRow = new ObservableCollection<ProfileCell>();
				var profiles = await _service.GetProfiles();
				Random rng = new Random();
				var correctProfile = profiles[rng.Next(profiles.Length)];
				Prompt = "Who is " + correctProfile.FullName + "?";
				int selected = rng.Next(profiles.Length);
				var profileDtoList = new ObservableCollection<ProfileCell>();
				var deviceService = Mvx.Resolve<IDeviceService>();
				var imageService = Mvx.Resolve<IImageService>();
				int width = deviceService.GetDeviceWidth();
				int height = deviceService.GetDeviceHeight();
				int size;
				if (width > height)
				{
					size = height / 3;
				}
				else size = width / 3;

				for (int i = 0; i < profiles.Length; i++)
				{
					var profile = profiles[i];
					var profileCell = new ProfileCell()
					{
						FullName = profile.FullName,
						Correct = profile.Equals(correctProfile) ? true : false,
						Clicked = false,
						Size = new Scaling() { Width = size, Height = size }
					};

					var client = new HttpClient();
					var imageResponse = await client.GetAsync("http:" + profile.Headshot.Url);
					using (Stream stream = await imageResponse.Content.ReadAsStreamAsync())
					using (MemoryStream memStream = new MemoryStream())
					{
						stream.CopyTo(memStream);
						if (profile.Headshot.Width != profile.Headshot.Height)
							profileCell.Image = await imageService.CropImage(memStream.ToArray());
						else profileCell.Image = memStream.ToArray();
					}
					if (i < 2)
					{
						TopRow.Add(profileCell);
					}
					else
					{
						BottomRow.Add(profileCell);
					}
				}
			}
			catch (Exception e) { }
			finally { Loading = false; }

        }

        private ObservableCollection<ProfileCell> _topRow;
        public ObservableCollection<ProfileCell> TopRow
        {
            get { return _topRow; }
            set
            {
                SetProperty(ref _topRow, value);
                RaisePropertyChanged(() => TopRow);
            }
        }

        private ObservableCollection<ProfileCell> _bottomRow;
		public ObservableCollection<ProfileCell> BottomRow
		{
			get { return _bottomRow; }
			set
			{
				SetProperty(ref _bottomRow, value);
				RaisePropertyChanged(() => BottomRow);
			}
		}

		private ProfileCell _cell = new ProfileCell();
		public ProfileCell Cell
		{
            get { return _cell; }
			set
			{
				SetProperty(ref _cell, value);
				RaisePropertyChanged(() => Cell);
			}
		}

        private String _prompt;
		public String Prompt
		{
			get { return _prompt; }
			set
			{
				SetProperty(ref _prompt, value);
				RaisePropertyChanged(() => Prompt);
			}
		}

        private bool _loading;
		public bool Loading
		{
            get { return _loading; }
			set
			{
				SetProperty(ref _loading, value);
				RaisePropertyChanged(() => Loading);
			}
		}

		public IMvxCommand ResetImages
		{
			get
			{
				return new MvxCommand(() => LoadImages());
			}
		}
    }
}
