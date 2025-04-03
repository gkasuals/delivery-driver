using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 200;
    [SerializeField] private float moveSpeed = 15;
    [SerializeField] float boostSpeedRatio = 1.5f;
    [SerializeField] float slowSpeedRatio = 0.5f;
    float boostSpeed;
    float slowSpeed;
    void Start()
    {
        slowSpeed = moveSpeed * slowSpeedRatio;
        boostSpeed = moveSpeed * boostSpeedRatio;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Boost"))
        {
            moveSpeed = boostSpeed;
            Debug.Log("boost");
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        moveSpeed = slowSpeed;
    }
    void Update()
    {
        float turnAmount = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        float moveAmount = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Rotate(0, 0, -turnAmount);
        transform.Translate(0, moveAmount, 0);
    }
}
