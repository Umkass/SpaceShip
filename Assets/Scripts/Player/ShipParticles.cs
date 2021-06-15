using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipParticles : MonoBehaviour
{
    [SerializeField]
    private GameObject deathParticleSystemPrefab;
    void Awake()
    {
        if (GetComponent<ShipHealth>() != null)
            GetComponent<ShipHealth>().OnDie += HandleShipDeath;
    }
    void HandleShipDeath()
    {
        Instantiate(deathParticleSystemPrefab, transform.position, Quaternion.identity);
    }
}
