using System.Collections;
using UnityEngine;

public delegate void EnemyDestroyedHandler(int pointValue);
public class EnemyController : MonoBehaviour
{
    #region Field Declarations

    [Header("Prefabs")]
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
    private Transform currentTarget;
    private Vector2 randomTarget;
    private SpriteRenderer spriteRenderer;
    #endregion
    #region Startup

    private void Start()
    {
        FindCurrentTarget();
        GameManager.Instance.ShipSpawned += FindCurrentTarget;
        spriteRenderer = GetComponent<SpriteRenderer>();
        randomTarget = ScreenBounds.GetRandomPosition();
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
        if (currentTarget != null)
        {
            Vector3 direction = currentTarget.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x);
            transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg - 90);
            Move();
        }
        else
        {
            MoveRandomly();
        }
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, currentTarget.position) > 0.5f)
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, Time.deltaTime * speed);
    }
    private void MoveRandomly()
    {
        if (Vector2.Distance(transform.position, randomTarget) > 0.5f)
            transform.position = Vector2.MoveTowards(transform.position, randomTarget, Time.deltaTime * speed);
        else
            randomTarget = ScreenBounds.GetRandomPosition();
    }

    public void FindCurrentTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            currentTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
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
        shotDelay = new WaitForSeconds(shotdelayTime / 3);
        shotSpeed = shotSpeedxN;
    }

    #endregion
}
