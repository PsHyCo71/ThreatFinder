using System.Threading.Tasks;

namespace ThreatFinder.Core;
public interface IFilePickerService
{
    Task<string?> FilePickerAsync();
}