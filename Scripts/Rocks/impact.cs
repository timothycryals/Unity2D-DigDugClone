using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class impact : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.GetComponentInParent<Rock>().destroyRock();
    }
}
