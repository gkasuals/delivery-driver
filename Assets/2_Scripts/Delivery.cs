using UnityEngine;

public class Delivery : MonoBehaviour
{
    bool hasChicken = false;
    [SerializeField] Color noChickenColor = new Color(1, 1, 1);
    [SerializeField] Color hasChickenColor = new Color(0, 0, 0);
    SpriteRenderer spriteRenderer;

    [SerializeField] float delay = 0.3f;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chicken") && !hasChicken)
        {
            Debug.Log("Ä¡Å²ÇÈ¾÷µÊ");
            hasChicken = true;
            spriteRenderer.color = hasChickenColor;
            Destroy(collision.gameObject, delay);
        }
        if (collision.gameObject.CompareTag("Customer") && hasChicken)
        {
            Debug.Log("Ä¡Å²¹è´ÞµÊ");
            spriteRenderer.color = noChickenColor;
            hasChicken = false;
        }
    }
}
