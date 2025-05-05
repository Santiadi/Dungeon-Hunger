using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 originalScale;
    public float hoverScale = 1.1f;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Time.timeScale = 1f;

        // Destruir el menú
        Transform current = transform;
        while (current != null && current.name != "UpgradesMenuCanvas(Clone)")
        {
            current = current.parent;
        }

        if (current != null)
        {
            Destroy(current.gameObject);
        }

        // Destruir el trigger por nombre
        GameObject trigger = GameObject.Find("UpgradesTrigger");
        if (trigger != null)
        {
            Destroy(trigger);
        }
    }
}
