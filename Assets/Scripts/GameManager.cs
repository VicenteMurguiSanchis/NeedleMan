using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool paraMando; // TRUE = SE USA MANDO

    public static int enemiesInScene;

    public static bool reseting;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip destroyEnemySound;

    private void Awake()
    {
        enemiesInScene = 0;

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Physics2D.IgnoreLayerCollision(8, 9);
        Physics2D.IgnoreLayerCollision(8, 10);
        Physics2D.IgnoreLayerCollision(9, 9);
        Physics2D.IgnoreLayerCollision(10, 10);

        if (reseting)
            StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.5f);
        reseting = false;
    }

    public void PlayDestroyEnemySound()
    {
        audioSource.PlayOneShot(destroyEnemySound);
    }
}
