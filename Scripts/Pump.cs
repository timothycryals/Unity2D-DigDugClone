using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pump : MonoBehaviour
{

    [SerializeField]
    private Player player;

    private EdgeCollider2D collider;

    private Transform parentObject;

    private Enemy target;

    private bool isStretching = false;

    [SerializeField]
    private float maxHoseLength = 0.75f;
    private float throwSpeed;

    private SpriteRenderer hoseRenderer;
    private SpriteRenderer nozzleRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<EdgeCollider2D>();
        throwSpeed = maxHoseLength / 20f;
        parentObject = transform.parent.parent;
        hoseRenderer = transform.parent.GetComponent<SpriteRenderer>();
        nozzleRenderer = GetComponent<SpriteRenderer>();
        ResetHose();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(ControllerInputs.XBOX_A))
        {
            if (player.CurrentState != PlayerState.Attacking && player.CurrentState != PlayerState.Dead && !isStretching)
            {
                player.CurrentState = PlayerState.Attacking;
                StartCoroutine(ThrowHose());
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(ControllerInputs.XBOX_A))
        {
            ResetHose();
        }
    }

    //This will retract the hose when the player is done with their attack
    private void ResetHose()
    {
        collider.enabled = false;
        target = null;
        isStretching = false;
        ActivateRenderers(false);
        parentObject.localScale = new Vector2(0f, parentObject.localScale.y);
        player.CurrentState = PlayerState.Idle;
        player.Pump(false);
        StopAllCoroutines();
    }

    private void ExtendHose(float length)
    {
        float newLength = length + throwSpeed;
        newLength = Mathf.Clamp(newLength, 0f, maxHoseLength);

        parentObject.localScale = new Vector3(newLength, parentObject.localScale.y);
    }

    private IEnumerator ThrowHose()
    {
        ActivateRenderers(true);
        collider.enabled = true;
        isStretching = true;
        while(parentObject.localScale.x < maxHoseLength)
        {
            ExtendHose(parentObject.localScale.x);
            yield return new WaitForSeconds(0.02f);
        }
        ResetHose();
    }

    private void ActivateRenderers(bool value)
    {
        hoseRenderer.enabled = value;
        nozzleRenderer.enabled = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && target == null)
        {
            StopAllCoroutines();
            target = collision.GetComponent<Enemy>();
            if(target.currentState == EnemyState.Flying)
            {
                target = null;
                return;
            }
            target.currentState = EnemyState.Pumped;
			if (target.GetComponent<Fygar>()) {
				if (player.currentDirection == Direction.Left || player.currentDirection == Direction.Right) {
					target.GetComponent<Fygar>().isPlayerBeside = 2;
				} else {
					target.GetComponent<Fygar>().isPlayerBeside = 1;
				}
			}
		}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && player.CurrentState != PlayerState.Attacking)
        {
            if (target != null && target.currentState != EnemyState.Disabled && target.currentState != EnemyState.Flying)
            {
                target.currentState = EnemyState.Recovering;
                ResetHose();
            }
        }
    }
}
