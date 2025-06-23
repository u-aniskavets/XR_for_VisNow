using Oculus.Interaction.HandGrab;
using Oculus.Interaction;
using UnityEngine;

public class BoxVisibilityController : MonoBehaviour
{
    [SerializeField] private Renderer boxRenderer;
    [SerializeField] private DistanceHandGrabInteractable grabInteractable; 

    private Material boxMaterial;
    private float emptyAlpha = 1.0f;
    private float occupiedAlpha = 0.0f;
    private float hoverAlpha = 0.3f;

    private void Awake()
    {
        if (boxRenderer == null)
            boxRenderer = GetComponent<Renderer>();

        if (boxRenderer != null)
            boxMaterial = boxRenderer.material;

        if (grabInteractable != null)
        {
            grabInteractable.WhenSelectingInteractorViewAdded += OnHoverEnter;
            grabInteractable.WhenSelectingInteractorViewRemoved += OnHoverExit;
        }
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.WhenSelectingInteractorViewAdded -= OnHoverEnter;
            grabInteractable.WhenSelectingInteractorViewRemoved -= OnHoverExit;
        }
    }

    private void OnHoverEnter(IInteractorView interactor)
    {
        Debug.Log("BoxVisibilityController: Hover Enter - Setting hover alpha");
        SetAlpha(hoverAlpha);
    }

    private void OnHoverExit(IInteractorView interactor)
    {
        Debug.Log("BoxVisibilityController: Hover Exit - Setting occupied alpha");
        SetAlpha(occupiedAlpha);
    }

    public void SetEmptyState()
    {
        Debug.Log("BoxVisibilityController: SetEmptyState - Setting alpha");
        SetAlpha(emptyAlpha);
    }

    public void SetOccupiedState()
    {
        Debug.Log("BoxVisibilityController: SetOccupiedState - Setting alpha");
        SetAlpha(occupiedAlpha);
    }

    private void SetAlpha(float alpha)
    {
        if (boxMaterial == null)
        {
            Debug.LogWarning("BoxVisibilityController: No material assigned");
            return;
        }

        Color color = boxMaterial.color;
        color.a = alpha;
        boxMaterial.color = color;

        Debug.Log($"BoxVisibilityController: New material alpha: {alpha}");
    }
}
