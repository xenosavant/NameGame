using System;
using MvvmCross.Core.ViewModels;

namespace WillowTree.NameGame.Core.Models
{
    public class ProfileCell : MvxNotifyPropertyChanged
    {
        
        public string FullName { get; set; }

        public bool Correct { get; set; }

		private byte [] _image;
		public byte[] Image
		{
			get { return _image; }
			set
			{
				_image = value;
				RaisePropertyChanged(() => Image);
			}
		}

        private bool _clicked;
        public bool Clicked {
            get { return _clicked; }
            set { 
                _clicked = value;
                RaisePropertyChanged(() => Clicked);
            } 
        }

		private Scaling _size;
		public Scaling Size
		{
			get { return _size; }
			set
			{
				_size = value;
				RaisePropertyChanged(() => Size);
			}
		}

		public IMvxCommand ImageClick
		{
			get
			{
                return new MvxCommand(() => Clicked = true);
			}
		}

    }
}
