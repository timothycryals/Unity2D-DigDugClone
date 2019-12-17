using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooka : Enemy
{
    private void FixedUpdate()
    {
        if (Player.isPlayerDead)
        {
            rb.velocity = Vector2.zero;
            anim.speed = 0;
            GetComponent<Collider2D>().enabled = false;
        }

        animatorInfo = anim.GetCurrentAnimatorStateInfo(0);

        switch (currentState)
        {
            case EnemyState.Idle:
                if (CurrentPath != null)
                {
                    rb.velocity = Vector2.zero;
                    t.position = Vector2.MoveTowards(t.position, CurrentPath[0].vPosition, 1f * Time.deltaTime);
                    if (Vector2.Distance(t.position, CurrentPath[0].vPosition) < 0.1f)
                    {
                        if (CurrentPath[0].vPosition.x < t.position.x)
                        {
                            RotateSprite(Direction.Left);
                        }
                        else
                        {
                            RotateSprite(Direction.Right);
                        }
                        CurrentPath.RemoveAt(0);
                        CurrentPath.TrimExcess();
                        if (CurrentPath.Count == 0)
                        {
                            CurrentPath = null;
                        }
                    }
                }
                else
                {
                    CheckForTunnelEnd();
                    Move();
                }
                anim.SetBool(WALKING_PARAM, true);
                break;
            case EnemyState.Flying:
                rb.velocity = Vector2.zero;
                RotateSprite(Direction.Right);
                FlyToPosition(flyingTarget);
                break;
            case EnemyState.Pumped:
                anim.SetBool(WALKING_PARAM, false);
                anim.SetBool(PUMPED_PARAM, true);
                PlayPumpedAudio(true);
                rb.velocity = Vector2.zero;
                if (animatorInfo.IsName("Dead"))
                {
					float yHeight = gameObject.transform.position.y;
					if (yHeight > 1.44) {
                        PlayerScore.AddToScore(200);
					} else if (yHeight < 1.44 && yHeight > -3.23) {
                        PlayerScore.AddToScore(300);
					} else if (yHeight < -3.23 && yHeight > -7.97) {
                        PlayerScore.AddToScore(400);
					} else {
                        PlayerScore.AddToScore(500);
					}
                    StartCoroutine(Die());
                }
                break;
            case EnemyState.Recovering:
                PlayPumpedAudio(false);
                anim.SetBool(PUMPED_PARAM, false);
                rb.velocity = Vector2.zero;
                if (animatorInfo.IsName("Idle"))
                {
                    currentState = EnemyState.Idle;
                }
                break;
            case EnemyState.Disabled:
                break;
        }
        RotateSprite(currentDirection);
    }

    private void OnDisable()
    {
        currentState = EnemyState.Idle;
        CurrentPath = null;
        anim.SetBool(PUMPED_PARAM, false);

    }
}
