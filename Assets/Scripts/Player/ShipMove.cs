using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipInput))]
public class ShipMove : MonoBehaviour
{
    #region Field Declarations
    //Set by GameSceneController
    [HideInInspector] public float speed;
    [HideInInspector] public float turnSpeed;
    private ShipInput shipInput;

    #endregion
    void Awake()
    {
        shipInput = GetComponent<ShipInput>();
    }
    void Update()
    {
        MovePlayer();
    }

    #region Movement
    private void MovePlayer()
    {
        if (Mathf.Abs(shipInput.Horizontal) > Mathf.Epsilon || Mathf.Abs(shipInput.Vertical) > Mathf.Epsilon)
        {
            transform.position += Time.deltaTime * shipInput.Vertical * transform.up * speed;
            transform.Rotate(-Vector3.forward * shipInput.Horizontal * turnSpeed * Time.deltaTime);
        }
    }
    #endregion
}
