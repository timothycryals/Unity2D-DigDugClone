using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void changeScene()
    {
        Debug.Log("changing scenes");
        SceneManager.LoadScene("main");

    }

    public void ChangeToMainMenu()
    {
        SceneManager.LoadScene("menu");
    }
}
