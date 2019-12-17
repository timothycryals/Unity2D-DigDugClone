using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public static EnemySpawner Instance;

    [SerializeField]
    private GameObject pookaPrefab;
    [SerializeField]
    private GameObject fygarPrefab;

    private Queue<GameObject> Fygars;
    private Queue<GameObject> Pookas;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Fygars = new Queue<GameObject>();
        Pookas = new Queue<GameObject>();
    }

    public void SpawnEnemy(Vector3 position)
    {
        int rand = Random.Range(1, 3);
        if (rand == 1)
        {
            SpawnFygar(position);
        }
        else
        {
            SpawnPooka(position);
        }
    }

    public void SpawnFygar(Vector3 position)
    {
        GameObject fygar;
        if (Fygars.Count != 0)
        {
            fygar = Fygars.Dequeue();
            fygar.transform.position = position;
            fygar.SetActive(true);
        }
        else
        {
            Fygars.Enqueue(Instantiate(fygarPrefab, transform));
            SpawnFygar(position);
        }
    }

    public void SpawnPooka(Vector3 position)
    {
        GameObject pooka;
        if(Pookas.Count != 0)
        {
            pooka = Pookas.Dequeue();
            pooka.transform.position = position;
            pooka.SetActive(true);
        }
        else
        {
            Pookas.Enqueue(Instantiate(pookaPrefab, transform));
            SpawnPooka(position);
        }
    }

    public void ReturnEnemyToQueue(GameObject enemy)
    {
        if (enemy.GetComponent<Enemy>().GetType() == typeof(Pooka))
        {
            Pookas.Enqueue(enemy);
        }
        else
        {
            Fygars.Enqueue(enemy);
        }
    }
}
