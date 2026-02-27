using System.Collections;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveForce = 500f;
    [SerializeField] private float turboBoostForce = 10f;

    [Header("Powerup Settings")]
    [SerializeField] private float powerupDuration = 5f;
    [SerializeField] private float normalHitForce = 10f;
    [SerializeField] private float poweredHitForce = 25f;

    [Header("Smash Settings")]
    [SerializeField] private float smashJumpForce = 15f;
    [SerializeField] private float smashDownForce = 30f;
    [SerializeField] private float smashRadius = 20f;
    [SerializeField] private float smashMaxForce = 40f;

    [Header("References")]
    [SerializeField] private GameObject powerupIndicator;
    [SerializeField] private GameObject smashShield;
    [SerializeField] private ParticleSystem turboSmoke;

    private Rigidbody rb;
    private GameObject focalPoint;

    private bool hasNormalPowerup;
    private bool hasSmashPowerup;
    private bool isSmashing;

    private Coroutine normalPowerupRoutine;
    private Coroutine smashPowerupRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    private void Start()
    {
        smashShield.SetActive(false);
        powerupIndicator.SetActive(false);
    }

    private void Update()
    {
        HandleMovement();
        HandleInput();
        UpdateVisuals();
    }

    private void HandleMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        rb.AddForce(focalPoint.transform.forward * verticalInput * moveForce * Time.deltaTime);
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(focalPoint.transform.forward * turboBoostForce, ForceMode.Impulse);
            turboSmoke?.Play();
        }

        if (hasSmashPowerup && Input.GetKeyDown(KeyCode.F) && !isSmashing)
        {
            StartCoroutine(SmashAttack());
        }
    }

    private void UpdateVisuals()
    {
        if (powerupIndicator.activeSelf)
            powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

        if (smashShield.activeSelf)
        {
            float scale = 1.8f + Mathf.Sin(Time.time * 5f) * 0.1f;
            smashShield.transform.localScale = Vector3.one * scale;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            ActivateNormalPowerup();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("SmashPowerup"))
        {
            ActivateSmashPowerup();
            Destroy(other.gameObject);
        }
    }

    private void ActivateNormalPowerup()
    {
        if (normalPowerupRoutine != null)
            StopCoroutine(normalPowerupRoutine);

        hasNormalPowerup = true;
        powerupIndicator.SetActive(true);
        normalPowerupRoutine = StartCoroutine(PowerupTimer(() =>
        {
            hasNormalPowerup = false;
            powerupIndicator.SetActive(false);
        }));
    }

    private void ActivateSmashPowerup()
    {
        if (smashPowerupRoutine != null)
            StopCoroutine(smashPowerupRoutine);

        hasSmashPowerup = true;
        smashShield.SetActive(true);

        smashPowerupRoutine = StartCoroutine(PowerupTimer(() =>
        {
            hasSmashPowerup = false;
            smashShield.SetActive(false);
        }));
    }

    private IEnumerator PowerupTimer(System.Action onEnd)
    {
        yield return new WaitForSeconds(powerupDuration);
        onEnd?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
        if (enemyRb == null) return;

        Vector3 direction = collision.transform.position - transform.position;
        float force = hasNormalPowerup ? poweredHitForce : normalHitForce;

        enemyRb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }

    private IEnumerator SmashAttack()
    {
        isSmashing = true;

        rb.AddForce(Vector3.up * smashJumpForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.4f);

        rb.AddForce(Vector3.down * smashDownForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.2f);
        turboSmoke.Play();
        ApplySmashImpact();

        hasSmashPowerup = false;
        smashShield.SetActive(false);
        isSmashing = false;
    }

    private void ApplySmashImpact()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, smashRadius);

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            Rigidbody enemyRb = hit.GetComponent<Rigidbody>();
            if (enemyRb == null) continue;

            float distance = Vector3.Distance(transform.position, hit.transform.position);
            float forcePercent = 1 - (distance / smashRadius);

            float finalForce = smashMaxForce * Mathf.Clamp01(forcePercent);

            enemyRb.AddExplosionForce(finalForce, transform.position, smashRadius, 1f, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, smashRadius);
    }
}