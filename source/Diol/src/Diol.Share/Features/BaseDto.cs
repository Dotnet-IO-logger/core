namespace Diol.Share.Features
{
    public abstract class BaseDto
    {
        public string CorrelationId { get; set; }

        public abstract string CategoryName { get; }

        public abstract string EventName { get; }

        public int ProcessId { get; set; }

        public string ProcessName { get; set; }
    }
}
