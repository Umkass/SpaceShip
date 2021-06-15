using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public delegate void OutOfBoundsHandler();
    #region Field Declarations
    public float projectileSpeed;
    public bool isPlayers = false;
    public int damage;
    public Transform shipTransform;
    #endregion
    public event OutOfBoundsHandler ProjectileOutOfBounds;
    #region Movement


    void Start()
    {
        ProjectileDirection();
    }
    void Update()
    {
        MoveProjectile();
    }
    private void MoveProjectile()
    {
        transform.Translate(transform.right * Time.deltaTime * projectileSpeed, Space.World);

        if (ScreenBounds.OutOfBounds(transform.position))
        {
            if(isPlayers == true)
            {
                ProjectileOutOfBounds?.Invoke();
            }
            Destroy(gameObject);
        }
    }

    private void ProjectileDirection()
    {
        transform.rotation = Quaternion.Euler(shipTransform.eulerAngles.x, shipTransform.eulerAngles.y, shipTransform.eulerAngles.z + 90);
    }
    #endregion
}
