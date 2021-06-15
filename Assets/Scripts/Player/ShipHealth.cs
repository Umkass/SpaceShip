using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    #region Field Declarations

    [SerializeField]
    private int maxHealth = 100;
    private int health;
    [SerializeField] private GameObject explosion;

    public Action DiedByEnemy = delegate { };

    public event Action OnDie = delegate { };

    #endregion
    void Awake()
    {
        health = maxHealth;
        OnDie += HandleDie;
    }
    void Update()
    {
        if (ScreenBounds.OutOfBounds(transform.position))
        {
            OnDie?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
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
    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            OnDie?.Invoke();
    }
    private void HandleDie()
    {
        Destroy(gameObject);
        DiedByEnemy?.Invoke();
    }
}
