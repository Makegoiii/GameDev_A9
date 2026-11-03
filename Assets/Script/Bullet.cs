using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 12f;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Launch(Vector3 direction)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearVelocity = direction.normalized * speed;
    }
}
