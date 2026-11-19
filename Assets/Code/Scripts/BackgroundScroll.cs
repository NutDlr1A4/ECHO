using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.01f;

    private RawImage image;

    void Awake()
    {
        image = GetComponent<RawImage>();
    }

    void Update()
    {
        Rect uv = image.uvRect;
        uv.x = Mathf.Repeat(uv.x + scrollSpeed * Time.unscaledDeltaTime, 1f);
        image.uvRect = uv;
    }
}