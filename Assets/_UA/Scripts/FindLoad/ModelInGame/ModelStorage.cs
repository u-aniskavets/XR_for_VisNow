using System.Collections.Generic;
using UnityEngine;

public class ModelStorage : MonoBehaviour
{
    [SerializeField] private Transform BoxInteractable;
    [SerializeField] private Transform attachTransform;
    [SerializeField] private BoxVisibilityController boxVisibility;
    private List<GameObject> storedModels = new List<GameObject>();


    private void Awake()
    {
        if (BoxInteractable == null)
        {
            Debug.LogError("ModelStorage: BoxInteractable not found");
        }
        if (attachTransform == null)
        {
            Debug.LogError("ModelStorage: Attach Transform not found");
        }
    }

    public void AddModel(GameObject model)
    {
        if (model == null)
        {
            Debug.LogError("ModelStorage: Attempted to add a null model.");
            return;
        }
        if (attachTransform == null)
        {
            Debug.LogError("ModelStorage: Attach Transform not assigned.");
            return;
        }

        model.transform.SetParent(attachTransform, true);
        ModelTransformer.TransformModel(model, BoxInteractable);

        storedModels.Add(model);

        boxVisibility?.SetOccupiedState();
        Debug.Log($"ModelStorage: Model {model.name} scaled and added.");
    }

    public List<GameObject> GetAllModels()
    {
        return storedModels;
    }

    public void RemoveModel(GameObject model)
    {
        if (model != null)
        {
            if (storedModels.Contains(model))
            {
                storedModels.Remove(model);
            }
            Destroy(model);
        }
    }

    public void RemoveAllModels()
    {
        foreach (var model in storedModels)
        {
            if (model != null)
            {
                Destroy(model);
            }
        }
        storedModels.Clear();
    }
}
