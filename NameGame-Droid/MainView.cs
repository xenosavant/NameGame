using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Views;
using WillowTree.NameGame.Core.ViewModels;
using WillowTree.NameGame.Droid.Services;
using WillowTree.NameGame.Core.Services;
using WillowTree.NameGame.Core.Models;
using MvvmCross.Platform;

namespace WillowTree.NameGame.Droid
{
    [Activity(Label = "NameGame", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainView : MvxActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.view_main);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }


    }
}