using Android.Content;
using Android.Webkit;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using WillowTree.NameGame.Droid.CustomBindings;
using WillowTree.NameGame.Core;
using Android.Widget;
using MvvmCross.Platform;
using WillowTree.NameGame.Core.Services;
using WillowTree.NameGame.Droid.Services;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Droid.Views;

namespace WillowTree.NameGame.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
            // Register the custom bindings
            registry.RegisterFactory(new MvxCustomBindingFactory<MvxImageView>("ImageSource", (imageView) => new ImageSourceBinding(imageView)));
            registry.RegisterFactory(new MvxCustomBindingFactory<FrameLayout>("Scale", (imageView) => new FrameScaleBinding(imageView)));
            registry.RegisterFactory(new MvxCustomBindingFactory<WebView>("LoadingView", (imageView) => new WebViewLoadingBinding(imageView)));
        }

        protected override IMvxApplication CreateApp()
        {
            // Register the platform dependencies
            Mvx.LazyConstructAndRegisterSingleton<IImageService, ImageService>();
            Mvx.LazyConstructAndRegisterSingleton<IDeviceService, DeviceService>();
            return new App();
        }
    }
}