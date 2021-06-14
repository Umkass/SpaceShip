using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField]
    private ProjectileMove projectilePrefab;
    [SerializeField]
    private Transform weaponMountPoint;
    private void Awake()
    {
        GetComponent<ShipInput>().OnFire += HandleFire;
    }
    void HandleFire()
    {
        Vector2 spawnPosition = weaponMountPoint.transform.position;
        ProjectileMove projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.AngleAxis(90, Vector3.forward));
        projectile.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        projectile.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        projectile.isPlayers = true;
        projectile.projectileSpeed = 5f;
        projectile.damage = 10;
        projectile.shipTransform = transform;
}
}
