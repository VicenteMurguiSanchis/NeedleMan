using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Control_Pausa : MonoBehaviour
{
    public Canvas Menu_Pausa;
    public Canvas muerte;
    public bool menu_cerrado;

    private void Awake()
    {
        menu_cerrado = true;
    }

    void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            if (menu_cerrado && !muerte.enabled)
            {
                Menu_Pausa.enabled = true;
                menu_cerrado = false;
                Time.timeScale = 0f;
            }

            else if(!menu_cerrado)
            {
                Menu_Pausa.enabled = false;
                menu_cerrado = true;
                Time.timeScale = 1f;
            }
        }
    }

    public void Volver()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }

    public void Continuar()
    {
        Time.timeScale = 1f;
        Menu_Pausa.enabled = false;
        menu_cerrado = true;
    }

}
