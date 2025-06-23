using UnityEngine;
using System.Threading.Tasks;
using Dummiesman;

public class OBJModelLoader : MonoBehaviour, IModelLoader
{
    private GameObject loadedModel;

    public string LoaderName => "OBJ Loader";

    public bool IsLoading { get; private set; } = false;

    public GameObject LoadedModel => loadedModel;

    public string[] GetSupportedFileTypes()
    {
        return new[] { ".obj" };
    }

    public async Task<bool> LoadModelAsync(string filePath)
    {
        IsLoading = true;
        try
        {
            Debug.Log("OBJModelLoader: Starting model load... " + filePath);

            OBJLoader objLoader = new OBJLoader();
            loadedModel = objLoader.Load(filePath);

            if (loadedModel != null)
            {
                loadedModel.transform.SetParent(transform, false);
                Debug.Log("OBJModelLoader: Model loaded successfully!");
                return true;
            }
            else
            {
                Debug.LogWarning("OBJModelLoader: Load returned null.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("OBJModelLoader: Exception: " + ex);
        }
        finally
        {
            IsLoading = false;
        }

        return false;
    }
}
