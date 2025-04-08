using System;
using UnityEngine;

public class Drift : MonoBehaviour
{
    [SerializeField] float acceleration = 20f; // 전-후진 가속도 
    [SerializeField] float maxSpeed = 10f; // 최대속도
    [SerializeField] float steering = 3f; // 스티어링
    [SerializeField] float driftFactor = 0.95f; // 값이 낮으면 더 미끄러짐
    [SerializeField] float boostAccelerationRatio = 1.5f;
    [SerializeField] float slowAccelerationRatio = 0.5f;

    [SerializeField] ParticleSystem smokeLeft;
    [SerializeField] ParticleSystem smokeRight;
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
        if (speed < maxSpeed)
        {
            rb.AddForce(transform.up * Input.GetAxis("Vertical") * acceleration);
        }

        float turnAmount = Input.GetAxis("Horizontal") * steering * Mathf.Clamp(speed / maxSpeed, 0.4f, 1f);
        rb.MoveRotation(rb.rotation - turnAmount);

        // Drift
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up); // fowward → forward
        Vector2 sideVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);

        rb.linearVelocity = forwardVelocity + (sideVelocity * driftFactor);
    }

    private void Update()
    {
        float sidewayVelocity = Vector2.Dot(rb.linearVelocity, transform.right);
        bool isDrift = rb.linearVelocity.magnitude > 2f && MathF.Abs(sidewayVelocity) > 1f;

        if (isDrift)
        {
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying) audioSource.Stop();
        }

        leftTrail.emitting = isDrift;
        rightTrail.emitting = isDrift;
    }
}
