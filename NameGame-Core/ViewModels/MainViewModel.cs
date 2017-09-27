using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using WillowTree.NameGame.Core.Models;
using WillowTree.NameGame.Core.Services;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace WillowTree.NameGame.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {

        // The API service
        private INameGameService _nameGameService;

        // The platform specific device service that retrieves screen dimensions
        private IDeviceService _deviceService;

        // The platform specific image service for cropping the images
        private IImageService _imageService;

        // The current game mode
        private Enumerations.GameModes _mode;

        // A list of the current ProfileCells
        private List<ProfileCell> _profileCells;

        // The currently running Task
        private Task _currentTask;

        // Use constructor injection to resolve the dependenies
        public MainViewModel(INameGameService service, IDeviceService deviceService, IImageService imageService)
        {
            _nameGameService = service;
            _deviceService = deviceService;
            _imageService = imageService;
        }

        public override void Start()
        {
            base.Start();
            _currentTask = StartGame(Enumerations.GameModes.Standard);
        }

        private async Task StartGame(Enumerations.GameModes mode)
        {

            // Set the game mode
            _mode = mode;

            // If already loading, return
            if (Loading)
                return;

            // If an error occured on the last run, reset the flag
            if (Error)
                Error = false;

            // Set loading flag
            Loading = true;

            try
            {
                // If this isn't the first run, recalculate the score
                if (TopRow != null && BottomRow != null)
                    CalculateScore();

                // Initalize collections
                TopRow = new ObservableCollection<ProfileCell>();
                BottomRow = new ObservableCollection<ProfileCell>();
                _profileCells = new List<ProfileCell>();

                // Get the 5 profiles from the service
                var profiles = await _nameGameService.GetProfiles(5);

                // Pick a random profile as the correct choice and set prompt
                Random rng = new Random();
                var correctProfile = profiles.ElementAt(rng.Next(profiles.Count()));
                Prompt = "Who is " + correctProfile.FullName + "?";

                // Get the width and height for the current orientation and set
                // the cell size to 1/3 the smaller dimension with some padding
                int width = _deviceService.GetScreenWidth();
                int height = _deviceService.GetScreenHeight();
                int size;
                if (width > height)
                {
                    size = (height - 40) / 3;
                }
                else size = (width - 40) / 3;

                for (int i = 0; i < profiles.Count(); i++)
                {
                    // Create a cell object from the profile
                    Profile profile = profiles.ElementAt(i);
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

                    // Start a new thread to process the image for each profile and await them
                    await Task.Run(async () =>
                    {
                        var imageResponse = await client.GetAsync("http:" + profile.Headshot.Url);
                        using (Stream stream = await imageResponse.Content.ReadAsStreamAsync())
                        using (MemoryStream memStream = new MemoryStream())
                        {
                            stream.CopyTo(memStream);

                            // If the image is not square, crop with the imageService
                            if (profile.Headshot.Width != profile.Headshot.Height)
                                profileCell.Image = await _imageService.CropImage(memStream.ToArray());
                            else profileCell.Image = memStream.ToArray();
                        }
                    });

                    // Add the cells to the observable collection rows for view presentation
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
                // Catch any exceptions that occured and set the error flag to true
                Debug.WriteLine(e.Message);
                Error = true;
            }
            finally
            {
                // Set loading to false and if we are in timed mode and no errors occured
                // then start the timer again
                Loading = false;
                if (_mode == Enumerations.GameModes.Timed && !Error)
                    await StartTimer();
            }
        }

        void CalculateScore()
        {
            // Iterate though the cells and tabulate the answers
            foreach (ProfileCell profile in _profileCells)
            {
                if (profile.Clicked)
                {
                    TotalAnswers++;
                    if (profile.Correct)
                        CorrectAnswers++;
                }
            }
            // if any were answered calculate the score and update the ViewModel score value
            if (TotalAnswers > 0)
                PercentCorrect = ((float)CorrectAnswers / (float)TotalAnswers) * (float)100;
            Score = String.Format("{0}% Correct", Math.Floor(PercentCorrect));
        }

        // See if there are more than 1 incorrect cells that are unselected and still visible.
        // If so, remove a random one by setting it's Visible property to false
        void RemoveItem()
        {
            var possibleRemovals = _profileCells.Where(p => !p.Clicked && !p.Correct && p.Visible);
            if (possibleRemovals.Count() > 1)
            {
                var rng = new Random();
                possibleRemovals.ElementAt(rng.Next(possibleRemovals.Count())).Visible = false;
            }
        }

        // Start a timer with an adaptive time that is 2 seconds plus a scale factor
        // between 0 and 5 based on the current score
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


        public IMvxCommand Next
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

        // If the correct answer has been selected or there is an error or we are in timed mode
        // then restart the game in standard mode
        private void GoToNext()
        {
            if (_profileCells.Where(p => p.Clicked && p.Correct).Any() || Error || _mode == Enumerations.GameModes.Timed)
                _currentTask = StartGame(Enumerations.GameModes.Standard);
        }
    }
}
