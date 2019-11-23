using RabbitMQ.Client;
using System;

namespace Common.RabbitMq
{
    internal interface IRabbitMQPersistentConnection
         : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
