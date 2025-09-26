namespace LabXand.DomainLayer.Core
{
    public interface IHandler<T>
    {
        bool CanHandle(IDomainEvent eventType);
        void Handle(IDomainEvent eventData);
    }
}