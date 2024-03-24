namespace Diol.Share.Features.EntityFrameworks
{
    public class CommandExecutingDto : BaseDto
    {
        public override string CategoryName => "EntityFramework";

        public override string EventName => nameof(CommandExecutingDto);

        public string Parameters { get; set; }

        public string CommandText { get; set; }
    }
}
