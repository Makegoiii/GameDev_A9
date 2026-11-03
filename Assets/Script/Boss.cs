using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    [Header("Денсаулық")]
    public int maxHp = 15;
    public RectTransform hpFill;
    public Slider hpBar;

    [Header("Сілтемелер")]
    public Transform player;
    public Transform firePoint;
    public Transform arenaCenter;
    public GameObject fireballPrefab;

    [Header("Баптау")]
    public float arenaRadius = 9f;
    public AudioClip hitClip;

    int hp;
    bool dead;
    Renderer[] rends;

    void Start()
    {
        hp = maxHp;
        rends = GetComponentsInChildren<Renderer>();
        UpdateBar();
        StartCoroutine(AttackLoop());
    }

    void Update()
    {
        if (dead || player == null) return;

        Vector3 look = player.position;
        look.y = transform.position.y;
        transform.LookAt(look);
    }

    void OnTriggerEnter(Collider c)    { CheckBullet(c.gameObject); }
    void OnCollisionEnter(Collision c) { CheckBullet(c.gameObject); }

    void CheckBullet(GameObject obj)
    {
        if (obj.GetComponent<Bullet>() == null) return;
        Destroy(obj);
        TakeDamage(1);
    }

    public void TakeDamage(int dmg)
    {
        if (dead) return;

        hp -= dmg;
        UpdateBar();
        if (hitClip) AudioSource.PlayClipAtPoint(hitClip, transform.position);

        if (hp <= 0)
        {
            dead = true;
            StopAllCoroutines();
            foreach (var r in rends) r.enabled = true;
            Invoke(nameof(Win), 1f);
        }
        else
        {
            StartCoroutine(Flash());
        }
    }

    void UpdateBar()
    {
        float k = Mathf.Clamp01((float)hp / maxHp);
        if (hpFill) hpFill.localScale = new Vector3(k, 1f, 1f);
        if (hpBar) hpBar.value = k;
    }

    IEnumerator Flash()
    {
        for (int i = 0; i < 4; i++)
        {
            foreach (var r in rends) r.enabled = !r.enabled;
            yield return new WaitForSeconds(0.06f);
        }
        foreach (var r in rends) r.enabled = true;
    }

    void Win()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(2f);

        while (!dead)
        {
            bool phase2 = hp <= maxHp / 2;

            if (Random.value < 0.5f)
                yield return StartCoroutine(Fan(phase2 ? 5 : 3));
            else
                yield return StartCoroutine(Rain(phase2 ? 5 : 3));

            yield return new WaitForSeconds(phase2 ? 1.2f : 2f);
        }
    }

    IEnumerator Fan(int n)
    {
        Vector3 to = player.position - firePoint.position;
        to.y = 0f;
        if (to.sqrMagnitude < 0.01f) to = transform.forward;
        Quaternion baseRot = Quaternion.LookRotation(to);

        for (int i = 0; i < n; i++)
        {
            float angle = (i - (n - 1) / 2f) * 15f;
            Instantiate(fireballPrefab, firePoint.position,
                        baseRot * Quaternion.Euler(0f, angle, 0f));
        }
        yield return null;
    }

    IEnumerator Rain(int n)
    {
        Vector3 c = arenaCenter ? arenaCenter.position : Vector3.zero;
        int count = n * 2;

        for (int i = 0; i < count; i++)
        {
            Vector2 r = Random.insideUnitCircle * arenaRadius;
            Vector3 p = c + new Vector3(r.x, Random.Range(9f, 13f), r.y);

            Instantiate(fireballPrefab, p, Quaternion.Euler(90f, 0f, 0f));
            yield return new WaitForSeconds(Random.Range(0.05f, 0.25f));
        }
    }
}
