using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public GameObject enemyHitEffect;
    public GameObject deathParticles;
    public GameManager gameManager;
    public GameObject activeHitEffect;
    public GameObject activeDeathParticles;


    bool isInvincible = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isInvincible && collision.collider.tag == "Enemy")
        {
            if (gameManager.gameLost) return;

            activeHitEffect = Instantiate(enemyHitEffect, collision.contacts[0].point, Quaternion.identity);
            collision.collider.GetComponent<Enemy>().hitPlayer();
            activeDeathParticles = Instantiate(deathParticles, transform.position, transform.rotation);
            isInvincible = true;

            gameManager.LoseGame();

            Debug.Log("Enemy Hit");
        }
    }

    public void SetInvincible(float seconds)
    {
        StartCoroutine(Invincible(seconds));
    }

    public void DestroyEffects(float delay = 0f)
    {
        Destroy(activeHitEffect, delay);
        Destroy(activeDeathParticles, delay);
    }

    IEnumerator Invincible(float seconds)
    {
        isInvincible = true;
        yield return new WaitForSeconds(seconds);
        isInvincible = false;
    }

}
