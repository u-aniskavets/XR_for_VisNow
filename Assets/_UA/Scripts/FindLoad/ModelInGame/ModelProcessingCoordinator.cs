using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ModelProcessingCoordinator : MonoBehaviour
{
    [SerializeField] private FileSearcherManager fileSearcherManager;
    [SerializeField] private ModelLoaderSelector loaderSelector;
    [SerializeField] private ModelStorage modelStorage;

    private void Start()
    {
        if (fileSearcherManager == null || loaderSelector == null || modelStorage == null)
        {
            Debug.LogError("ModelProcessingCoordinator: Missing required dependencies.");
        }

        var fileExtensions = loaderSelector.GetAllSupportedExtensions();
        fileSearcherManager.SetFileExtensions(fileExtensions);

        //StartProcessing();
    }

    [ContextMenu("RUN Start Processing")]
    public void StartProcessing()
    {
        _ = StartProcessingAsync();
    }

    public async Task StartProcessingAsync()
    {
        Debug.Log("ModelProcessingCoordinator: Starting file search...");
        var files = await fileSearcherManager.FindAllSupportedFilesAsync();
        if (files.Count == 0)
        {
            Debug.LogWarning("ModelProcessingCoordinator: No files found for processing.");
            return;
        }
        Debug.Log($"ModelProcessingCoordinator: Found {files.Count} files for processing.");
        await ProcessFileAsync(files[0]);
        Debug.Log("ModelProcessingCoordinator: Processing complete.");
    }

    private async Task ProcessFileAsync(string filePath)
    {
        Debug.Log($"ModelProcessingCoordinator: Processing file {filePath}...");
        var loader = loaderSelector.SelectLoader(filePath);
        if (loader == null)
        {
            Debug.LogWarning($"ModelProcessingCoordinator: Skipping file {filePath}. No suitable loader found.");
            return;
        }
        Debug.Log($"ModelProcessingCoordinator: Using loader {loader.GetType().Name} for file {filePath}.");
        
        modelStorage.RemoveAllModels();
        bool loadSuccess = await loader.LoadModelAsync(filePath);
        if (!loadSuccess)
        {
            Debug.LogWarning($"ModelProcessingCoordinator: Failed to load model from {filePath}.");
            return;
        }

        var loadedModel = loader.LoadedModel;
        modelStorage.AddModel(loadedModel);
        Debug.Log($"ModelProcessingCoordinator: File {filePath} processed and added to storage.");
    }
}
