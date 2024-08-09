using Diol.Share.Services;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Diol.Wpf.Core
{
    /// <summary>
    /// Represents the main module of the application.
    /// </summary>
    public class MainModule : IModule
    {
        /// <summary>
        /// Initializes the module.
        /// </summary>
        /// <param name="containerProvider">The container provider.</param>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var applicationStateService = containerProvider.Resolve<IApplicationStateService>();
            applicationStateService.Subscribe();

            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("MainRegion", typeof(Views.WelcomeComponent));
        }

        /// <summary>
        /// Registers types in the container.
        /// </summary>
        /// <param name="containerRegistry">The container registry.</param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.WelcomeComponent>();
            containerRegistry.RegisterForNavigation<Views.MainComponent>();
        }
    }
}
