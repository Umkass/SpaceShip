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

    public Action HitByEnemy = delegate { };

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
        var projectile = other.collider.GetComponent<ProjectileMove>();
        if (projectile != null)
            TakeDamage(projectile.damage);
    }
    private void TakeDamage (int damage)
    {
        health -= damage;
        if (health <= 0)
            OnDie?.Invoke();
        HitByEnemy?.Invoke();
    }
    private void HandleDie()
    {
        Destroy(gameObject);
    }
}
