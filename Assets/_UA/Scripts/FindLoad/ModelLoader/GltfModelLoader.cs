using System;
using System.Threading.Tasks;
using UnityEngine;
using GLTFast;

public class GltfModelLoader : BaseModelLoader
{
    private GltfImport gltf;
    public override string LoaderName => "GLTF Loader";
    public override string[] GetSupportedFileTypes()
    {
        return new[] { ".gltf", ".glb" };
    }

    public override async Task<bool> LoadModelAsync(string filePath)
    {
        IsLoading = true;
        try
        {
            gltf = new GltfImport();
            Debug.Log($"Starting GLTF load: {filePath}");
            bool loadSuccess = await gltf.Load(new Uri(filePath).AbsoluteUri);
            if (!loadSuccess)
            {
                Debug.LogError("Failed to load GLTF model.");
                return false;
            }
            Debug.Log("GLTF model loaded successfully. Instantiating...");
            RemoveAnimatorIfPresent(transform);
            bool instantiateSuccess = await gltf.InstantiateMainSceneAsync(transform);
            if (!instantiateSuccess)
            {
                Debug.LogError("Failed to instantiate GLTF model.");
                return false;
            }
            Debug.Log("GLTF model instantiated successfully.");
            loadedModel = transform.GetChild(0).gameObject;
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading GLTF model: {ex.Message}");
            return false;
        }
        finally
        {
            IsLoading = false;
        }
    }
}
