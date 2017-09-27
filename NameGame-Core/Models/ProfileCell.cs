﻿using System;
using MvvmCross.Core.ViewModels;

namespace WillowTree.NameGame.Core.Models
{
    public class ProfileCell : MvxNotifyPropertyChanged
    {
        
        public string FullName { get; set; }

        public bool Correct { get; set; }

        public int Index { get; set; }

        public byte[] Image { get; set; }

        public Scaling Size { get; set; }

        private bool _clicked;
        public bool Clicked {
            get { return _clicked; }
            set { 
                _clicked = value;
                RaisePropertyChanged(() => Clicked);
            } 
        }

		private bool _visible;
		public bool Visible
		{
			get { return _visible; }
			set
			{
				_visible = value;
				RaisePropertyChanged(() => Visible);
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
