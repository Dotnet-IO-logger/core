using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Diol.Wpf.Core
{
    public class MainModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion("MainRegion", typeof(Views.WelcomeComponent));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.WelcomeComponent>();
            containerRegistry.RegisterForNavigation<Views.MainComponent>();
        }
    }
}
