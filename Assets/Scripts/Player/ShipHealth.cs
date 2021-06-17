using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    #region Field Declarations

    private int maxHealth = 100;
    [SerializeField] private int health;
    [SerializeField] private GameObject explosion;
    public HealthType healthType;
    public int pointValue = 10;
    private PowerUpManagament powerUpManagament;

    public Action DiedByEnemy = delegate { };
    public int Health { get => health; private set => health = value; }

    public event Action OnDie = delegate { };
    public event EnemyDestroyedHandler EnemyDestroyed;

    #endregion
    void Awake()
    {
        Health = maxHealth;
        OnDie += HandleDie;
    }

    void Update()
    {
        if (ScreenBounds.OutOfBounds(transform.position))
        {
            OnDie?.Invoke();
        }
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            powerUpManagament = GameObject.FindGameObjectWithTag("Player").GetComponent<PowerUpManagament>();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (healthType == HealthType.Player)
        {
            if (other.collider.GetComponent<ProjectileMove>() && !powerUpManagament.isShield)
            {
                var projectile = other.collider.GetComponent<ProjectileMove>();
                TakeDamage(projectile.damage);
                Vector2 explosionPos = other.transform.position;
                Destroy(other.gameObject);
                GameObject newSpark = Instantiate(this.explosion, explosionPos, Quaternion.identity);
                newSpark.transform.localScale = new Vector2(.75f, .75f);
            }
            else if (other.collider.GetComponent<ProjectileMove>() && powerUpManagament.isShield)
            {
                var projectile = other.collider.GetComponent<ProjectileMove>();
                Vector2 explosionPos = new Vector2(other.transform.position.x + 0.4f, other.transform.position.y + 0.4f);
                Destroy(other.gameObject);
                GameObject newSpark = Instantiate(this.explosion, explosionPos, Quaternion.identity);
                newSpark.transform.localScale = new Vector2(.75f, .75f);
            }
        }
        else
        {
            if (other.collider.GetComponent<ProjectileMove>())
            {
                var projectile = other.collider.GetComponent<ProjectileMove>();
                TakeDamage(projectile.damage);
                Vector2 explosionPos = other.transform.position;
                Destroy(other.gameObject);
                GameObject newSpark = Instantiate(this.explosion, explosionPos, Quaternion.identity);
                newSpark.transform.localScale = new Vector2(.75f, .75f);
            }
        }
    }
    private void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0 && healthType == HealthType.Player)
            OnDie?.Invoke();
        else if (Health <= 0 && healthType == HealthType.Enemy)
        {
            if (!powerUpManagament.isX2)
            {
                Destroy(gameObject);
                EnemyDestroyed?.Invoke(pointValue);
            }
            else if (powerUpManagament.isX2)
            {
                Destroy(gameObject);
                EnemyDestroyed?.Invoke(pointValue * 2);
            }
        }
    }
    private void HandleDie()
    {
        Destroy(gameObject);
        DiedByEnemy?.Invoke();
    }
}
public enum HealthType
{
    Enemy,
    Player
};