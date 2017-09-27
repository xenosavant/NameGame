using System;
using System.IO;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;
using MvvmCross.Binding.Droid.Views;
using WillowTree.NameGame.Core.Models;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace WillowTree.NameGame.Droid.CustomBindings
{
	public class FrameScaleBinding : MvxAndroidTargetBinding
	{
		public FrameScaleBinding(object target) : base(target)
		{

		}

		public override MvxBindingMode DefaultMode
		{
			get { return MvxBindingMode.OneWay; }
		}

		public override Type TargetType
		{
			get { return typeof(int); }
		}

		protected override void SetValueImpl(object target, object value)
		{
            var view = target as FrameLayout;
			if (view != null && value != null)
			{
                var scaling = (Scaling)value;
                view.LayoutParameters.Width = scaling.Width;
                view.LayoutParameters.Height = scaling.Height;
			}
		}
	}
}
