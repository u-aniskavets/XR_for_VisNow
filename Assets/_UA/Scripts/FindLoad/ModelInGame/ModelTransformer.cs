using UnityEngine;

public static class ModelTransformer
{
    public static void TransformModel(GameObject model, Transform boxTransform)
    {
        if (model == null)
        {
            Debug.LogError("ModelTransformer: Model is null.");
            return;
        }
        if (boxTransform == null)
        {
            Debug.LogError("ModelTransformer: Box is null.");
            return;
        }

        Debug.Log("ModelTransformer: Starting transformation.");

        model.transform.localRotation = Quaternion.identity;
        model.transform.localScale = Vector3.one;
        Debug.Log($"ModelTransformer: Model localRotation reset to: {model.transform.localRotation}, localScale reset to: {model.transform.localScale}");

        Renderer[] renderers = model.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogWarning("ModelTransformer: No renderers found.");
            return;
        }

        Bounds worldBounds = renderers[0].bounds;
        foreach (var r in renderers)
            worldBounds.Encapsulate(r.bounds);

        Debug.Log($"ModelTransformer: worldBounds center: {worldBounds.center}, size: {worldBounds.size}");

        Renderer boxRenderer = boxTransform.GetComponentInChildren<Renderer>();
        if (boxRenderer == null)
        {
            Debug.LogError("ModelTransformer: Box has no renderer.");
            return;
        }

        Bounds boxBounds = boxRenderer.bounds;
        Debug.Log($"ModelTransformer: boxBounds center: {boxBounds.center}, size: {boxBounds.size}");

        Vector3 modelSize = worldBounds.size;
        Vector3 boxSize = boxBounds.size;

        float modelMax = Mathf.Max(modelSize.x, Mathf.Max(modelSize.y, modelSize.z));
        float boxMin = Mathf.Min(boxSize.x, Mathf.Min(boxSize.y, boxSize.z));

        Debug.Log($"ModelTransformer: modelMax: {modelMax}");
        Debug.Log($"ModelTransformer: boxMin: {boxMin}");

        if (modelMax == 0)
        {
            Debug.LogWarning("ModelTransformer: Model size is zero.");
            return;
        }

        float scaleFactor = boxMin / modelMax;
        Debug.Log($"ModelTransformer: scaleFactor: {scaleFactor}");

        model.transform.localScale = Vector3.one * scaleFactor;
        Debug.Log($"ModelTransformer: Model localScale after scaling: {model.transform.localScale}");

        worldBounds = renderers[0].bounds;
        foreach (var r in renderers)
            worldBounds.Encapsulate(r.bounds);

        Debug.Log($"ModelTransformer: worldBounds AFTER scaling center: {worldBounds.center}, size: {worldBounds.size}");

        Vector3 worldOffset = boxBounds.center - worldBounds.center;
        Debug.Log($"ModelTransformer: worldOffset for centering: {worldOffset}");

        Transform attachTransform = model.transform.parent;
        Vector3 localOffset = attachTransform.InverseTransformVector(worldOffset);
        Debug.Log($"ModelTransformer: localOffset for centering: {localOffset}");

        model.transform.localPosition += localOffset;

        Debug.Log($"ModelTransformer: Final model localPosition: {model.transform.localPosition}");
        Debug.Log($"ModelTransformer: Final model localScale: {model.transform.localScale}");

        Debug.Log("ModelTransformer: Transformation complete.");
    }
}
