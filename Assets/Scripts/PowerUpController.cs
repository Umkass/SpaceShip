using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    #region Field Declarations

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
        if (other.collider.GetComponent<PowerUpManagament>())
        {
            PowerUpManagament playerPowerUp = other.gameObject.GetComponent<PowerUpManagament>();
            if (powerType == PowerType.Shield)
                playerPowerUp.EnableShield();
            else
                playerPowerUp.EnableX2();
        }
        else if (other.collider.GetComponentInParent<PowerUpManagament>())
        {
            PowerUpManagament playerPowerUp = other.gameObject.GetComponentInParent<PowerUpManagament>();
            if (powerType == PowerType.Shield)
                playerPowerUp.EnableShield();
            else
                playerPowerUp.EnableX2();
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