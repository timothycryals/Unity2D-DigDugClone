using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState { Attacking, Idle, Walking, Dead}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

    public static bool isPlayerDead = false;

    public static int NumLives = 3;
    public static bool ControllerEnabled;

    private const string WALK_PARAM = "Walking";
    private const string PUMP_PARAM = "isPumping";
    private const string DEATH_PARAM = "Dead";

	// public Tilemap tilemap;
	public Rigidbody2D rb;
	public float movementSpeed;
    public AudioSource audio;
    public AudioSource audio2;
    public AudioSource deathAudio;

	private Transform t;
	private Animator anim;

	private int digCount;

	public Direction currentDirection = Direction.Right;

    private PlayerState _currentState = PlayerState.Idle;
    public PlayerState CurrentState
    {
        get { return _currentState; }
        set { _currentState = value; }
    }

	private void Start() {
        audio = GetComponent<AudioSource>();
        audio.clip.LoadAudioData();
        audio2.clip.LoadAudioData();
        deathAudio.clip.LoadAudioData();
        digCount = 0;
		t = transform;
		anim = GetComponent<Animator>();
        GameManager.instance.player = gameObject;
        StartCoroutine(CheckForController());
	}

	void FixedUpdate() {

		switch (_currentState)
        {
            case PlayerState.Idle:
                Move();
                break;
            case PlayerState.Attacking:
                PlayMovementAudio(false);
                Pump(true);
                break;
            case PlayerState.Walking:
                Move();
                break;
            case PlayerState.Dead:
                rb.velocity = Vector3.zero;
                PlayMovementAudio(false);
                break;
        }
	}

	private void LateUpdate() {
        ChangeRotation();
	}

	private void Move() {
        /*
		
		NOTE: 

		The first problem is that Input.GetAxis with a keyboard means that there will be a slight delay in stopping/switching directions

		The second problem with our current input handling is that Input.GetButton does not work with controllers, 
		or atleast with my controller it wasnt, so we have to use Input.GetAxis to work with them.
		
		We need some way to detect if the player is using a controller, or if they are using a keyboard

		*/
        if (ControllerEnabled)
        {
            if (Input.GetAxis("Horizontal") > 0.9f)
            {
                rb.velocity = new Vector2(movementSpeed, 0);
                currentDirection = Direction.Right;
                anim.SetBool(WALK_PARAM, true);
                PlayMovementAudio(true);
            }
            else if (Input.GetAxis("Horizontal") < -0.7f)
            {
                rb.velocity = new Vector2(-movementSpeed, 0);
                currentDirection = Direction.Left;
                anim.SetBool(WALK_PARAM, true);
                PlayMovementAudio(true);
            }
            else if (Input.GetAxis("Vertical") > 0.3f)
            {
                rb.velocity = new Vector2(0, movementSpeed);
                currentDirection = Direction.Up;
                anim.SetBool(WALK_PARAM, true);
                PlayMovementAudio(true);
            }
            else if (Input.GetAxis("Vertical") < -0.3f)
            {
                rb.velocity = new Vector2(0, -movementSpeed);
                currentDirection = Direction.Down;
                anim.SetBool(WALK_PARAM, true);
                PlayMovementAudio(true);
            }
            else
            {
                anim.SetBool(WALK_PARAM, false);
                PlayMovementAudio(false);
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(movementSpeed, 0);
                currentDirection = Direction.Right;
                anim.SetBool(WALK_PARAM, true);
                PlayMovementAudio(true);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-movementSpeed, 0);
                currentDirection = Direction.Left;
                anim.SetBool(WALK_PARAM, true);
                PlayMovementAudio(true);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                rb.velocity = new Vector2(0, movementSpeed);
                currentDirection = Direction.Up;
                anim.SetBool(WALK_PARAM, true);
                PlayMovementAudio(true);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rb.velocity = new Vector2(0, -movementSpeed);
                currentDirection = Direction.Down;
                anim.SetBool(WALK_PARAM, true);
                PlayMovementAudio(true);
            }
            else
            {
                anim.SetBool(WALK_PARAM, false);
                PlayMovementAudio(false);
                rb.velocity = Vector2.zero;
            }
        }
	}

	private void PlayMovementAudio(bool isActive)
    {
        if (isActive)
        {
            if (audio.isPlaying)
            {
                return;
            }
            else
            {
                audio.UnPause();
            }
        }
        else
        {
            if (audio.isPlaying)
            {
                audio.Pause();
            }
            else
            {
                return;
            }
        }
    }

    public void Pump(bool value)
    {
        try
        {
            if (value)
            {
                if (!audio2.isPlaying)
                {
                    audio2.Play();
                }
            }
            else
            {
                audio2.Stop();
            }
            rb.velocity = Vector3.zero;
            anim.SetBool(PUMP_PARAM, value);
            
        }
        catch (Exception e)
        {
            return;
        }
    }

    private void ChangeRotation()
    {
        switch (currentDirection)
        {
            case Direction.Left:
                t.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case Direction.Right:
                t.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.Down:
                t.rotation = Quaternion.Euler(0, t.rotation.eulerAngles.y, -90);
                break;
            case Direction.Up:
                t.rotation = Quaternion.Euler(0, t.rotation.eulerAngles.y, 90);
                break;
        }
    }
	
    private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.layer == 10) {
			collision.gameObject.SetActive(false);
			digCount += 1;
			if (digCount >= 20) {
                PlayerScore.AddToScore(10);
				digCount = 0;
			}
		}
	}

    public void Die()
    {
        _currentState = PlayerState.Dead;
        isPlayerDead = true;
        deathAudio.Play();
        anim.SetTrigger(DEATH_PARAM);
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
            yield return null;
        }
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        PlayerScore.SaveScores();
        if(isPlayerDead)
            SceneManager.LoadScene("GameOver");
    }

    private IEnumerator CheckForController()
    {
        string[] inputs;
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            inputs = Input.GetJoystickNames();
            for (int i = 0; i < inputs.Length; i++)
            {
                if (!string.IsNullOrEmpty(inputs[i]))
                {
                    ControllerEnabled = true;
                    break;
                }
                else
                {
                    ControllerEnabled = false;
                    break;
                }
            }
        }
    }
}