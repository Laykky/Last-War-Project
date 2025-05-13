using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload_Game : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ResetScene()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

}
