using System;

namespace Drop.Api.Services
{
    public interface IMessenger
    {
        string GetMessage();
    }

    public class Messenger : IMessenger
    {
        private readonly Guid _id = Guid.NewGuid();
        
        public string GetMessage() => $"Hello [id: {_id}]";
    }
    
    public class MessengerV2 : IMessenger
    {
        private readonly Guid _id = Guid.NewGuid();
        
        public string GetMessage() => $"Hello v2 [id: {_id}]";
    }
}