using System.Collections.Generic;
using System.Threading.Tasks;

public interface IFileSearcherManager
{
    void RegisterSearcher(IFileSearcher searcher);
    void SetFileExtensions(string[] extensions);
    Task<List<string>> FindAllSupportedFilesAsync();
}
