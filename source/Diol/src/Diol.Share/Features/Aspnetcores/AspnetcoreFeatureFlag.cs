namespace Diol.Share.Features.Aspnetcores
{
    public class AspnetcoreFeatureFlag : BaseFeatureFlag
    {
        private readonly bool isEnabled;

        public override string Name => nameof(AspnetcoreFeatureFlag);

        public override bool IsEnabled => this.isEnabled;

        /// <summary>
        /// Set feature flag to true by default.
        /// </summary>
        public AspnetcoreFeatureFlag()
        {
            this.isEnabled = true;
        }

        /// <summary>
        /// Set feature flag to true or false.
        /// </summary>
        /// <param name="isEnabled"></param>
        public AspnetcoreFeatureFlag(bool isEnabled)
        {
            this.isEnabled = isEnabled;
        }
    }
}
