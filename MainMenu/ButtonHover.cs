using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 hoverScale = Vector3.one * 1.08f;
    public float duration = 0.12f;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.cyan;

    private Vector3 startScale;
    private Image image;

    private void Awake()
    {
        startScale = transform.localScale;
        image = GetComponentInChildren<Image>();
        if (image == null) image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(hoverScale, duration));
        if (image != null) image.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(startScale, duration));
        if (image != null) image.color = normalColor;
    }

    private System.Collections.IEnumerator ScaleTo(Vector3 target, float secs)
    {
        Vector3 from = transform.localScale;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / secs;
            transform.localScale = Vector3.Lerp(from, target, t);
            yield return null;
        }
        transform.localScale = target;

    }
}
