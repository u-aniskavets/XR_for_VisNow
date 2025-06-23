using UnityEngine;

public interface IModelManipulator
{
    void SetModel(GameObject model);
    void RotateModel(Vector3 rotation);
    void MoveModel(Vector3 position);
}
