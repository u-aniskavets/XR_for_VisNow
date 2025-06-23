using System.Collections.Generic;
using System.Threading.Tasks;

public interface IFileSearcher
{
    string SearcherName { get; }
    Task<List<string>> SearchFilesAsync(string[] fileExtensions);
}
