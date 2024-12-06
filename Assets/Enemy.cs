using QFSW.MOP2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public static Enemy Instance;
    public GameObject player;
    public float health = 100;
    public float maxHealth = 100;
    public AudioClip destroySFX;
    public AudioClip[] BulletHitSFX;

    public GameObject coin;
    public ObjectPool explosionPool;
    public ObjectPool coinsPool;
    public NavMeshAgent agent;

    public GameObject heightHaReBaba;
    public float playerDamage = 10f;
    public bool Tutorial;
    private void OnEnable()
    {
         health = maxHealth;
        spriteRenderer.color = Color.white;
    }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        heightHaReBaba = GameObject.FindGameObjectWithTag("Height");
    }
    // Start is called before the first frame update
    void Start()
    {
        //if (spriteRenderer == null)
        //{
        //    spriteRenderer = GetComponent<SpriteRenderer>();
        //}
        originalColor = spriteRenderer.color;

        if(boss)
            slider.maxValue = maxHealth;
    }
    public bool heightLe;
    // Update is called once per frame
    void Update()
    {
        if(!heightLe)
        {
            if (player != null)
            {
                transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z));
            }
        }
        else
        {
            if (heightHaReBaba != null)
            {
                transform.LookAt(new Vector3(heightHaReBaba.transform.position.x, heightHaReBaba.transform.position.y, heightHaReBaba.transform.position.z));
            }
        }

       
        if(agent.enabled)
            agent.SetDestination(player.transform.position);
        HandleShooting();
        //MoveTowardsPlayer();
    }
    public GameObject explosionPrefab;
    public float moveSpeed = 5f;
    public EnemyHead enemyHead;
    GameObject explosion;
    public AudioClip killSound;
    public bool normalDestroy;
    public GameObject mainBody;
    public bool gamla;
    bool killed;
    public GameObject BadaDamaka;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health -= collision.gameObject.GetComponent<Projectile>().bulletDamage;
            if (boss)
                slider.value = health;
            AudioSource.PlayClipAtPoint(BulletHitSFX[Random.Range(0, 2)], transform.position, 1f);
            if (health <= 0)
            {
                if (boss)
                {
                    Instantiate(BadaDamaka, transform.position, Quaternion.identity);
                    LevelCompletion.Instance.Win();
                }
                explosion = explosionPool.GetObject();
                explosion.transform.position = transform.position;
                coinsPool.GetObject().transform.position = transform.position;
                //Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                
                AudioSource.PlayClipAtPoint(destroySFX, transform.position, 1f);
                AudioSource.PlayClipAtPoint(killSound, Camera.main.transform.position, 1f);
                //Instantiate(coin, transform.position, Quaternion.identity);

                if (gamla && !killed)
                {
                    LevelCompletion.Instance.GamlaCheck();
                    killed = true;
                }
           
                    if(!Tutorial)
                        KillStreak.Instance.KilledEnemy();
                    if (!normalDestroy)
                        enemyHead.ReleaseThisEnemy();
                    else
                        Destroy(mainBody);
                

                    



            }

            if (health <= maxHealth / 1.5 && health >= maxHealth / 2.5)
                spriteRenderer.color = collisionColorYellow;

            else if (health <= maxHealth / 2.5)
                spriteRenderer.color = collisionColorRed;
            else
                spriteRenderer.color = Color.white;
        }
        if (collision.gameObject.CompareTag("Player") && !collision.gameObject.GetComponent<PlayerHealth>().playerInvincible)
        {
            HurtPlayer();
            collision.gameObject.GetComponent<PlayerHealth>().health -= playerDamage;
            collision.gameObject.GetComponent<PlayerHealth>().GenerateHealth();
        }
        if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerHealth>().playerInvincible)
        {
            if(!gamla)
            {
                health = 0;
                explosion = explosionPool.GetObject();
                explosion.transform.position = transform.position;
                coinsPool.GetObject().transform.position = transform.position;
                //Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                

                AudioSource.PlayClipAtPoint(destroySFX, transform.position, 1f);
                AudioSource.PlayClipAtPoint(killSound, Camera.main.transform.position, 1f);
                //Instantiate(coin, transform.position, Quaternion.identity);

           
                    if (!Tutorial)
                        KillStreak.Instance.KilledEnemy();

                    if (!normalDestroy)
                        enemyHead.ReleaseThisEnemy();
                    else
                        Destroy(mainBody);
                
            }
            
        }
            
    }
    
    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }
    public SpriteRenderer spriteRenderer;
    public Color collisionColorRed = Color.red;
    public Color collisionColorYellow = Color.yellow;

    private Color originalColor;
    [SerializeField] GameObject Sender;
    public void HurtPlayer()
    {
       
        //PlayerHealth.instance.Damage(damageDone);
        bl_DamageInfo info = new bl_DamageInfo(playerDamage);
        info.Sender = Sender;
        Sender.SetIndicator();
        bl_DamageDelegate.OnDamageEvent(info);
        //CameraShake.Instance.shakeDuration += 0.3f;
    }

    public UnityEngine.UI.Slider slider;
    public bool boss;


    public bool FireEnemy;

    [Header("Settings")]
    public float safeDistance = 5f; // Distance to maintain from the player
    public float shootTime = 2f; // Time between each shot
    public Transform shootPoint; // Point from which the projectile is instantiated
    private float shootTimer; // Timer to keep track of shooting intervals
    public ObjectPool enemies;
    private void HandleShooting()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootTime)
        {
            shootTimer = 0f;
            float distance = Vector2.Distance(transform.position,player.transform.position);
            if (shootPoint && distance < 40)
            {
                // Instantiate the projectile and give it a direction
               
                GameObject enemy = enemies.GetObject(); 
                enemy.transform.position = shootPoint.position;
                enemy.transform.rotation = Quaternion.LookRotation(transform.right);
                enemy.GetComponent<Goli>().flame = transform;


            }
        }
    }
}
