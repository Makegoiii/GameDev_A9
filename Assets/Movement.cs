using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    public float speed = 6f;
    public float turnSpeed = 12f;
    public float jumpImpulse = 6f;
    public float airControl = 0.3f;
    public float timeLimit = 60f;

    public Transform feet;
    public float groundRadius = 0.2f;
    public LayerMask groundMask;

    public Animator anim;
    public Animator gateAnimator;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI winPanel;
    public TextMeshProUGUI timerText;
    public AudioSource audioSource;
    public AudioClip coinSound;
    public AudioSource bgMusic;

    public float messageTime = 2f;
    private Coroutine messageRoutine;
    private Rigidbody rb;
    private int coinsCollected = 0;
    private int totalCoins;
    private bool grounded = false;
    private float timeLeft;
    private bool gameOver = false;
    private Transform cam;

    private Vector2 move;
    private bool fireRequested;
    private Shooter shooter;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        shooter = GetComponent<Shooter>();
    }

    void Start()
    {
        totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        Debug.Log("Coins in this level: " + totalCoins);

        timeLeft = timeLimit;
        winPanel.text = "";
        ShowCoins();

        bool musicOn = PlayerPrefs.GetInt("Music", 1) == 1;
        if (bgMusic != null)
        {
            bgMusic.mute = !musicOn;
            bgMusic.volume = PlayerPrefs.GetFloat("Volume", 1f);
        }
    }

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if (gameOver || PauseMenu.IsPaused) return;

        if (grounded && rb.linearVelocity.y <= 0.1f)
        {
            Vector3 v = rb.linearVelocity;
            v.y = 0f;
            rb.linearVelocity = v;
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
        }
    }

    public void OnFire()
    {
        if (PauseMenu.IsPaused) return;
        fireRequested = true;
    }

    Vector3 GetMoveDir()
    {
        Vector3 forward = cam.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = cam.right;
        right.y = 0f;
        right.Normalize();

        return Vector3.ClampMagnitude(forward * move.y + right * move.x, 1f);
    }

    void Update()
    {
        if (gameOver || PauseMenu.IsPaused) return;

        grounded = Physics.CheckSphere(feet.position, groundRadius, groundMask);

        Vector3 flat = rb.linearVelocity;
        flat.y = 0f;
        anim.SetFloat("Speed", flat.magnitude);
        anim.SetBool("Grounded", grounded);

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            timeLeft = 0;
            EndGame("Время вышло!");
        }
        timerText.text = "Время: " + Mathf.CeilToInt(timeLeft);

        bool fireHeld = false;
        foreach (Gamepad gp in Gamepad.all)
            if (gp.buttonWest.isPressed) { fireHeld = true; break; }

        if (Keyboard.current != null && Keyboard.current.xKey.isPressed)
            fireHeld = true;

        bool pointerOnUI = EventSystem.current != null
                           && EventSystem.current.IsPointerOverGameObject();
        bool clickShot = fireRequested && !pointerOnUI;
        fireRequested = false;

        if (shooter != null && (fireHeld || clickShot))
            shooter.Shoot(transform.forward);

        if (transform.position.y < -10f)
            Restart();
    }

    void FixedUpdate()
    {
        if (gameOver) return;

        Vector3 dir = GetMoveDir();

        Vector3 target = dir * speed;
        Vector3 current = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        Vector3 horizontal = Vector3.Lerp(current, target, grounded ? 1f : airControl);

        rb.linearVelocity = new Vector3(horizontal.x, rb.linearVelocity.y, horizontal.z);

        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion look = Quaternion.LookRotation(dir);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, look, turnSpeed * Time.fixedDeltaTime));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameOver) return;

        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coinsCollected++;
            ShowCoins();

            if (audioSource != null && coinSound != null)
                audioSource.PlayOneShot(coinSound);

            if (coinsCollected >= totalCoins && gateAnimator != null)
                gateAnimator.SetBool("Open", true);
        }

        if (other.CompareTag("Exit"))
        {
            if (coinsCollected >= totalCoins)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                if (messageRoutine != null) StopCoroutine(messageRoutine);
                messageRoutine = StartCoroutine(ShowMessage("Сначала соберите все монеты!"));
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
            GameOver();
    }

    void GameOver()
    {
        if (gameOver) return;

        gameOver = true;
        winPanel.text = "Game Over!";
        StopPlayer();
        Invoke(nameof(Restart), 2f);
    }

    void EndGame(string message)
    {
        gameOver = true;
        winPanel.text = message;
        StopPlayer();
    }

    void StopPlayer()
    {
        rb.linearVelocity = Vector3.zero;
        anim.SetFloat("Speed", 0f);
        anim.SetBool("Grounded", true);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ShowCoins()
    {
        coinText.text = "Монеты: " + coinsCollected + " / " + totalCoins;
    }

    System.Collections.IEnumerator ShowMessage(string message)
    {
        winPanel.text = message;
        yield return new WaitForSeconds(messageTime);

        if (!gameOver)
            winPanel.text = "";
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    void OnDrawGizmosSelected()
    {
        if (feet == null) return;
        Gizmos.color = grounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(feet.position, groundRadius);
    }
}
