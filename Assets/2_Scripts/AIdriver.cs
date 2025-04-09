using System.Collections.Generic;
using UnityEngine;

public class PlayerChaserAI : MonoBehaviour
{
    [SerializeField] float acceleration = 20f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float steeringSpeed = 200f;
    [SerializeField] float driftFactor = 0.95f;
    [SerializeField] Transform player;
    [SerializeField] float delayTime = 0.05f;

    [Header("회피 설정")]
    [SerializeField] float obstacleCheckDistance = 2f;
    [SerializeField] float avoidDuration = 0.5f;
    [SerializeField] LayerMask obstacleMask;

    Rigidbody2D rb;
    List<Vector3> playerPositions = new List<Vector3>();
    float recordInterval = 0.02f;
    float recordTimer = 0f;

    bool isAvoiding = false;
    float avoidTimer = 0f;
    int avoidDirection = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        // 회피 중이 아니면 플레이어 위치 기록
        if (!isAvoiding)
        {
            recordTimer += Time.deltaTime;
            if (recordTimer >= recordInterval)
            {
                recordTimer = 0f;
                playerPositions.Add(player.position);

                float totalTime = playerPositions.Count * recordInterval;
                while (totalTime > delayTime && playerPositions.Count > 1)
                {
                    playerPositions.RemoveAt(0);
                    totalTime = playerPositions.Count * recordInterval;
                }
            }

            // 장애물 감지
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, obstacleCheckDistance, obstacleMask);
            if (hit.collider != null)
            {
                isAvoiding = true;
                avoidTimer = avoidDuration;
                avoidDirection = Random.value > 0.5f ? 1 : -1;
            }
        }
    }

    void FixedUpdate()
    {
        if (player == null || (!isAvoiding && playerPositions.Count == 0)) return;

        Vector2 moveDirection;

        if (isAvoiding)
        {
            // 회피 방향으로 이동
            moveDirection = (Vector2)(Quaternion.Euler(0, 0, 90f * avoidDirection) * transform.up);
            avoidTimer -= Time.fixedDeltaTime;
            if (avoidTimer <= 0f) isAvoiding = false;
        }
        else
        {
            // 플레이어 방향으로 이동
            Vector2 targetPos = playerPositions[0];
            moveDirection = (targetPos - (Vector2)transform.position).normalized;
        }

        // 회전
        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
        float angle = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, steeringSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(angle);

        // 가속
        float speed = Vector2.Dot(rb.linearVelocity, transform.up);
        if (speed < maxSpeed)
        {
            rb.AddForce(transform.up * acceleration);
        }

        // 드리프트 적용
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 sideVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);
        rb.linearVelocity = forwardVelocity + (sideVelocity * driftFactor);
    }
}
