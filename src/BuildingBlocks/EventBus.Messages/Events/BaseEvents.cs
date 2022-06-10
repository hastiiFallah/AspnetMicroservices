namespace EventBus.Messages.Events
{
    public class BaseEvents
    {
        public BaseEvents()
        {
            Id = Guid.NewGuid();
            DateCreation = DateTime.UtcNow;
        }
        public BaseEvents(Guid id, DateTime date)
        {
            Id = id;
            DateCreation = date;
        }
        public Guid Id { get; private set; }
        public DateTime DateCreation { get; private set; }
    }
}
