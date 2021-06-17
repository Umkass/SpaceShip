using System.Collections;
using UnityEngine;

public class PowerUpManagament : MonoBehaviour
{
    #region Field Declarations
    [SerializeField] private GameObject shield;
    //Set by GameSceneController
    [HideInInspector] public float shieldDuration;
    [HideInInspector] public float X2Duration;

    [HideInInspector] public bool isX2;
    [HideInInspector] public bool isShield;
    private WaitForSeconds shieldTimeOut;
    private WaitForSeconds X2TimeOut;

    #endregion
    private void Start()
    {
        shieldTimeOut = new WaitForSeconds(shieldDuration);
        X2TimeOut = new WaitForSeconds(X2Duration);
        EnableShield();
    }

    #region Shield Management

    public void EnableShield()
    {
        shield.SetActive(true);
        isShield = true;
        StartCoroutine(DisableShield());
    }
    private IEnumerator DisableShield()
    {
        yield return shieldTimeOut;
        shield.SetActive(false);
        isShield = false;
    }

    #endregion

    #region X2 Management

    public void EnableX2()
    {
        isX2 = true;
        StartCoroutine(DisableX2());
    }
    private IEnumerator DisableX2()
    {
        yield return X2TimeOut;
        isX2 = false;
    }

    #endregion
}
