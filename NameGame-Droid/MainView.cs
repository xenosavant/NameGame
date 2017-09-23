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
    }
}