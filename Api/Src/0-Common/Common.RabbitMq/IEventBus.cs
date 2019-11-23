namespace Common.RabbitMq
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent @event);
    }
}
