using System.Threading.Tasks;
using UnityEngine;

public abstract class BaseModelLoader : MonoBehaviour, IModelLoader
{
    public bool IsLoading { get; protected set; } = false;
    public abstract string[] GetSupportedFileTypes();
    public abstract Task<bool> LoadModelAsync(string filePath);
    public virtual GameObject LoadedModel => loadedModel;
    protected GameObject loadedModel;

    public abstract string LoaderName { get; }

    protected void RemoveAnimatorIfPresent(Transform target)
    {
        var animator = target.GetComponent<Animator>();
        if (animator != null)
        {
            Debug.Log($"Animator found on {target.name}. Removing component.");
            Destroy(animator);
        }
    }
}
