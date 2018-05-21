using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DynamicData.Binding;
using ReactiveUI;

namespace Blotter.Client.Infrastructure
{
    public class MenuItem : AbstractNotifyPropertyChanged
    {
        public MenuItem(string title, string description, Action action,
            IEnumerable<Link> link = null,
            object content = null)
        {
            Title = title;
            Description = description;
            Content = content;
            Category = MenuCategory.ReactiveUi; //DynamicData
            Link = link ?? Enumerable.Empty<Link>();
            Command = ReactiveCommand.Create(action);
        }

        public MenuItem(string title, string description, Action action, MenuCategory category,
            IEnumerable<Link> link = null,
            object content = null)
        {
            Title = title;
            Description = description;
            Category = category;
            Link = link ?? Enumerable.Empty<Link>();
            Command = ReactiveCommand.Create(action);
            Content = content;
        }


        public string Title { get; }

        public ICommand Command { get; }

        public IEnumerable<Link> Link { get; }

        public string Description { get; }

        public object Content { get; }

        public MenuCategory Category { get; }
    }
}
