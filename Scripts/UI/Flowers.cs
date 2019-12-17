using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flowers : MonoBehaviour
{
    public static Flowers instance = null;

    //Array holds all flower gameobjects
    [SerializeField]
    SpriteRenderer[] flowerArray;
    
    //the sprite for a small flower
    [SerializeField]
    private Sprite smallFlower;

    //the sprite for a large flower
    [SerializeField]
    private Sprite largeFlower;

    //These are the y cordinates for large and small flower sprites
    private float smallFlowerY = 3.71f;
    private float largeFlowerY = 3.88f;

    //The maximum level the flowers can go to on the screen
    private const int CAP_LEVEL = 68;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        GameManager.instance.UpdateRoundNumberGUI += updateFlowers;
        flowerArray = GetComponentsInChildren<SpriteRenderer>();
        updateFlowers(GameManager.instance.roundNumber);
    }

    //changes the sprite attached to flower game objects based on the round number
    //A large flower for a multiple of 10 and small flower for all remainders
    void updateFlowers(int round)
    {
        for (int k = 0; k < flowerArray.Length; k++){
            flowerArray[k].sprite = null;
        }

        if (round <= CAP_LEVEL)
        {
            if (round < 10)
            {
                for (int i = 0; i < round; i++)
                {
                    flowerArray[i].sprite = smallFlower;
                    flowerArray[i].transform.localPosition = new Vector2(flowerArray[i].transform.localPosition.x, smallFlowerY);
                }
            }
            else
            {
                int i;
                for (i = 0; i < Mathf.FloorToInt(round / 10); i++)
                {
                    flowerArray[i].sprite = largeFlower;
                    flowerArray[i].transform.localPosition = new Vector2(flowerArray[i].transform.localPosition.x, largeFlowerY);
                }
                for (int j = 0; j < round % 10; j++)
                {
                    flowerArray[i + j].sprite = smallFlower;
                    flowerArray[i + j].transform.localPosition = new Vector2(flowerArray[i + j].transform.localPosition.x, smallFlowerY);
                }

            }
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.UpdateRoundNumberGUI -= updateFlowers;
    }
}
