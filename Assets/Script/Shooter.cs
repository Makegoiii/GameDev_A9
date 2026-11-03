using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Bullet bulletPrefab;
    public float cooldown = 0.3f;
    public float muzzleDistance = 1f;
    public float muzzleHeight = 0.5f;
    public AudioClip shootClip;

    public bool useMovementDirection = true;

    float nextShot;
    AudioSource src;

    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    public void Shoot(Vector3 moveDir)
    {
        if (Time.time < nextShot) return;

        Vector3 aimDir = useMovementDirection ? moveDir : transform.forward;
        aimDir.y = 0f;
        if (aimDir.sqrMagnitude < 0.0001f) aimDir = Vector3.forward;
        aimDir.Normalize();

        nextShot = Time.time + cooldown;

        Vector3 pos = transform.position + aimDir * muzzleDistance + Vector3.up * muzzleHeight;
        Bullet b = Instantiate(bulletPrefab, pos, Quaternion.LookRotation(aimDir));
        b.Launch(aimDir);

        if (src != null && shootClip != null) src.PlayOneShot(shootClip);
    }
}
