using System;
using System.IO;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;
using MvvmCross.Binding.Droid.Views;
using WillowTree.NameGame.Core.Models;
using Android.Graphics;
using Android.Views;
using Android.Webkit;

namespace WillowTree.NameGame.Droid.CustomBindings
{
    public class WebViewLoadingBinding : MvxAndroidTargetBinding
    {
        public WebViewLoadingBinding(object target) : base(target)
        {

        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override Type TargetType
        {
            get { return typeof(byte[]); }
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as WebView;
            if (view != null && value != null)
            {
                view.LoadUrl(string.Format("file:///android_asset/loading.html"));
            }
        }
    }
}
