using System.Collections.Generic;
using UnityEngine;

public class VegetableSpawner : MonoBehaviour
{
    public static VegetableSpawner Instance;

    [SerializeField]
    private Transform spawnPoint;

    [Header("Vegetable Prefabs")]
    [SerializeField]
    private GameObject carrot;
    [SerializeField]
    private GameObject turnip;
    [SerializeField]
    private GameObject mushroom;
    [SerializeField]
    private GameObject cucumber;
    [SerializeField]
    private GameObject eggplant;
    [SerializeField]
    private GameObject pepper;
    [SerializeField]
    private GameObject tomato;
    [SerializeField]
    private GameObject garlic;
    [SerializeField]
    private GameObject watermelon;
    [SerializeField]
    private GameObject galaxian;
    [SerializeField]
    private GameObject pineapple;

    private Dictionary<VegetableType, GameObject> Vegetables;
    private VegetableType CurrentVegetableType;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            InstantiateVegetables();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    public void UpdateCurrentVegetable(int roundNumber)
    {
        switch (roundNumber)
        {
            case 1:
                CurrentVegetableType = VegetableType.CARROT;
                break;
            case 2:
                CurrentVegetableType = VegetableType.TURNIP;
                break;
            case 3:
                CurrentVegetableType = VegetableType.MUSHROOM;
                break;
            case 4:
                CurrentVegetableType = VegetableType.CUCUMBER;
                break;
            case 6:
                CurrentVegetableType = VegetableType.EGGPLANT;
                break;
            case 8:
                CurrentVegetableType = VegetableType.PEPPER;
                break;
            case 10:
                CurrentVegetableType = VegetableType.TOMATO;
                break;
            case 12:
                CurrentVegetableType = VegetableType.GARLIC;
                break;
            case 14:
                CurrentVegetableType = VegetableType.WATERMELON;
                break;
            case 16:
                CurrentVegetableType = VegetableType.GALAXIAN;
                break;
            case 18:
                CurrentVegetableType = VegetableType.PINEAPPLE;
                break;
            default:
                break;
        }
    }

    private void InstantiateVegetables()
    {
        Vegetables = new Dictionary<VegetableType, GameObject>();
        Vegetables.Add(VegetableType.CARROT, carrot);
        Vegetables.Add(VegetableType.TURNIP, turnip);
        Vegetables.Add(VegetableType.MUSHROOM, mushroom);
        Vegetables.Add(VegetableType.CUCUMBER, cucumber);
        Vegetables.Add(VegetableType.EGGPLANT, eggplant);
        Vegetables.Add(VegetableType.PEPPER, pepper);
        Vegetables.Add(VegetableType.TOMATO, tomato);
        Vegetables.Add(VegetableType.GARLIC, garlic);
        Vegetables.Add(VegetableType.WATERMELON, watermelon);
        Vegetables.Add(VegetableType.GALAXIAN, galaxian);
        Vegetables.Add(VegetableType.PINEAPPLE, pineapple);
        CurrentVegetableType = VegetableType.CARROT;
        
    }

    public void SpawnVegetable()
    {
        Vegetables[CurrentVegetableType].SetActive(true);
    }

}
