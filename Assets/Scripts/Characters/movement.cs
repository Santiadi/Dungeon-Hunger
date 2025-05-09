using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movement : MonoBehaviour
{
    public static movement Instance;

    [Header("Movimiento")]
    public float speed = 100;

    public float revive = 1f;
    private Vector2 input;
    public Rigidbody2D rb;

    [Header("Combate")]
    public float fireDelay;
    private float lastFire;
    public GameObject swordHitBox;

    [Header("Vida")]
    public float currentHearts = 3f;
    public float maxHearts = 3f;

    [Header("Ghost Hearts")]
    public float ghostHearts = 0f;

    [Header("Audio")]
    public AudioClip attackClip;
    public AudioClip reviveClip;
    public AudioClip hurtClip;
    public AudioClip healClip;

    [Header("Efectos visuales")]
    public GameObject healParticlesPrefab;


    private AudioSource audioSource;



    [Header("Inventario y HUD")]
    public InventoryManager inventoryManager;
    public HUD_Hearts hudHearts;

    private Animator anim;

    void Awake()
    {
        if (SaveSystem.LoadPlayer() != null)
        {
            currentHearts = SaveSystem.GetUpgradeBonus("Salud maxima") == 0 ? 1 : SaveSystem.GetUpgradeBonus("Salud maxima");
            maxHearts = SaveSystem.GetUpgradeBonus("Salud maxima") == 0 ? 1 : SaveSystem.GetUpgradeBonus("Salud maxima");
            fireDelay = (float)(SaveSystem.GetUpgradeBonus("Velocidad de ataque") == 0 ? 1.5 : SaveSystem.GetUpgradeBonus("Velocidad de ataque"));
            speed = (float)(SaveSystem.GetUpgradeBonus("Velocidad de movimiento") == 0 ? 1.5 : SaveSystem.GetUpgradeBonus("Velocidad de movimiento"));
            revive = SaveSystem.GetUpgradeBonus("Revivir") == 0 ? 0 : SaveSystem.GetUpgradeBonus("Revivir");
            Debug.Log("Revive: " + revive);

        }

        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);

        if (hudHearts != null)
        {
            hudHearts.UpdateHearts(currentHearts, maxHearts);
        }
    }

    void Update()
    {
        ProcessInputs();
        Animate();

        float shootHor = Input.GetAxisRaw("ShootHorizontal");
        float shootVer = Input.GetAxisRaw("ShootVertical");

        if ((shootHor != 0 || shootVer != 0) && Time.time > lastFire + fireDelay)
        {
            anim.SetFloat("AttackX", shootHor);
            anim.SetFloat("AttackY", shootVer);
            anim.SetTrigger("AttackTrigger");

            Shoot(shootHor, shootVer);

            // ⚔️ Si el efecto SwordSlash está activo, dispara el slash
            SwordSlashEffect slashEffect = GetComponent<SwordSlashEffect>();
            if (slashEffect != null && slashEffect.IsActive())
            {
                slashEffect.TryShootSlash(new Vector2(shootHor, shootVer));
            }

            lastFire = Time.time;
        }
    }

    void FixedUpdate()
    {
        Vector2 movement = input * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void ProcessInputs()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        input.Normalize();
    }

    void Animate()
    {
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
        anim.SetFloat("MoveMagnitud", input.magnitude);
    }

    void Shoot(float x, float y)
    {
        Vector2 shootDirection = new Vector2(x, y);
        if (shootDirection.magnitude > 0)
        {
            shootDirection.Normalize();
        }

        SwordSlashEffect slashEffect = GetComponent<SwordSlashEffect>();

        if ((slashEffect == null || !slashEffect.IsActive()) && attackClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackClip);
        }

        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        swordHitBox.SetActive(true);
        swordHitBox.transform.position = transform.position + (Vector3)shootDirection;
    }

    public void DisableSwordHitbox()
    {
        swordHitBox.SetActive(false);
    }

    public void CharacterDamage(float damage)
    {
        if (ghostHearts > 0)
        {
            ghostHearts -= damage;
            ghostHearts = Mathf.Max(ghostHearts, 0);
        }
        else
        {
            currentHearts -= damage;
            currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);
        }

        hudHearts.UpdateHearts(currentHearts, maxHearts, ghostHearts);

        if (currentHearts <= 0 && ghostHearts <= 0)
        {
            anim.SetTrigger("Die");
        }
        else
        {
            anim.SetTrigger("Hurt");

            if (hurtClip != null && audioSource != null)
            {
                audioSource.PlayOneShot(hurtClip);
            }

        }

    }





    public void Die()
    {
        if (revive > 0)
        {
            currentHearts = maxHearts;
            revive--;
            if (hudHearts != null)
            {
                hudHearts.UpdateHearts(currentHearts, maxHearts);
            }
            anim.SetTrigger("Revive");

            if (reviveClip != null && audioSource != null)
            {
                audioSource.PlayOneShot(reviveClip);
            }
        }
        else
        {
            GameManager.Instance.ShowDeathScreen();
            GameManager.Instance.ReturnToMenuFromDeath(); // delega

            Destroy(gameObject);
        }
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHearts = Mathf.Clamp(maxHearts + amount, 1, 5);
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);

        if (hudHearts != null)
        {
            hudHearts.UpdateHearts(currentHearts, maxHearts);
        }
    }

    public void Heal(float amount)
    {
        currentHearts += amount;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);

        if (hudHearts != null)
        {
            hudHearts.UpdateHearts(currentHearts, maxHearts);
        }

        if (healClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(healClip);
        }

        if (healParticlesPrefab != null)
        {
            Instantiate(healParticlesPrefab, transform.position, Quaternion.identity);
        }
    }


}
