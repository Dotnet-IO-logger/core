namespace Diol.Share.Features
{
    public abstract class BaseFeatureFlag
    {
        public abstract string Name { get; }
        public abstract bool IsEnabled { get; }
    }
}
