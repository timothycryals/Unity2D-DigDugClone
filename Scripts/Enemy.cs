using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EnemyState{ Attacking, Idle, Recovering, Flying, Pumped, Disabled}
public enum Direction { Left, Right, Up, Down}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Pathfinder))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public static List<Enemy> enemies = new List<Enemy>();

    protected const string WALKING_PARAM = "Walking";
    protected const string PUMPED_PARAM = "beingPumped";
    protected const string ATTACK_PARAM = "Attacking";
    protected const string ROCK_PARAM = "HitByRock";
    protected const string FLYING_PARAM = "Flying";

    protected Vector2 downVelocity = new Vector2(0, -1f);
    protected Vector2 upVelocity = new Vector2(0, 1f);
    protected Vector2 leftVelocity = new Vector2(-1f, 0);
    protected Vector2 rightVelocity = new Vector2(1f, 0);


    protected Pathfinder pathfinder;
    protected Rigidbody2D rb;
    [SerializeField]
    protected AudioSource pumpedAudio;
    [SerializeField]
    protected AudioSource deathAudio;

    protected List<Node> CurrentPath;
    protected bool moveVertically;
    protected bool usingInitialMovement;

    public EnemyState currentState;
    public Direction currentDirection;
    
    protected Animator anim;
    protected AnimatorStateInfo animatorInfo;

    protected Transform t;
    protected Vector3 flyingTarget;

    protected Fire fire;

	//protected PlayerScore pScore;

    [SerializeField]
    protected Transform player;

    IEnumerator startPathfinder()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            if (currentState == EnemyState.Flying || currentState == EnemyState.Pumped || currentState == EnemyState.Recovering || currentState == EnemyState.Attacking) continue;
            CurrentPath = pathfinder.FindPath(pathfinder.generateRelativePoint(t.position), pathfinder.generateRelativePoint(player.position));
            if (CurrentPath == null)
            {
                int rand = Random.Range(1, 16);
                if(rand == 1)
                {
                    rb.velocity = Vector2.zero;
                    currentState = EnemyState.Flying;
                    anim.SetBool(WALKING_PARAM, false);
                    anim.SetBool(FLYING_PARAM, true);
                    flyingTarget = player.position;
                }
                else
                {
                    SetInitialMovement();
                    InitialMovement();
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (this.GetType() == typeof(Fygar))
        {
            fire = GetComponentInChildren<Fire>();
        }
        player = GameManager.instance.player.transform;
        //pScore = player.GetComponent<PlayerScore>();
        pumpedAudio = GetComponent<AudioSource>();
        pumpedAudio.clip.LoadAudioData();
        deathAudio.clip.LoadAudioData();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentState = EnemyState.Idle;
        t = transform;
        pathfinder = GetComponent<Pathfinder>();
        enemies.Add(this);
        StartCoroutine(startPathfinder());
        SetInitialMovement();
        InitialMovement();
    }
      

    protected void SetInitialMovement()
    {
        usingInitialMovement = true;
        RaycastHit2D hit;
        int layerMask = (1 << LayerMask.NameToLayer("Terrain")) | (1 << LayerMask.NameToLayer("Rocks"));
        if (Physics2D.Raycast(t.position, Vector3.right, 1.5f, layerMask).collider != null && Physics2D.Raycast(t.position, Vector3.left, 1.5f, layerMask).collider != null)
        {
            moveVertically = true;
        }
        else
        {
            moveVertically = false;
        }
    }

    protected void InitialMovement()
    {
        if (moveVertically)
        {
            rb.velocity = new Vector2(0, 1f);
            currentDirection = Direction.Up;
        }
        else {
            rb.velocity = new Vector3(1f, 0);
            currentDirection = Direction.Right;
        }
    }

    protected void CheckForTunnelEnd()
    {
        int layerMask = (1 << LayerMask.NameToLayer("Terrain")) | (1 << LayerMask.NameToLayer("Rocks")) | (1 << LayerMask.NameToLayer("Borders"));
        switch (currentDirection)
        {
            case Direction.Down:
                if (Physics2D.Raycast(t.position, -Vector3.up, 0.3f, layerMask).collider != null)
                {
                    currentDirection = Direction.Up;
                    //RotateSprite(Direction.Up);
                }
                break;
            case Direction.Up:
                if (Physics2D.Raycast(t.position, Vector3.up, 0.3f, layerMask).collider != null)
                {
                    currentDirection = Direction.Down;
                    //RotateSprite(Direction.Down);
                }
                break;

            case Direction.Left:
                if (Physics2D.Raycast(t.position, -Vector3.right, 0.3f, layerMask).collider != null)
                {
                    currentDirection = Direction.Right;
                    RotateSprite(Direction.Right);
                }
                break;
            case Direction.Right:
                if (Physics2D.Raycast(t.position, Vector3.right, 0.3f, layerMask).collider != null)
                {
                    currentDirection = Direction.Left;
                    RotateSprite(Direction.Left);
                }
                break;
        }
    }

    protected void FlyToPosition(Vector3 position)
    {
        t.position = Vector2.MoveTowards(t.position, position, 0.8f * Time.deltaTime);
        if (t.position == position)
        {
            currentState = EnemyState.Idle;
            anim.SetBool(FLYING_PARAM, false);
            anim.SetBool(WALKING_PARAM, true);
        }
    }

    protected void Move()
    {
        if (usingInitialMovement)
        {
            switch (currentDirection)
            {
                case Direction.Down:
                    rb.velocity = downVelocity;
                    break;
                case Direction.Up:
                    rb.velocity = upVelocity;
                    break;
                case Direction.Left:
                    rb.velocity = leftVelocity;
                    break;
                case Direction.Right:
                    rb.velocity = rightVelocity;
                    break;

            }
        }
    }

    protected void PlayPumpedAudio(bool isActive)
    {
        if (isActive)
        {
            if(!pumpedAudio.isPlaying)
                pumpedAudio.Play();
        }
        else
        {
            pumpedAudio.Stop();
        }
    }


    protected void RotateSprite(Direction newDir)
    {
        switch (newDir)
        {
            /*case Direction.Down:
                t.eulerAngles = new Vector3(0, 0, -90);
                break;
            case Direction.Up:
                t.eulerAngles = new Vector3(0, 0, 90);
                break;*/
            case Direction.Left:
                t.eulerAngles = new Vector3(0, 180, 0);
                break;
            case Direction.Right:
                t.eulerAngles = new Vector3(0, 0, 0);
                break;
            default:
                break;
        }
    }

    protected IEnumerator Die()
    {
        currentState = EnemyState.Disabled;
        deathAudio.Play();
        while (deathAudio.isPlaying)
        {
            yield return null;
        }
        enemies.Remove(this);
        enemies.TrimExcess();
        gameObject.SetActive(false);
        EnemySpawner.Instance.ReturnEnemyToQueue(gameObject);
        print(enemies.Count);
        if(enemies.Count == 0)
        {
            GameManager.instance.roundInProgress = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && (currentState != EnemyState.Pumped && currentState != EnemyState.Recovering))
        {
            if(!Player.isPlayerDead)
                collision.GetComponent<Player>().Die();

            //collision.gameObject.SetActive(false);
            //SceneManager.LoadScene("GameOver");
        }
        if (collision.tag == "impact")
        {
            print("Impact hit enemy");
            //gameObject.SetActive(false);
            StartCoroutine(Die());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Hose" && !animatorInfo.IsName("Pumped3") && currentState == EnemyState.Pumped)
        {
            currentState = EnemyState.Recovering;
            anim.SetBool(PUMPED_PARAM, false);
        }
    }

    public void UpdatePathfinderGrid(Grid grid)
    {
        pathfinder.GridReference = grid;
    }
}
