using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHp = 3;
    public int hp = 3;

    public AudioClip hurtClip;
    public AudioClip healClip;

    bool invincible;
    Renderer[] renderers;
    AudioSource src;

    void Start()
    {
        hp = maxHp;
        renderers = GetComponentsInChildren<Renderer>();
        src = GetComponent<AudioSource>();
        Debug.Log("HP: " + hp);
    }

    public void TakeDamage(int dmg)
    {
        if (invincible) return;

        hp -= dmg;
        Debug.Log("OUCH! HP: " + hp);
        if (src != null && hurtClip != null) src.PlayOneShot(hurtClip);

        if (hp <= 0)
        {
            GameOver();
            return;
        }
        StartCoroutine(Invincibility());
    }

    public void Heal(int amount)
    {
        hp = Mathf.Min(hp + amount, maxHp);
        Debug.Log("HEAL! HP: " + hp);
        if (src != null && healClip != null) src.PlayOneShot(healClip);
    }

    IEnumerator Invincibility()
    {
        invincible = true;
        for (int i = 0; i < 10; i++)
        {
            SetVisible(i % 2 == 1);
            yield return new WaitForSeconds(0.2f);
        }
        SetVisible(true);
        invincible = false;
    }

    void SetVisible(bool v)
    {
        foreach (Renderer r in renderers)
            if (r != null) r.enabled = v;
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
