using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModelLoaderSelector : MonoBehaviour
{
    private readonly List<IModelLoader> loaders = new List<IModelLoader>();

    private void Awake()
    {
        InitializeLoaders();
    }

    private void InitializeLoaders()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<IModelLoader>(out var loader))
            {
                RegisterLoader(loader);
            }
            else
            {
                Debug.LogWarning($"ModelLoaderSelector: Child {child.name} does not implement IModelLoader.");
            }
        }
        Debug.Log($"ModelLoaderSelector: Initialization complete. Found {loaders.Count} loaders.");
    }

    public void RegisterLoader(IModelLoader loader)
    {
        if (loader == null)
        {
            Debug.LogError("ModelLoaderSelector: Attempted to register a null loader.");
            return;
        }

        if (!loaders.Contains(loader))
        {
            loaders.Add(loader);
            Debug.Log($"ModelLoaderSelector: Successfully registered loader {loader.GetType().Name} ({loader.LoaderName}).");
        }
        else
        {
            Debug.LogWarning($"ModelLoaderSelector: Loader {loader.LoaderName} is already registered.");
        }
    }

    public string[] GetAllSupportedExtensions()
    {
        var extensions = new HashSet<string>();

        foreach (var loader in loaders)
        {
            var supportedExtensions = loader.GetSupportedFileTypes();
            Debug.Log($"ModelLoaderSelector: Loader {loader.LoaderName} supports extensions: {string.Join(", ", supportedExtensions)}");
            extensions.UnionWith(supportedExtensions);
        }
        Debug.Log($"ModelLoaderSelector: Total supported extensions: {string.Join(", ", extensions)}");
        return extensions.ToArray();
    }

    public IModelLoader SelectLoader(string filePath)
    {
        Debug.Log($"ModelLoaderSelector: Selecting loader for file {filePath}");
        string fileExtension = System.IO.Path.GetExtension(filePath)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(fileExtension))
        {
            Debug.LogWarning($"ModelLoaderSelector: File {filePath} has no valid extension.");
            return null;
        }
        Debug.Log($"ModelLoaderSelector: Extracted file extension {fileExtension}");
        foreach (var loader in loaders)
        {
            var supportedExtensions = loader.GetSupportedFileTypes().Select(ext => ext.TrimStart('*').ToLowerInvariant());

            Debug.Log($"ModelLoaderSelector: Checking loader {loader.LoaderName} with supported extensions: {string.Join(", ", supportedExtensions)}");
            if (supportedExtensions.Contains(fileExtension))
            {
                Debug.Log($"ModelLoaderSelector: Loader {loader.LoaderName} selected for file {filePath}");
                return loader;
            }
        }
        Debug.LogWarning($"ModelLoaderSelector: No suitable loader found for file {filePath}. Supported extensions: {string.Join(", ", GetAllSupportedExtensions())}");
        return null;
    }

}
