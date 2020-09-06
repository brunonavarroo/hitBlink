using System.Collections;
using Cinemachine;
using Pixelplacement;
using UnityEngine;

public class HitStopManager : Singleton<HitStopManager>
{

    CinemachineImpulseSource cinemachineImpulseSource;
    #region hitStop
    bool waitingHitStop;
    #endregion

    #region HITBLINK PARAMETERS
    //BoxCollider2D boxCollider2D; // OPTIONAL
    [SerializeField] SpriteRenderer playerSpriteRenderer;
    [Header("Blinking Parameters")]
    [SerializeField] float spriteBlinkingTimer = 0.0f;
    [SerializeField] float spriteBlinkingMiniDuration = 0.1f;
    [SerializeField] float spriteBlinkingTotalTimer = 0.0f;
    [SerializeField] float spriteBlinkingTotalDuration = 1.0f;

    [Header("Activate to start")]
    public bool startBlinking = false;
    #endregion
    private void Awake()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }
    private void Update()
    {
        if (startBlinking)
        {
            SpriteBlinkingEffect();
        }
    }

    #region hitStop

    public void hitStop(float duration)
    {
        //print("HITSOP");
        if (waitingHitStop)
        {
            return;
        }
        Time.timeScale = 0f;
        StartCoroutine(Wait(duration));
    }
    IEnumerator Wait(float duration)
    {
        waitingHitStop = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        waitingHitStop = false;//HitSTOP Finishes
        //cinemachineImpulseSource.GenerateImpulse(Vector2.one);
        GameManager.Instance.reduceHealth(GameManager.Instance.minDamage);
        if (GameManager.Instance.health>0)//we will only blink if we still be alive
        {
            startBlinking = true;
        }
        
    }
    #endregion

    #region HITBLINK
    private void SpriteBlinkingEffect()
    {
        spriteBlinkingTotalTimer += Time.deltaTime;
        //boxCollider2D.enabled = false;
        if (spriteBlinkingTotalTimer >= spriteBlinkingTotalDuration)
        {
            startBlinking = false;
            //boxCollider2D.enabled = true;
            spriteBlinkingTotalTimer = 0.0f;
            playerSpriteRenderer.enabled = true;   // according to your sprite
            return;
        }

        spriteBlinkingTimer += Time.deltaTime;
        if (spriteBlinkingTimer >= spriteBlinkingMiniDuration)
        {
            spriteBlinkingTimer = 0.0f;
            if (playerSpriteRenderer.enabled == true)
            {
                playerSpriteRenderer.enabled = false;  //make changes
            }
            else
            {
                playerSpriteRenderer.enabled = true;   //make changes
            }
        }
    }
    #endregion

}
