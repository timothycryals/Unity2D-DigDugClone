using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private SpriteRenderer renderer;
    private BoxCollider2D collider;

    [SerializeField]
    private Sprite Fire1;
    [SerializeField]
    private Sprite Fire2;
    [SerializeField]
    private Sprite Fire3;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
    }

    public void SetFireActive(bool isActive)
    {
        if (isActive)
            StartCoroutine(StartFire());
        else
        {
            StopFire();
        }
    }

    private void StopFire()
    {
        renderer.sprite = null;
        collider.enabled = false;
    }

    private IEnumerator StartFire()
    {
        collider.enabled = true;
        renderer.sprite = Fire1;
        collider.size = renderer.sprite.bounds.size;
        collider.offset = renderer.sprite.bounds.center;
        yield return new WaitForSeconds(0.4f);
        renderer.sprite = Fire2;
        collider.size = renderer.sprite.bounds.size;
        collider.offset = renderer.sprite.bounds.center;
        yield return new WaitForSeconds(0.4f);
        renderer.sprite = Fire3;
        collider.size = renderer.sprite.bounds.size;
        collider.offset = renderer.sprite.bounds.center;
        yield return new WaitForSeconds(0.4f);
        StopFire();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!Player.isPlayerDead)
            {
                other.GetComponent<Player>().Die();
                collider.enabled = false;
            }
        }


    }
}
