using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

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
