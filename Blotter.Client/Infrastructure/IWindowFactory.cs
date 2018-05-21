namespace Blotter.Client.Infrastructure
{
    public interface IWindowFactory
    {
        MainWindow Create(bool showMenu = false);
    }
}