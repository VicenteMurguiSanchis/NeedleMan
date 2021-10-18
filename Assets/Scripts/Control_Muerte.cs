using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Control_Muerte : MonoBehaviour
{

    public Canvas muerte;
    public character_controller personaje;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(personaje.muerto)
        {
            Time.timeScale = 0f;
            muerte.enabled = true;
        }
    }

    public void Menu_Principal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }
}
