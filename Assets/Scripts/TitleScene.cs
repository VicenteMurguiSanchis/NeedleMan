using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    Controller controls;

    [SerializeField] GameObject[] titleSceneThings;
    [SerializeField] GameObject[] gamePlaySceneThings;
    [SerializeField] GameObject[] gameOverSceneThings;

    private void Awake()
    {
        controls = new Controller();
        controls.GamePlay.Jump.performed += ctx => Play();

        for (int i = 0; i < titleSceneThings.Length; i++)
        {
            titleSceneThings[i].SetActive(true);
        }

        for (int i = 0; i < gamePlaySceneThings.Length; i++)
        {
            gamePlaySceneThings[i].SetActive(false);
        }

        for (int i = 0; i < gameOverSceneThings.Length; i++)
        {
            gameOverSceneThings[i].SetActive(false);
        }
    }

    public void Play()
    {
        for (int i = 0; i < gamePlaySceneThings.Length; i++)
        {
            gamePlaySceneThings[i].SetActive(true);
        }

        for (int i = 0; i < titleSceneThings.Length; i++)
        {
            titleSceneThings[i].SetActive(false);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        controls.GamePlay.Enable();
    }

    private void OnDisable()
    {
        controls.GamePlay.Disable();
    }

}
