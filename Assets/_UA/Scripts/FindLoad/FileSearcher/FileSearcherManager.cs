using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FileSearcherManager : MonoBehaviour, IFileSearcherManager
{
    private List<IFileSearcher> searchers = new List<IFileSearcher>();
    private string[] fileExtensions;

    private void Awake()
    {
        InitializeSearchers();
    }

    private void InitializeSearchers()
    {
        searchers = new List<IFileSearcher>(GetComponentsInChildren<IFileSearcher>());

        Debug.Log($"FileSearcherManager: Found {searchers.Count} searchers in children.");
    }

    public void RegisterSearcher(IFileSearcher searcher)
    {
        if (searcher == null)
        {
            Debug.LogError("FileSearcherManager: Attempted to register a null searcher.");
            return;
        }

        if (!searchers.Contains(searcher))
        {
            searchers.Add(searcher);
            Debug.Log($"FileSearcherManager: Registered searcher {searcher.SearcherName}.");
        }
    }

    public void SetFileExtensions(string[] extensions)
    {
        fileExtensions = extensions;
    }

    public async Task<List<string>> FindAllSupportedFilesAsync()
    {
        if (fileExtensions == null || fileExtensions.Length == 0)
        {
            Debug.LogError("FileSearcherManager: File extensions are not set. Aborting search.");
            return new List<string>();
        }

        var allFiles = new HashSet<string>();
        foreach (var searcher in searchers)
        {
            var files = await searcher.SearchFilesAsync(fileExtensions);
            foreach (var file in files)
            {
                allFiles.Add(file);
            }
        }

        Debug.Log($"FileSearcherManager: Search complete. Found {allFiles.Count} files.");
        return new List<string>(allFiles);
    }
}
