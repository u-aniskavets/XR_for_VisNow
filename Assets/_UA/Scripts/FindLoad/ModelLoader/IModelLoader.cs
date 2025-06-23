using System.Threading.Tasks;
using UnityEngine;

public interface IModelLoader
{
    Task<bool> LoadModelAsync(string filePath);
    string[] GetSupportedFileTypes();
    bool IsLoading {get;}
    GameObject LoadedModel {get;}
    string LoaderName {get;}
}
