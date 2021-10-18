using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conejo_Saltos : MonoBehaviour
{
    public Animator animator;

    public float velYR1;
    public float velRX1;

    public float velYR2;
    public float velRX2;

    public float velYR3;
    public float velRX3;

    public float velYL1;
    public float velLX1;

    public float velYL2;
    public float velLX2;

    public float velYL3;
    public float velXL3;

    public bool salto1R;
    public bool salto2R;
    public bool salto3R;
    public bool salto1L;
    public bool salto2L;
    public bool salto3L;

    Rigidbody2D rb;

    private bool muerto;
    private bool tocando_suelo;

    // Start is called before the first frame update
    void Awake()
    {
        salto1R = false;
        muerto = false;
        rb = GetComponent<Rigidbody2D>();

        GameManager.enemiesInScene++;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.reseting)
            Die();

        if (!muerto)
        {
            if (salto1R)
            {
                rb.AddForce(Vector2.up * velYR1);
                rb.AddForce(Vector2.right * velRX1);
                salto1R = false;
                tocando_suelo = false;
                animator.SetBool("suelo", false);
            }

            else if (salto2R)
            {
                rb.AddForce(Vector2.up * velYR2);
                rb.AddForce(Vector2.right * velRX2);
                salto2R = false;
                tocando_suelo = false;
                animator.SetBool("suelo", false);
            }

            else if (salto3R)
            {
                rb.AddForce(Vector2.up * velYR3);
                rb.AddForce(Vector2.right * velRX3);
                salto3R = false;
                tocando_suelo = false;
                animator.SetBool("suelo", false);
            }

            else if (salto1L)
            {
                rb.AddForce(Vector2.up * velYL1);
                rb.AddForce(Vector2.left * velLX1);
                salto1L = false;
                tocando_suelo = false;
                animator.SetBool("suelo", false);
            }

            else if (salto2L)
            {
                rb.AddForce(Vector2.up * velYL2);
                rb.AddForce(Vector2.left * velLX2);
                salto2L = false;
                tocando_suelo = false;
                animator.SetBool("suelo", false);
            }

            else if (salto3L)
            {
                rb.AddForce(Vector2.up * velYL3);
                rb.AddForce(Vector2.left * velXL3);
                salto3L = false;
                tocando_suelo = false;
                animator.SetBool("suelo", false);
            }

            /*if (rb.velocity.x >= 0)
                animator.SetBool("izquierda", false);

            else
                animator.SetBool("izquierda", true);
                */
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "plataforma")
        {
            tocando_suelo = true;
            animator.SetBool("suelo", true);
            rb.velocity = new Vector3(0f, 0f, 0f);
            int dir;

            if (collision.gameObject.name == "plat_1")
            {
                this.transform.position = new Vector3(-6.2f, this.transform.position.y, this.transform.position.z);
                dir = Random.Range(0, 2);

                if (dir == 0)
                {
                    salto1R = true;
                    animator.SetBool("izquierda", false);
                }

                else
                {
                    salto3R = true;
                    animator.SetBool("izquierda", false);
                }
            }

            if (collision.gameObject.name == "plat_2")
            {
                this.transform.position = new Vector3(-0f, this.transform.position.y, this.transform.position.z);
                dir = Random.Range(0, 2);

                if (dir == 0)
                {
                    animator.SetBool("izquierda", false);
                    salto2R = true;
                }

                else
                {
                    animator.SetBool("izquierda", true);
                    salto1L = true;
                }
            }

            if (collision.gameObject.name == "plat_3")
            {
                this.transform.position = new Vector3(6.2f, this.transform.position.y, this.transform.position.z);
                dir = Random.Range(0, 2);

                if (dir == 0)
                {
                    salto3L = true;
                    animator.SetBool("izquierda", true);
                }

                else
                {
                    salto2L = true;
                    animator.SetBool("izquierda", true);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Needel")
        {
            Die();
        }
    }

    void Die()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().PlayDestroyEnemySound();
        animator.SetTrigger("aguja");
        rb.velocity = new Vector2(0, 0);
        rb.isKinematic = true;
        muerto = true;
        GameManager.enemiesInScene--;
        GetComponent<CapsuleCollider2D>().enabled = false;
        Destroy(this.gameObject, 0.5f);
    }
}
