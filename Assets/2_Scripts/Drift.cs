using UnityEngine;

public class Drift : MonoBehaviour
{
    [SerializeField] float accleration = 20f; //전-후진 가속도
    [SerializeField] float maxSpeed = 10f; //최대속도
    [SerializeField] float steering = 3f; //스티어링
    [SerializeField] float driftFactor = 0.95f; //값이 낮으면 더 미끄러짐
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float speed = Vector2.Dot(rb.linearVelocity, transform.up);
        if (speed < maxSpeed)
        {
            rb.AddForce(transform.up * Input.GetAxis("Vertical") * accleration);
        }
        //float turnAmount = Input.GetAxis("Horizontal") * steering * speed * Time.fixedDeltaTime;
        float turnAmount = Input.GetAxis("Horizontal") * steering * Mathf.Clamp(speed / maxSpeed, 0.4f, 1f);
        rb.MoveRotation(rb.rotation - turnAmount);

        //Drift
        Vector2 fowwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 sideVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);

        rb.linearVelocity = fowwardVelocity + (sideVelocity * driftFactor);
    }
}