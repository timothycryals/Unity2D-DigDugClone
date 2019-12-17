using System.Collections;
using System;
using UnityEngine;

public enum RockState { IDLE, FALLING, WAITING_FOR_PLAYER, DISABLED }

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class Rock : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip hitGround;
    //This is a struct that will represent the collider's dimensions in the world space
    private struct ColliderWorldSpace
    {
        public float xLeft;
        public float xRight;

        public float yTop;
        public float yBottom;

        public ColliderWorldSpace(float x1, float x2, float y1, float y2)
        {
            xLeft = x1;
            xRight = x2;
            yTop = y1;
            yBottom = y2;
        }
    }

    public GameObject impactBase;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Transform player;
    private ColliderWorldSpace ColliderLocation;
    
    private static int RocksFallen;

    private RockState currentState;

    private void Start()
    {
        //audio = GetComponent<AudioSource>();
        //audio.clip.LoadAudioData();
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        impactBase.SetActive(false);
        RocksFallen = 0;
        currentState = RockState.IDLE;

        ColliderLocation = new ColliderWorldSpace(
            transform.position.x - (col.size.x * 2),
            transform.position.x + (col.size.x * 2),
            transform.position.y + (col.size.y),
            transform.position.y - (col.size.y)
            );
        StartCoroutine(AssignPlayer());
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    //audio.PlayOneShot(hitGround, 1);
    //    if (currentState == RockState.FALLING)
    //    {
    //        //RocksFallen += 1;
    //        Destroy(gameObject);
    //    }

        

    //}

    public void destroyRock()
    {
        //RocksFallen += 1;
        Destroy(gameObject);
        
    }

    public void stateToFall()
    {
        currentState = RockState.WAITING_FOR_PLAYER;
    }


    

    private void FixedUpdate()
    {

        switch (currentState)
        {
            case RockState.IDLE:
            case RockState.FALLING:
                break;
            case RockState.WAITING_FOR_PLAYER:
                if (!CheckForPlayer())
                {
                    StartCoroutine(dropRock());
                }
                break;
                
                
        }
    }

    IEnumerator dropRock()
    {
        currentState = RockState.FALLING;
        yield return new WaitForSeconds(0.5f);
        //audio.Play(0);
        rb.gravityScale = 1;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.freezeRotation = true;
        col.enabled = false;
        yield return new WaitForSeconds(.1f);
        col.enabled = false;
        impactBase.SetActive(true);

    }

    private bool CheckForPlayer()
    {
        Vector3 playerPosition = player.position;
        if(playerPosition.x >= ColliderLocation.xLeft && playerPosition.x <= ColliderLocation.xRight)
        {
            Debug.Log("Within X boundaries");
            if (playerPosition.y < transform.position.y)
            {
                Debug.Log("Under rock");
                return true;
            }
            return false;
        }
        return false;

    }

    private void OnDestroy()
    {
        if (currentState == RockState.FALLING)
            RocksFallen += 1;

        if (RocksFallen == 2 && GameManager.instance.roundInProgress)
        {
            VegetableSpawner.Instance.SpawnVegetable();
        }
    }

    private IEnumerator AssignPlayer()
    {
        while(player == null)
        {
            yield return new WaitForFixedUpdate();
            try {
                player = GameManager.instance.player.transform;
            }
            catch
            {
                continue;
            }
        }
    }
}
