using System;
using WillowTree.NameGame.Core.Services;
using Xamarin.Android;
using Android.Preferences;
using Android.Util;
using Android.Content.Res;
using Android.Hardware;

namespace WillowTree.NameGame.Droid.Services
{
    public class DeviceService : IDeviceService
    {
        DisplayMetrics metrics = Resources.System.DisplayMetrics;

        public int GetDeviceWidth()
        {
			return (int)metrics.WidthPixels;
        }

		public int GetDeviceHeight()
		{
			return (int)metrics.HeightPixels;
		}

    }
}
