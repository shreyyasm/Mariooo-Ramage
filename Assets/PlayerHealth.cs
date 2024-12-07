using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    public bool regenerateHealth;
    public bool playerInvincible;
    public float invincibleTimer = 20f;

    public AudioSource BgMusic;
    public AudioSource StarBG;

    public PostProcessVolume volume;
    ChromaticAberration chromaticAberration;
    public bool hit;
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGetSettings(out chromaticAberration);
    }
    bool playerDead;
    // Update is called once per frame
    void Update()
    {
        if (regenerateHealth && health <= 100)
        {
            health += Time.deltaTime * 2;
        }
        if(playerInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                chromaticAberration.active = false;
                playerInvincible = false;
                character.holdingButtonRun = false;
                PlayerMovement.moveSpeed -= 1000;
                BgMusic.Play();
                StarBG.Stop();
            }
               
        }
        if(health <= 0 && !playerDead)
        {
            RaceCountdown.Instance.GameOver();
            gameObject.GetComponent<PlayerMovement>().ExplodeItself();
            playerDead = true;
        }
      
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Blood"))
        //    HurtPlayer();
    }
    public void HurtPlayer()
    {
        
    }
    private IEnumerator coroutine;
    public void GenerateHealth()
    {
        
        regenerateHealth = false;
        StartCoroutine(Generate());
    }
    IEnumerator Generate()
    {
        
        yield return new WaitForSeconds(3f);
        regenerateHealth = true;
       
    }
    public PlayerMovement PlayerMovement;
    public Character character;
    public void StartInvincibilty()
    {
        chromaticAberration.active = true;
        playerInvincible = true;
        invincibleTimer = 20f;
        character.holdingButtonRun = true;
        PlayerMovement.moveSpeed += 1000;
        BgMusic.Stop();
        StarBG.Play();
        
    }
}
