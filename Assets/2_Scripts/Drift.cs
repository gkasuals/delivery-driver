using System;
using UnityEngine;

public class Drift : MonoBehaviour
{
    [SerializeField] float accleration = 20f; //전-후진 가속도
    [SerializeField] float maxSpeed = 10f; //최대속도
    [SerializeField] float steering = 3f; //스티어링
    [SerializeField] float driftFactor = 0.95f; //값이 낮으면 더 미끄러짐
    [SerializeField] ParticleSystem smokeLeft;
    [SerializeField] ParticleSystem smokeRight;

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



    private void Update()
    {
        float sidewayVelocity = Vector2.Dot(rb.linearVelocity, transform.right);
        bool isDrift = rb.linearVelocity.magnitude > 2f && MathF.Abs(sidewayVelocity) > 1f;
        if (isDrift)
        {
            if (!audioSource.isPlaying) audioSource.Play();
            if (!smokeLeft.isPlaying) smokeLeft.Play();
            if (!smokeRight.isPlaying) smokeRight.Play();
        }
        else
        {
            if (audioSource.isPlaying) audioSource.Stop();
            if (smokeLeft.isPlaying)  smokeLeft.Stop(); 
            if (smokeRight.isPlaying)  smokeRight.Stop(); 
        }
    }
}