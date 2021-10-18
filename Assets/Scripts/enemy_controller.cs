using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_controller : MonoBehaviour
{
    public float velocidad_movimiento;
    private bool toca_suelo;
    private bool dir;

    private bool muerto;

    public Animator animator;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        muerto = false;
        rb = GetComponent<Rigidbody2D>();
        dir = animator.GetBool("izquierda");
        velocidad_movimiento *= -1;

        GameManager.enemiesInScene++;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.reseting)
            Die();

        //GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -3);
      if (toca_suelo && !muerto)
         
        {
            this.transform.position = new Vector3(this.transform.position.x + velocidad_movimiento * Time.deltaTime, this.transform.position.y, 10);
        }

    }
   
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "extremo_plataforma")
        {
            velocidad_movimiento *= -1;

            if (dir == true)
                dir = false;

            else
                dir = true;

            animator.SetBool("izquierda", dir);
        }

        if (collision.gameObject.tag == "Needel")
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemigo")
        {
            velocidad_movimiento *= -1;

            if (dir == true)
                dir = false;

            else
                dir = true;

            animator.SetBool("izquierda", dir);
        }
        if (collision.gameObject.tag == "plataforma")
        {
            toca_suelo = true;
            animator.SetBool("suelo", toca_suelo);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "plataforma")
        {
            toca_suelo = false;
            animator.SetBool("suelo", toca_suelo);
        }

    }

    void Die()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().PlayDestroyEnemySound();
        GameManager.enemiesInScene--;
        animator.SetTrigger("aguja");
        rb.velocity = new Vector2(0, 0);
        rb.isKinematic = true;
        muerto = true;
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(this.gameObject, 0.15f);
    }


}


