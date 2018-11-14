namespace Imago.IO
{
    public static class EventLogger
    {
        public static IEventLogger Default { get; set; } = new DoNothingEventLogger();
    }
}
