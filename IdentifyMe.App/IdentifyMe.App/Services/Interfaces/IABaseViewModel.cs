using System.Threading.Tasks;

namespace IdentifyMe.App.Services.Interfaces
{
    public interface IABaseViewModel
    {
        string Name { get; set; }

        string Title { get; set; }

        bool IsBusy { get; set; }

        Task InitializeAsync(object navigationData);
    }
}
