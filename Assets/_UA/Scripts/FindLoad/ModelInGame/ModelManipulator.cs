using UnityEngine;

public class ModelManipulator : MonoBehaviour, IModelManipulator
{
    private GameObject currentModel;

    public void SetModel(GameObject model)
    {
        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        currentModel = Instantiate(model, transform);
    }

    public void RotateModel(Vector3 rotation)
    {
        if (currentModel != null)
        {
            currentModel.transform.Rotate(rotation);
        }
    }

    public void MoveModel(Vector3 position)
    {
        if (currentModel != null)
        {
            currentModel.transform.position = position;
        }
    }
}
