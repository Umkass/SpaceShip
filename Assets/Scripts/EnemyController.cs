using System.Collections;
using UnityEngine;

public delegate void EnemyDestroyedHandler(int pointValue);
public class EnemyController : MonoBehaviour
{
    #region Field Declarations

    [Header("Prefabs")]
    public GameObject explosion;
    public ProjectileMove projectilePrefab;

    // Set by GameSceneController
    [HideInInspector] public float shotSpeed;
    [HideInInspector] public float shotdelayTime;
    [HideInInspector] public float angerdelayTime;
    [HideInInspector] public float speed;
    [HideInInspector] public int pointValue = 10;

    private WaitForSeconds shotDelay;
    private WaitForSeconds angerDelay;
    private float shotSpeedxN;
    private GameObject targetShip;
    private Transform currentTarget;
    private SpriteRenderer spriteRenderer;

    #endregion
    public event EnemyDestroyedHandler EnemyDestroyed;
    #region Startup

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetShip = GameObject.FindGameObjectWithTag("Player");
        currentTarget = targetShip.GetComponent<Transform>();
        shotDelay = new WaitForSeconds(shotdelayTime);
        angerDelay = new WaitForSeconds(angerdelayTime);
        shotSpeedxN = shotSpeed * 1.5f;

        StartCoroutine(AngerCountDown());
        StartCoroutine(OpenFire());
    }

    #endregion

    #region Movement

    // Update is called once per frame
    private void Update()
    {
        Vector3 direction = currentTarget.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg - 90);
        Move();
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, currentTarget.position) > 0.5f)
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, Time.deltaTime * speed);
    }

    #endregion

    #region Collisons

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
        FindObjectOfType<HUDController>().UpdateScore(pointValue);
        GameObject xPlosion = Instantiate(explosion, transform.position, Quaternion.identity);
        xPlosion.transform.localScale = new Vector2(2, 2);
        if (EnemyDestroyed != null)
            EnemyDestroyed(pointValue);
        Destroy(gameObject);
    }

    #endregion

    #region Projectile control

    private void FireProjectile()
    {
        Vector2 spawnPosition = transform.position;

        ProjectileMove projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.AngleAxis(90, Vector3.forward));

        projectile.gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        projectile.projectileSpeed = shotSpeed;
        projectile.shipTransform = transform;
        projectile.damage = 10;
    }

    IEnumerator OpenFire()
    {
        while (true)
        {
            FireProjectile();
            yield return shotDelay;
        }
    }

    #endregion

    #region Anger management

    IEnumerator AngerCountDown()
    {
        yield return angerDelay;
        GetAngry();
    }

    private void GetAngry()
    {
        spriteRenderer.color = Color.red;
        currentTarget.position = ScreenBounds.GetRandomPosition();
        shotDelay = new WaitForSeconds(shotdelayTime / 3);
        shotSpeed = shotSpeedxN;
    }

    #endregion
}
