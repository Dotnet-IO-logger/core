namespace Diol.Share.Features.Httpclients
{
    public class HttpclientFeatureFlag : BaseFeatureFlag
    {
        private readonly bool isEnabled;

        public override string Name => nameof(HttpclientFeatureFlag);

        public override bool IsEnabled => this.isEnabled;

        /// <summary>
        /// Set feature flag to true by default.
        /// </summary>
        public HttpclientFeatureFlag()
        {
            this.isEnabled = true;
        }

        /// <summary>
        /// Set feature flag to true or false.
        /// </summary>
        /// <param name="isEnabled"></param>
        public HttpclientFeatureFlag(bool isEnabled)
        {
            this.isEnabled = isEnabled;
        }
    }
}
