// This binding takes in an image byte aray and sets the view image on
// an MvxImageView

using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;
using MvvmCross.Binding.Droid.Views;
using Android.Graphics;
using System.IO;

namespace WillowTree.NameGame.Droid.CustomBindings
{
    public class ImageSourceBinding : MvxAndroidTargetBinding
    {
        public ImageSourceBinding(object target) : base(target)
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
            var control = target as MvxImageView;
            if (control != null && value != null)
            {
                var memoryStream = new MemoryStream((byte[])value);
                Bitmap bitmap = BitmapFactory.DecodeStream(memoryStream);
                control.SetImageBitmap(bitmap);
            }
        }

    }
}
