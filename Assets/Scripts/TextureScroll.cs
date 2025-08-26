using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeedX = 0.2f;
    [SerializeField] private float scrollSpeedY = 0.0f;

    private Renderer rend;
    private Vector2 offset = Vector2.zero;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        offset.x += scrollSpeedX * Time.deltaTime;
        offset.y += scrollSpeedY * Time.deltaTime;

        rend.material.mainTextureOffset = offset;
    }
}
