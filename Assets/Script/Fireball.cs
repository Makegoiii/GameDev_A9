using UnityEngine;

public class Fireball : Hazard
{
    public float speed = 8f;
    public int damage = 1;

    protected override void Start()
    {
        base.Start();
        GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.GetComponentInParent<Boss>()) return;
        if (c.isTrigger) return;

        Health h = c.GetComponentInParent<Health>();
        if (h != null) OnHitPlayer(h);

        Destroy(gameObject);
    }

    protected override void OnHitPlayer(Health player)
    {
        player.TakeDamage(damage);
    }
}
