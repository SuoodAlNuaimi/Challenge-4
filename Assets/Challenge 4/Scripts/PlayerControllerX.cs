using System.Collections;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveForce = 500f;
    [SerializeField] private float turboBoostForce = 10f;

    [Header("Power Settings")]
    [SerializeField] private float powerupDuration = 5f;
    [SerializeField] private float freezeDuration = 3f;

    [Header("Hit Forces")]
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

    [Header("Reference")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform focalPoint;

    private bool hasNormalPower;
    private bool hasSmashPower;
    private bool isSmashing;

    private float normalTime;
    private float smashTime;

    private void Awake()
    {
        if(rb==null)
            rb = GetComponent<Rigidbody>();
        if(focalPoint==null)
            focalPoint = GameObject.Find("Focal Point").transform;
    }

    private void Start()
    {
        powerupIndicator.SetActive(false);
        smashShield.SetActive(false);
    }

    public void OnGameStart()
    {
        moveForce = GameSettings.PlayerMoveForce;
        turboBoostForce = GameSettings.TurboForce;
        powerupDuration = GameSettings.PowerupDuration;
        smashRadius *= GameSettings.SmashRadiusMultiplier;
    }

    private void Update()
    {
        HandleMovement();
        HandleInput();
        UpdateVisuals();
        UpdatePowerTimers();
    }

    private void HandleMovement()
    {
        float input = Input.GetAxis("Vertical");
        rb.AddForce(focalPoint.forward * input * moveForce * Time.deltaTime);
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(focalPoint.forward * turboBoostForce, ForceMode.Impulse);
            turboSmoke?.Play();
        }

        if (hasSmashPower && Input.GetKeyDown(KeyCode.F) && !isSmashing)
            StartCoroutine(SmashAttack());
    }

    private void UpdateVisuals()
    {
        if (powerupIndicator.activeSelf)
            powerupIndicator.transform.position = transform.position + Vector3.down * 0.6f;

        if (smashShield.activeSelf)
        {
            float scale = 1.8f + Mathf.Sin(Time.time * 5f) * 0.1f;
            smashShield.transform.localScale = Vector3.one * scale;
        }
    }

    private void UpdatePowerTimers()
    {
        if (hasNormalPower)
        {
            normalTime -= Time.deltaTime;
            UIManager.Instance.UpdateNormalPowerSlider(normalTime / powerupDuration);

            if (normalTime <= 0f)
                DisableNormalPower();
        }

        if (hasSmashPower)
        {
            smashTime -= Time.deltaTime;
            UIManager.Instance.UpdateSmashPowerSlider(smashTime / powerupDuration);

            if (smashTime <= 0f)
                DisableSmashPower();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
            ActivateNormalPower();

        if (other.CompareTag("SmashPowerup"))
            ActivateSmashPower();

        if (other.CompareTag("FreezePowerup"))
            StartCoroutine(FreezeEnemies());

        if (other.CompareTag("Powerup") ||
            other.CompareTag("SmashPowerup") ||
            other.CompareTag("FreezePowerup"))
        {
            SoundsController.Instance.PlayPowerup();
            Destroy(other.gameObject);
        }
    }

    private void ActivateNormalPower()
    {
        hasNormalPower = true;
        normalTime = powerupDuration;
        powerupIndicator.SetActive(true);
        UIManager.Instance.SetNormalPowerStatus(true);
    }

    private void DisableNormalPower()
    {
        hasNormalPower = false;
        powerupIndicator.SetActive(false);
        UIManager.Instance.SetNormalPowerStatus(false);
    }

    private void ActivateSmashPower()
    {
        hasSmashPower = true;
        smashTime = powerupDuration;
        smashShield.SetActive(true);
        UIManager.Instance.SetSmashPowerStatus(true);
    }

    private void DisableSmashPower()
    {
        hasSmashPower = false;
        smashShield.SetActive(false);
        UIManager.Instance.SetSmashPowerStatus(false);
    }

    private IEnumerator FreezeEnemies()
    {
        UIManager.Instance.ShowFreezeHint(true);

        EnemyX[] enemies = FindObjectsOfType<EnemyX>();
        foreach (var e in enemies) e.Freeze(true);

        yield return new WaitForSeconds(freezeDuration);

        foreach (var e in enemies) e.Freeze(false);

        UIManager.Instance.ShowFreezeHint(false);
    }

    private IEnumerator SmashAttack()
    {
        isSmashing = true;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.up * smashJumpForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.4f);

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.down * smashDownForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.2f);

        turboSmoke.gameObject.SetActive(true);
        turboSmoke.Play();

        ApplySmashImpact();
        SoundsController.Instance.PlayLand();

        isSmashing = false;
    }

    private void ApplySmashImpact()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, smashRadius);

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            Rigidbody enemyRb = hit.GetComponent<Rigidbody>();
            if (enemyRb == null) continue;

            float distance = Vector3.Distance(transform.position, hit.transform.position);
            float forcePercent = 1f - (distance / smashRadius);
            float finalForce = smashMaxForce * Mathf.Clamp01(forcePercent);

            enemyRb.AddExplosionForce(finalForce, transform.position, smashRadius);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
        if (enemyRb == null) return;

        Vector3 dir = (collision.transform.position - transform.position).normalized;
        float force = hasNormalPower ? poweredHitForce : normalHitForce;

        enemyRb.AddForce(dir * force, ForceMode.Impulse);
        SoundsController.Instance.PlayBallHit();
    }
}