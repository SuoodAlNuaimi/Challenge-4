using UnityEngine;

public class EnemyX : MonoBehaviour
{
    private enum AIState
    {
        Attack,
        Intercept,
        Score
    }

    [Header("AI Personality")]
    public EnemyType enemyType;

    private Rigidbody rb;
    private Rigidbody playerRb;

    private Transform player;
    private Transform playerGoal;

    private AIState currentState;

    private float baseSpeed;
    private float currentSpeed;

    private float decisionTimer;
    private float decisionRate = 0.25f;

    private SpawnManagerX spawnManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerRb = player.GetComponent<Rigidbody>();

        playerGoal = GameObject.Find("Player Goal").transform;

        spawnManager = FindObjectOfType<SpawnManagerX>();

        baseSpeed = Random.Range(3f, 4.5f);
        // apply according to the game type and current wave
        currentSpeed = Mathf.Min(
            baseSpeed + (spawnManager.CurrentWave * GameSettings.EnemySpeedMultiplier),
            GameSettings.EnemyMaxSpeed
        );

        decisionRate = GameSettings.EnemyDecisionRate;

        currentState = AIState.Attack;
    }

    private void FixedUpdate()
    {
        decisionTimer += Time.fixedDeltaTime;

        if (decisionTimer >= decisionRate)
        {
            DecideState();
            decisionTimer = 0f;
        }

        ExecuteState();
    }

    private void DecideState()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        float distToGoal = Vector3.Distance(transform.position, playerGoal.position);

        // Close to goal → try to score
        if (distToGoal < 7f)
        {
            currentState = AIState.Score;
            return;
        }

        switch (enemyType)
        {
            case EnemyType.Aggressive:
                currentState = AIState.Attack;
                break;

            case EnemyType.Defensive:
                currentState = AIState.Intercept;
                break;

            case EnemyType.Evasive:
                currentState = distToPlayer < 5f ? AIState.Intercept : AIState.Attack;
                break;
        }
    }

    private void ExecuteState()
    {
        switch (currentState)
        {
            case AIState.Attack:
                MoveSmart(player.position);
                break;

            case AIState.Intercept:
                Vector3 predictedPos = PredictPlayerPosition();
                MoveSmart(predictedPos);
                break;

            case AIState.Score:
                MoveSmart(playerGoal.position);
                break;
        }
    }

    private Vector3 PredictPlayerPosition()
    {
        // Predict where player will be shortly
        Vector3 futurePos = player.position + playerRb.linearVelocity * GameSettings.InterceptPredictionMultiplier;
        return futurePos;
    }

    private void MoveSmart(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;

        // Slight natural steering
        Vector3 steeringOffset = new Vector3(
            Mathf.Sin(Time.time * 2f) * 0.2f,
            0,
            Mathf.Cos(Time.time * 2f) * 0.2f
        );

        Vector3 finalDir = (direction + steeringOffset).normalized;

        rb.AddForce(finalDir * currentSpeed, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player Goal")
        {
            UIManager.Instance.AddEnemyScore();
            Destroy(gameObject);
        }
        if (other.gameObject.name == "Enemy Goal")
        {
            UIManager.Instance.AddPlayerScore();
            Destroy(gameObject);
        }
    }
}