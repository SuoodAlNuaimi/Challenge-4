using UnityEngine;

public class EnemyX : MonoBehaviour
{
    private enum AIState { Attack, Intercept, Score }

    [Header("AI")]
    [SerializeField] private float steeringStrength = 0.2f;
    [SerializeField] private float steeringSpeed = 2f;

    public EnemyType enemyType;

    private Rigidbody rb;
    private Rigidbody playerRb;
    private Transform player;
    private Transform playerGoal;

    private AIState currentState;
    private float currentSpeed;
    private float decisionRate;
    private float decisionTimer;

    private bool isFrozen;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerRb = player.GetComponent<Rigidbody>();
        playerGoal = GameObject.Find("Player Goal").transform;

        SpawnManagerX spawner = FindObjectOfType<SpawnManagerX>();

        float baseSpeed = Random.Range(3f, 4.5f);
        currentSpeed = Mathf.Min(
            baseSpeed + (spawner.CurrentWave * GameSettings.EnemySpeedMultiplier),
            GameSettings.EnemyMaxSpeed
        );

        decisionRate = GameSettings.EnemyDecisionRate;
    }

    private void FixedUpdate()
    {
        if (isFrozen) return;

        decisionTimer += Time.fixedDeltaTime;
        if (decisionTimer >= decisionRate)
        {
            DecideState();
            decisionTimer = 0f;
        }

        ExecuteState();
    }

    public void Freeze(bool value)
    {
        isFrozen = value;

        if (value)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void DecideState()
    {
        float distGoal = Vector3.Distance(transform.position, playerGoal.position);

        if (distGoal < 7f)
        {
            currentState = AIState.Score;
            return;
        }

        currentState = enemyType switch
        {
            EnemyType.Aggressive => AIState.Attack,
            EnemyType.Defensive => AIState.Intercept,
            EnemyType.Evasive =>
                Vector3.Distance(transform.position, player.position) < 5f
                ? AIState.Intercept : AIState.Attack,
            _ => AIState.Attack
        };
    }

    private void ExecuteState()
    {
        Vector3 target = currentState switch
        {
            AIState.Attack => player.position,
            AIState.Intercept => PredictPosition(),
            AIState.Score => playerGoal.position,
            _ => player.position
        };

        Move(target);
    }

    private Vector3 PredictPosition()
    {
        return player.position + playerRb.linearVelocity *
               GameSettings.InterceptPredictionMultiplier;
    }

    private void Move(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;

        Vector3 steering = new Vector3(
            Mathf.Sin(Time.time * steeringSpeed) * steeringStrength,
            0,
            Mathf.Cos(Time.time * steeringSpeed) * steeringStrength
        );

        rb.AddForce((direction + steering).normalized * currentSpeed);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player Goal")
        {
            UIManager.Instance.AddEnemyScore();
            Destroy(gameObject);
        }
        else if (other.gameObject.name == "Enemy Goal")
        {
            UIManager.Instance.AddPlayerScore();
            Destroy(gameObject);
        }
    }
}