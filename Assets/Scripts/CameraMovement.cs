using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform backGround;
    [SerializeField] GameObject player;

    private void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        Vector2 targetPos = player.transform.position.normalized;

        transform.position = Vector2.MoveTowards(transform.position, new Vector3(targetPos.x,targetPos.y, -10), 0.1f);
        backGround.position = new Vector3(-transform.position.x/2, -transform.position.y/2, backGround.position.z);
    }

}
