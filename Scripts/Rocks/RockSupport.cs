using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSupport : MonoBehaviour
{
    private List<Collider2D> supportingTiles;
    private Rock rock;

    // Start is called before the first frame update
    void Start()
    {
        rock = GetComponentInParent<Rock>();
        supportingTiles = new List<Collider2D>();
        StartCoroutine(CheckSupportingTiles());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            supportingTiles.Add(collision);
        }
    }

    private IEnumerator CheckSupportingTiles()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            if (supportingTiles.Count == 0)
            {
                rock.stateToFall();
                gameObject.SetActive(false);
                break;
            }

            foreach (Collider2D col in supportingTiles.ToArray())
            {
                if (col.gameObject.activeSelf)
                    continue;
                else
                    supportingTiles.Remove(col);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
