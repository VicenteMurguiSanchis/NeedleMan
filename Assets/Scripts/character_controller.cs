using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class character_controller : MonoBehaviour
{
    // Start is called before the first frame update
    public float alturaSalto;
    public float movimiento;
    private bool salto;
    public bool atacando;
    public bool enganchado;
    public bool muerto;

    public bool cantMove;

    public bool onGround;

    public direccion_Needle aguja;

    public Animator animator;

    [HideInInspector]public Transform needelTarget;
    Vector2 needelMove;

    public Rigidbody2D rb;
    [SerializeField] LayerMask groundMask;

    [SerializeField] Image[] hearts = new Image[3];
    [SerializeField] Sprite[] heartType = new Sprite[2];

    [SerializeField] Color[] colors = new Color[2];
    bool canBeHitted;

    int currentLife = 3;

    Controller controls;

    Vector3 whereSpawn;

    [SerializeField] GameObject[] titleSceneThings;
    [SerializeField] GameObject[] gamePlaySceneThings;
    [SerializeField] GameObject[] gameOverSceneThings;

    AudioSource audioSource;
    [SerializeField] AudioClip saltoSound;

    void Awake()
    {
        controls = new Controller();
        ControlesMando();

        audioSource = GetComponentInChildren<AudioSource>();

        whereSpawn = transform.position;

        colors[0] = GetComponent<SpriteRenderer>().color;

        salto = false;
        atacando = false;
        enganchado = false;
        cantMove = false;
        muerto = false;
        canBeHitted = true;
        rb = GetComponent<Rigidbody2D>();

        if (GameObject.FindObjectOfType<GameManager>().paraMando)
            needelTarget.position = this.transform.position + new Vector3(1, 0, 0);
        /*else
            needelTarget.gameObject.SetActive(false);*/
        //startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (muerto) return;

        HeartsImages();

        if (GameObject.FindObjectOfType<GameManager>().paraMando)
        {
            Vector2 n = new Vector2(needelMove.x, needelMove.y).normalized;
            if (n.magnitude >= 0.3f)
                needelTarget.position = this.transform.position + (Vector3)n * 3;
        }
        else
        {
            needelTarget.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 10);
        }



        //Physics2D.IgnoreLayerCollision(8, 9);

        /*if(rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-startScale.x, startScale.y, startScale.z);
        }

        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(startScale.x, startScale.y, startScale.z);
        }*/

        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            cantMove = false;
        }

        if (!atacando)
        {

            animator.SetBool("D_E", atacando);
            animator.SetBool("D_NE", atacando);
            animator.SetBool("D_N", atacando);
            animator.SetBool("D_NW", atacando);
            animator.SetBool("D_W", atacando);
            animator.SetBool("D_SW", atacando);
            animator.SetBool("D_S", atacando);
            animator.SetBool("D_SE", atacando);

            movimiento_Axis();

            if (Input.GetKeyDown(KeyCode.Space) && !salto && Time.timeScale == 1f && onGround)
            {
                audioSource.PlayOneShot(saltoSound);
                //onGround = false;
                rb.AddForce(Vector2.up * alturaSalto);
                salto = true;
                animator.SetBool("aire", salto);
            }
        }
        

        else
        {
            if(!enganchado)
                rb.velocity = new Vector2(0, 0);

            Vector3 rot_actual = Quaternion.ToEulerAngles(aguja.transform.rotation);
            rot_actual *= Mathf.Rad2Deg;
            Debug.Log(rot_actual);
            
            /****** Los animator que salen en verde son aquellos que aun no se han podido implementar al juego *********/

            if (rot_actual.z >= -22.5f && rot_actual.z < 22.5f)
            {
                animator.SetBool("D_E", atacando);
            }

            else if (rot_actual.z >= 22.5f && rot_actual.z < 67.5f)
            {
                animator.SetBool("D_NE", atacando);
                //animator.SetBool("D_E", atacando);
            }

            else if (rot_actual.z >= 67.5f && rot_actual.z < 112.5f)
            {
                animator.SetBool("D_N", atacando);
            }

            else if (rot_actual.z >= 112.5f && rot_actual.z < 157.5f)
            {
                animator.SetBool("D_NW", atacando);
                //animator.SetBool("D_W", atacando);
            }

            else if (rot_actual.z >= 157.5f && rot_actual.z <= 180f)
            {
                animator.SetBool("D_W", atacando);
            }

            else if (rot_actual.z <= -22.5f && rot_actual.z > -67.5f)
            {
                animator.SetBool("D_SE", atacando);
                //animator.SetBool("D_E", atacando);
            }

            else if (rot_actual.z <= -67.5f && rot_actual.z > -90f)
            {
                animator.SetBool("D_S", atacando);
            }

            else if (rot_actual.z <= -90.5f && rot_actual.z > -157.5f)
            {
                animator.SetBool("D_SW", atacando);
                //animator.SetBool("D_W", atacando);
            }

            else if (rot_actual.z <= -157.5f && rot_actual.z >= -180f)
            {
                animator.SetBool("D_W", atacando);
            }
        }

        if (enganchado && !atacando)
            enganchado = false;

        if (enganchado)
            cantMove = true;

        if (onGround && cantMove)
            cantMove = false;

        /*
        if (Input.GetKey (KeyCode.D))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(movimiento, GetComponent<Rigidbody2D>().velocity.y);
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-movimiento, GetComponent<Rigidbody2D>().velocity.y);

        }
        */
    }

    void ControlesMando()
    {
        controls.GamePlay.Jump.performed += ctx => Jump();
        controls.GamePlay.Shot.performed += ctx => GetComponentInChildren<direccion_Needle>().Shot();
        controls.GamePlay.Aim.performed += ctx => needelMove = ctx.ReadValue<Vector2>();
    }

    void Jump()
    {
        if (/*Input.GetKeyDown(KeyCode.Space) &&*/ !salto && Time.timeScale == 1f && onGround)
        {
            audioSource.PlayOneShot(saltoSound);
            //onGround = false;
            rb.AddForce(Vector2.up * alturaSalto);
            salto = true;
            animator.SetBool("aire", salto);
        }
    }

    private void FixedUpdate()
    {
        onGround = Physics2D.OverlapCircle(new Vector3(transform.position.x, transform.position.y - 0.83f, transform.position.z), 0.2f, groundMask);
    }

    void movimiento_Axis()
    {
        float translate_h = Input.GetAxisRaw("Horizontal") * movimiento;
        //translate_h *= Time.deltaTime;
        //transform.Translate(translate_h, 0, 0);
        if(!cantMove)
            rb.velocity = new Vector2(translate_h, rb.velocity.y);

        //rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -5, 5), rb.velocity.y);

        if(rb.velocity.x >= 0.01f)
        {
            if(!salto)
                animator.SetBool("mueve", true);

            else
                animator.SetBool("mueve", false);

            animator.SetBool("izquierda", false);
        }

        else if(rb.velocity.x <= -0.01f)
        {
            if (!salto)
                animator.SetBool("mueve", true);

            else
                animator.SetBool("mueve", false);

            animator.SetBool("izquierda", true);
        }

        else
        {
            animator.SetBool("mueve", false);
        }
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "plataforma")
        {
            salto = false;
            animator.SetBool("aire", salto);
        }

        else if(collision.gameObject.tag == "enemigo" && canBeHitted)
        {
            currentLife--;
            GameObject.Find("GameManager").GetComponent<GameManager>().PlayDestroyEnemySound();
            StartCoroutine(Hitted());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "plataforma")
        {
            animator.SetBool("aire", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Boundary")
        {
            transform.position = whereSpawn;
            rb.velocity = Vector2.zero;
            if (canBeHitted) {
                currentLife--;
                GameObject.Find("GameManager").GetComponent<GameManager>().PlayDestroyEnemySound();
            }
            
            StartCoroutine(Hitted());
        }
    }

    IEnumerator Hitted()
    {
        float toWait = 0.2f;
        
        canBeHitted = false;
        GetComponent<SpriteRenderer>().color = colors[1];
        yield return new WaitForSeconds(toWait);
        GetComponent<SpriteRenderer>().color = colors[0];
        yield return new WaitForSeconds(toWait);
        GetComponent<SpriteRenderer>().color = colors[1];
        yield return new WaitForSeconds(toWait);
        GetComponent<SpriteRenderer>().color = colors[0];
        yield return new WaitForSeconds(toWait);
        GetComponent<SpriteRenderer>().color = colors[1];
        yield return new WaitForSeconds(toWait);
        GetComponent<SpriteRenderer>().color = colors[0];
        canBeHitted = true;
    }

    void HeartsImages()
    {
        switch (currentLife)
        {
            case 0:
                hearts[0].sprite = heartType[0];
                hearts[1].sprite = heartType[0];
                hearts[2].sprite = heartType[0];
                muerto = true;
                cantMove = true;
                GameManager.reseting = true;

                StartCoroutine(Die());

                animator.SetTrigger("enemigo");
                break;
            case 1:
                hearts[0].sprite = heartType[1];
                hearts[1].sprite = heartType[0];
                hearts[2].sprite = heartType[0];
                break;
            case 2:
                hearts[0].sprite = heartType[1];
                hearts[1].sprite = heartType[1];
                hearts[2].sprite = heartType[0];
                break;
            case 3:
                hearts[0].sprite = heartType[1];
                hearts[1].sprite = heartType[1];
                hearts[2].sprite = heartType[1];
                break;
        }
    }

    private void Reset()
    {
        /*currentLife = 3;
        animator.SetTrigger("revivir");
        muerto = false;
        cantMove = false*/;
    }

    IEnumerator Die()
    {
        for (int i = 0; i < gameOverSceneThings.Length; i++)
        {
            gameOverSceneThings[i].SetActive(true);
        }
        /*for (int i = 0; i < gamePlaySceneThings.Length; i++)
        {
            gamePlaySceneThings[i].SetActive(false);
        }*/
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(2.0f);
        /*GameManager.reseting = true;

        for (int i = 0; i < titleSceneThings.Length; i++)
        {
            titleSceneThings[i].SetActive(true);
        }
        for (int i = 0; i < gameOverSceneThings.Length; i++)
        {
            gameOverSceneThings[i].SetActive(false);
        }*/
        SceneManager.LoadScene(0);
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