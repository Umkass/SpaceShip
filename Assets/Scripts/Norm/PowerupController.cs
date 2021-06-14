using UnityEngine;

public class PowerupController : MonoBehaviour
{
    #region Field Declarations

    public GameObject explosion;

    public PowerType powerType;

    #endregion

    #region Movement

    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector2.down * Time.deltaTime * 3, Space.World);

        if (ScreenBounds.OutOfBounds(transform.position))
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Collisons

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (powerType == PowerType.Shield)
        {
            if (other.collider.GetComponent<ShieldController>())
            {
                ShieldController playerShield = other.gameObject.GetComponent<ShieldController>();
                playerShield.EnableShield();
            }
        }
        if (powerType == PowerType.X2)
        {

        }

        Destroy(gameObject);
    }

    #endregion
}

public enum PowerType
{
    Shield,
    X2
};