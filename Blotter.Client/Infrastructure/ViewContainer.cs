using System;
using DynamicData.Binding;

namespace Blotter.Client.Infrastructure
{
    public class ViewContainer : AbstractNotifyPropertyChanged, IViewContainer
    {
        public ViewContainer(string title, object content)
        {
            Title = title;
            Content = content;
        }

        public delegate IViewContainer Factory(string title, object content);

        public Guid Id { get; } = Guid.NewGuid();
        public string Title { get; }
        public object Content { get; }
    }
}