using System;
using UnityEngine;

public class Drift : MonoBehaviour
{
    [SerializeField] float acceleration = 20f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float steering = 3f;
    [SerializeField] float driftFactor = 0.95f;
    [SerializeField] float boostAccelerationRatio = 1.5f;
    [SerializeField] float slowAccelerationRatio = 0.5f;

    [SerializeField] float brakeSlowdownFactor = 0.97f; // ← 브레이크 감속 비율
    [SerializeField] ParticleSystem smoke;

    [SerializeField] TrailRenderer leftTrail;
    [SerializeField] TrailRenderer rightTrail;

    float defaultAcceleration;
    float boostAcceleration;

    Rigidbody2D rb;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = rb.GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        float speed = Vector2.Dot(rb.linearVelocity, transform.up);

        if (Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity *= brakeSlowdownFactor;
            
        }
        else if (speed < maxSpeed)
        {
            rb.AddForce(transform.up * Input.GetAxis("Vertical") * acceleration);
            
        }

        float turnAmount = Input.GetAxis("Horizontal") * steering * Mathf.Clamp(speed / maxSpeed, 0.4f, 1f);
        rb.MoveRotation(rb.rotation - turnAmount);

        // Drift
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 sideVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);

        rb.linearVelocity = forwardVelocity + (sideVelocity * driftFactor);
    }

    void Update()
    {
        float sidewayVelocity = Vector2.Dot(rb.linearVelocity, transform.right);
        bool isDrift = rb.linearVelocity.magnitude > 6f && MathF.Abs(sidewayVelocity) > 6f;

        if (isDrift)
        {
            if (!audioSource.isPlaying) audioSource.Play();
            if (!smoke.isPlaying) smoke.Play();
        }
        else
        {
            if (audioSource.isPlaying) audioSource.Stop();
            if (smoke.isPlaying) smoke.Stop();

        }

        leftTrail.emitting = isDrift;
        rightTrail.emitting = isDrift;
    }
}
