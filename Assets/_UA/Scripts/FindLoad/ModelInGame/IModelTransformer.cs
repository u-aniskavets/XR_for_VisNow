using UnityEngine;

public interface IModelTransformer
{
    void ScaleModelToFit(GameObject model);
    void CenterModel(GameObject model);
}
