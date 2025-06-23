using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class LocalFileSearcher : MonoBehaviour, IFileSearcher
{
    [SerializeField] private FileSearchDirectories fileSearchDirectories;
    public string SearcherName => "LocalFileSearcher";

    public Task<List<string>> SearchFilesAsync(string[] fileExtensions)
    {
        return Task.Run(() =>
        {
            var foundFiles = new List<string>();

            if (fileSearchDirectories == null || fileSearchDirectories.Directories == null)
            {
                Debug.LogWarning("LocalFileSearcher: FileSearchDirectories not assigned or empty.");
                return foundFiles;
            }

            foreach (var directory in fileSearchDirectories.Directories)
            {
                if (!Directory.Exists(directory))
                {
                    Debug.LogWarning($"LocalFileSearcher: Directory does not exist: {directory}");
                    continue;
                }

                foreach (var extension in fileExtensions)
                {
                    try
                    {
                        var files = Directory.GetFiles(directory, $"*{extension}", SearchOption.AllDirectories)
                            .Select(file => file.Replace("\\", "/"));
                        foundFiles.AddRange(files);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"LocalFileSearcher: Error searching in {directory} with extension {extension}. Exception: {ex.Message}");
                    }
                }
            }

            Debug.Log($"LocalFileSearcher: Found {foundFiles.Count} files.");
            return foundFiles;
        });
    }

    public void AddDirectory(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Debug.LogWarning($"LocalFileSearcher: Directory does not exist and cannot be added: {directory}");
            return;
        }

        if (fileSearchDirectories != null && !fileSearchDirectories.Directories.Contains(directory))
        {
            fileSearchDirectories.Directories.Add(directory);
            Debug.Log($"LocalFileSearcher: Directory added: {directory}");
        }
    }

    public void RemoveDirectory(string directory)
    {
        if (fileSearchDirectories != null && fileSearchDirectories.Directories.Remove(directory))
        {
            Debug.Log($"LocalFileSearcher: Directory removed: {directory}");
        }
        else
        {
            Debug.LogWarning($"LocalFileSearcher: Directory not found in the list: {directory}");
        }
    }

    public List<string> GetDirectories()
    {
        return fileSearchDirectories != null ? new List<string>(fileSearchDirectories.Directories) : new List<string>();
    }

}
