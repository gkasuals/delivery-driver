using System.Collections.Generic;
using UnityEngine;

public class PlayerChaserAI : MonoBehaviour
{
    [SerializeField] float acceleration = 20f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float steeringSpeed = 200f; // 빠르게 회전할 수 있도록
    [SerializeField] float driftFactor = 0.95f;
    [SerializeField] Transform player;
    [SerializeField] float delayTime = 0.05f; // 딜레이 시간

    Rigidbody2D rb;

    // 플레이어 위치 기록용
    List<Vector3> playerPositions = new List<Vector3>();
    float recordInterval = 0.02f;
    float recordTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        // 일정 간격으로 플레이어 위치 기록
        recordTimer += Time.deltaTime;
        if (recordTimer >= recordInterval)
        {
            recordTimer = 0f;
            playerPositions.Add(player.position);

            // 오래된 위치 제거
            float totalTime = playerPositions.Count * recordInterval;
            while (totalTime > delayTime && playerPositions.Count > 1)
            {
                playerPositions.RemoveAt(0);
                totalTime = playerPositions.Count * recordInterval;
            }
        }
    }

    void FixedUpdate()
    {
        if (player == null || playerPositions.Count == 0) return;

        // 📍 과거 위치를 목표로 설정
        Vector2 targetPos = playerPositions[0];
        Vector2 directionToPlayer = (targetPos - (Vector2)transform.position).normalized;

        // 목표 회전값 구하기
        float targetAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;

        // 회전
        float angle = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, steeringSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(angle);

        // 속도 제한 체크 후 가속
        float speed = Vector2.Dot(rb.linearVelocity, transform.up);
        if (speed < maxSpeed)
        {
            rb.AddForce(transform.up * acceleration);
        }

        // Drift 물리 적용
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 sideVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);
        rb.linearVelocity = forwardVelocity + (sideVelocity * driftFactor);
    }
}
