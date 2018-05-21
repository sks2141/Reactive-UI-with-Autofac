using System;

namespace Blotter.Client.Infrastructure
{
    public interface IViewContainer
    {
        Guid Id { get; }

        string Title { get; }

        object Content { get; }
    }
}