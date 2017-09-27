using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
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

		private Enumerations.GameModes _mode;

        private List<ProfileCell> _profileCells;

        private Task _currentTask;

        public MainViewModel(INameGameService service)
        {
            _service = service;
        }

        public override async void Start()
        {
			base.Start();
            _currentTask = StartGame(Enumerations.GameModes.Standard);
        }

        private async Task StartGame(Enumerations.GameModes mode){

			_mode = mode;

            if (Loading)
                return;
            if (Error)
                Error = false;

			Loading = true;

			try
			{
                if (TopRow != null && BottomRow != null)
                    CalculateScore();
                TopRow = new ObservableCollection<ProfileCell>();
                BottomRow = new ObservableCollection<ProfileCell>();
                _profileCells = new List<ProfileCell>();
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
                    size = (height - 40) / 3;
				}
                else size = (width - 40) / 3;

				for (int i = 0; i < profiles.Length; i++)
				{
					var profile = profiles[i];
					var profileCell = new ProfileCell()
					{
						FullName = profile.FullName,
						Correct = profile.Equals(correctProfile) ? true : false,
						Clicked = false,
						Size = new Scaling() { Width = size, Height = size },
                        Visible = true,
                        Index = i
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
                    _profileCells.Add(profileCell);
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
			catch (Exception e) 
            {
                Debug.WriteLine(e.Message);
                Error = true;
            }
			finally {
				Loading = false;
				if (_mode == Enumerations.GameModes.Timed && !Error)
                    await StartTimer();
            }
        }

		void CalculateScore()
		{
            var profiles = TopRow.Concat(BottomRow).ToList();
			foreach (ProfileCell profile in profiles)
			{
				if (profile.Clicked)
				{
                    TotalAnswers++;
					if (profile.Correct)
						CorrectAnswers++;
				}
			}
            if (TotalAnswers > 0)
			    PercentCorrect = ((float)CorrectAnswers / (float)TotalAnswers) * (float)100;
            Score = String.Format("{0}% Correct", Math.Floor(PercentCorrect));
		}

        void RemoveItem()
        {
            var possibleRemovals = _profileCells.Where(p => !p.Clicked && !p.Correct && p.Visible);
            if (possibleRemovals.Count() > 0)
            {
                var rng = new Random();
                possibleRemovals.ElementAt(rng.Next(possibleRemovals.Count())).Visible = false;
            }
        }

        async Task StartTimer()
        {
            await Task.Delay(TimeSpan.FromSeconds(2 + ((100 - PercentCorrect) / 20)));
            if (_mode == Enumerations.GameModes.Timed && !Error)
                _currentTask = StartGame(Enumerations.GameModes.Timed);
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

		private String _score;
		public String Score
		{
			get { return _score; }
			set
			{
				SetProperty(ref _score, value);
				RaisePropertyChanged(() => Score);
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

		private bool _error;
		public bool Error
		{
			get { return _error; }
			set
			{
				SetProperty(ref _error, value);
				RaisePropertyChanged(() => Error);
			}
		}



		private int _correctAnswers;
		public int CorrectAnswers
		{
			get { return _correctAnswers; }
			set
			{
				SetProperty(ref _correctAnswers, value);
				RaisePropertyChanged(() => CorrectAnswers);
			}
		}

		private int _totalAnswers;
		public int TotalAnswers
		{
			get { return _totalAnswers; }
			set
			{
				SetProperty(ref _totalAnswers, value);
				RaisePropertyChanged(() => TotalAnswers);
			}
		}

		private float _percentCorrect;
		public float PercentCorrect
		{
			get { return _percentCorrect; }
			set
			{
				SetProperty(ref _percentCorrect, value);
				RaisePropertyChanged(() => PercentCorrect);
			}
		}


		public IMvxCommand Reset
		{
			get
			{
                return new MvxCommand(() => GoToNext());
			}
		}

		public IMvxCommand TimedMode
		{
			get
			{
                return new MvxCommand(() => _currentTask = StartGame(Enumerations.GameModes.Timed));
			}
		}

		public IMvxCommand Hint
		{
			get
			{
				return new MvxCommand(() => RemoveItem());
			}
		}

        private void GoToNext()
        {
            if (_profileCells.Where(p => p.Clicked && p.Correct).Any() || Error || _mode == Enumerations.GameModes.Timed)
                _currentTask = StartGame(Enumerations.GameModes.Standard);
        }
    }
}
