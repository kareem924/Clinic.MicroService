using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.CQRS
{
    public interface ICommand : INotification
    {
    }
}
