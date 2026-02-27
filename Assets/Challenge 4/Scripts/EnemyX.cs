using UnityEngine;

public class EnemyX : MonoBehaviour
{
   
    [Header("AI Settings")]
    public EnemyType enemyType;

    private Rigidbody enemyRb;
    private GameObject playerGoal;
    private GameObject player;

    private float baseSpeed;
    private float currentSpeed;

    private void Start()
    {
        enemyRb = GetComponent<Rigidbody>();

        playerGoal = GameObject.Find("Player Goal");
        player = GameObject.FindGameObjectWithTag("Player");

        baseSpeed = Random.Range(2f, 4f);

        // Wave scaling
        int wave = FindObjectOfType<SpawnManagerX>().CurrentWave;
        currentSpeed = baseSpeed + (wave * 0.5f);
    }

    private void FixedUpdate()
    {
        HandleBehavior();
    }

    private void HandleBehavior()
    {
        switch (enemyType)
        {
            case EnemyType.Aggressive:
                MoveTowards(player.transform.position);
                break;

            case EnemyType.Defensive:
                MoveTowards(playerGoal.transform.position);
                break;

            case EnemyType.Evasive:
                MoveEvasive();
                break;
        }
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        enemyRb.AddForce(direction * currentSpeed, ForceMode.Force);
    }

    private void MoveEvasive()
    {
        Vector3 toPlayer = (player.transform.position - transform.position).normalized;

        // Move sideways relative to player
        Vector3 sideDirection = Vector3.Cross(toPlayer, Vector3.up);

        enemyRb.AddForce((toPlayer + sideDirection).normalized * currentSpeed, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Enemy Goal" ||
            other.gameObject.name == "Player Goal")
        {
            Destroy(gameObject);
        }
    }
}