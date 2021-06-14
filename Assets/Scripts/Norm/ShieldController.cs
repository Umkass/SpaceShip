using System.Collections;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    #region Field Declarations
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject explosion;
    //Set by GameSceneController
    [HideInInspector] public float shieldDuration;
    [HideInInspector] public float X2Duration;

    [HideInInspector] public bool isX2;
    private WaitForSeconds shieldTimeOut;
    private WaitForSeconds X2TimeOut;

    #endregion
    private void Start()
    {
        shieldTimeOut = new WaitForSeconds(shieldDuration);
        X2TimeOut = new WaitForSeconds(X2Duration);
        EnableShield();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.GetComponent<ProjectileMove>())
        {
            Vector2 explosionPos = other.transform.position;
            Destroy(other.gameObject);
            GameObject newSpark = Instantiate(this.explosion, explosionPos, Quaternion.identity);
            newSpark.transform.localScale = new Vector2(.5f, .5f);
        }
        else if (other.collider.GetComponent<PowerupController>())
        {
            if (other.collider.GetComponent<PowerupController>().powerType == PowerType.Shield)
            {
                EnableShield();
            }
            else
            {
                EnableX2();
            }
        }
    }

    #region Shield Management

    public void EnableShield()
    {
        shield.SetActive(true);
        StartCoroutine(DisableShield());
    }
    private IEnumerator DisableShield()
    {
        yield return shieldTimeOut;
        shield.SetActive(false);
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
        yield return shieldTimeOut;
        isX2 = false;
    }

    #endregion
}
