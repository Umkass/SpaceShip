using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInput : MonoBehaviour
{
    #region Field Declarations
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public bool FireWeapons { get; private set; }
    #endregion
    public event Action OnFire = delegate { };
    void Update()
    {
        if(GameManager.Instance._currentGameState != GameManager.GameState.PAUSED)
        {
            Horizontal = Input.GetAxis("Horizontal");
            Vertical = Input.GetAxis("Vertical");
            FireWeapons = Input.GetButtonDown("Fire1");
            if (FireWeapons)
                OnFire();
        }
    }
}
