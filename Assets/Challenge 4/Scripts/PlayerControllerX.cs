using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;
    private GameObject focalPoint;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;

    public GameObject smashShield;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup

    private float turboBoost = 10;

    [SerializeField] private float smashJumpForce = 15f;
    [SerializeField] private float smashDownForce = 30f;
    [SerializeField] private float smashRadius = 20f;
    [SerializeField] private float smashForce = 40f;

    private bool hasSmashPowerup = false;
    private bool isSmashing = false;

    public ParticleSystem turboSmoke;
    
    void Start()
    {
        smashShield.SetActive(false);
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

        
    }

    void Update()
    {

        if (smashShield.activeSelf)
        {
            float scale = 1.8f + Mathf.Sin(Time.time * 5f) * 0.1f;
            smashShield.transform.localScale = new Vector3(scale, scale, scale);
        }

        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime); 

        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

        if (Input.GetKeyDown(KeyCode.Space)) {
            playerRb.AddForce(focalPoint.transform.forward * turboBoost, ForceMode.Impulse);
            
            turboSmoke.Play(true);
            
        }

        if (hasSmashPowerup && Input.GetKeyDown(KeyCode.F) && !isSmashing)
        {
            StartCoroutine(SmashAttack());
        }

    }



    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("SmashPowerup"))
        {
            smashShield.SetActive(true);
            hasSmashPowerup = true;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer =  other.gameObject.transform.position - transform.position; 
           
            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }


        }
    }

    IEnumerator SmashAttack()
    {
        isSmashing = true;

        // Jump up
        playerRb.AddForce(Vector3.up * smashJumpForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.4f);

        // Slam down
        playerRb.AddForce(Vector3.down * smashDownForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f);

        // Detect nearby enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, smashRadius);

        foreach (Collider enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();

                if (enemyRb != null)
                {
                    enemyRb.AddExplosionForce(smashForce, transform.position, smashRadius, 1f, ForceMode.Impulse);
                }
            }
        }

        hasSmashPowerup = false;
        isSmashing = false;
        smashShield.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, smashRadius);
    }



}
