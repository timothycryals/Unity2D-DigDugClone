using System.Collections;
using UnityEngine;

public enum VegetableType { CARROT, TURNIP, MUSHROOM, CUCUMBER, EGGPLANT, PEPPER, TOMATO, GARLIC, WATERMELON, GALAXIAN, PINEAPPLE }


[RequireComponent(typeof(Collider2D))]
public class Vegetable : MonoBehaviour
{
    private const int CARROT_VALUE = 400;
    private const int TURNIP_VALUE = 600;
    private const int MUSHROOM_VALUE = 800;
    private const int CUCUMBER_VALUE = 1000;
    private const int EGGPLANT_VALUE = 2000;
    private const int PEPPER_VALUE = 3000;
    private const int TOMATO_VALUE = 4000;
    private const int GARLIC_VALUE = 5000;
    private const int WATERMELON_VALUE = 6000;
    private const int GALAXIAN_VALUE = 7000;
    private const int PINEAPPLE_VALUE = 8000;

    private const float VEGETABLE_TIME = 10.0f;

    [SerializeField]
    private VegetableType type;

    private int _points;
    public int points
    {
        get
        {
            return _points;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Assign the proper amount of points to the vegetable based on its type
        //in the frame the vegetable was created
        switch (type)
        {
            case VegetableType.CARROT:
                _points = CARROT_VALUE;
                break;
            case VegetableType.TURNIP:
                _points = TURNIP_VALUE;
                break;
            case VegetableType.MUSHROOM:
                _points = MUSHROOM_VALUE;
                break;
            case VegetableType.CUCUMBER:
                _points = CUCUMBER_VALUE;
                break;
            case VegetableType.EGGPLANT:
                _points = EGGPLANT_VALUE;
                break;
            case VegetableType.PEPPER:
                _points = PEPPER_VALUE;
                break;
            case VegetableType.TOMATO:
                _points = TOMATO_VALUE;
                break;
            case VegetableType.GARLIC:
                _points = GARLIC_VALUE;
                break;
            case VegetableType.WATERMELON:
                _points = WATERMELON_VALUE;
                break;
            case VegetableType.GALAXIAN:
                _points = GALAXIAN_VALUE;
                break;
            case VegetableType.PINEAPPLE:
                _points = PINEAPPLE_VALUE;
                break;
            default:
                //If there is no type assigned for any reason, then disable this object
                Debug.Log("No type specified.");
                gameObject.SetActive(false);
                break;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Timer());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the vegetable collides with the player, then add the points to the player's score and
        //disable the vegetable gameObject
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerScore.AddToScore(_points);
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(VEGETABLE_TIME);
        gameObject.SetActive(false);
    }
}
