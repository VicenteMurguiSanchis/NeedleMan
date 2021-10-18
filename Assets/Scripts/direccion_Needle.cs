using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class direccion_Needle : MonoBehaviour
{
    // Start is called before the first frame update

    float contador_animacion = 0.5f;
    private float contador;
    public SpriteRenderer imagen;
    bool ataque;
    float vel_needel = 20f;
    Vector3 direccion_lanzamiento;
    Vector3 velocidad;
    public Transform personaje;
    public character_controller control_jugador;
    public bool delay_act;
    public float contador_delay;
    public float contador2;

    float timerFuerza;
    float timerCont;

    [SerializeField] LineRenderer rope;

    AudioSource audioSource;
    [SerializeField] AudioClip lanzarAguja;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        ataque = false;
        delay_act = false;
        imagen.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (personaje.GetComponent<character_controller>().muerto) return;

        timerFuerza -= Time.deltaTime;
        timerCont -= Time.deltaTime;

        rope.SetPosition(0, personaje.position);
        rope.SetPosition(1, transform.position);

        if (!ataque)
        {
            var dir = new Vector2();

            if(GameObject.FindObjectOfType<GameManager>().paraMando)
                dir = personaje.GetComponent<character_controller>().needelTarget.position - transform.position;
            else
                dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.position = new Vector3(personaje.position.x, personaje.position.y, 10);
        }

        else
        {
            contador += Time.deltaTime;
            if(contador <= contador_animacion/2)
            {
                transform.position = transform.position + velocidad * Time.deltaTime;
            }

            else if(contador > contador_animacion/2 && contador < contador_animacion)
            {
                transform.position = transform.position - velocidad * Time.deltaTime;
            }

            else
            {
                ataque = false;
                contador = 0;
                transform.position = new Vector3(personaje.position.x, personaje.position.y, 10);
                control_jugador.atacando = false;
                GetComponent<BoxCollider2D>().enabled = false;
                //if(imagen.enabled)
                imagen.enabled = false;
            }
        }

        if(delay_act)
        {
            contador2 += Time.deltaTime;
            if (contador2 >= contador_delay)
            {
                delay_act = false;
                contador2 = 0;
            }
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && !ataque && !delay_act && Time.timeScale == 1 && !control_jugador.muerto)
        {
            audioSource.PlayOneShot(lanzarAguja);
            personaje.GetComponent<character_controller>().cantMove = false;
            imagen.enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
            ataque = true;
            control_jugador.atacando = true;
            var pos_raton = personaje.GetComponent<character_controller>().needelTarget.position/*Camera.main.ScreenToWorldPoint(Input.mousePosition)*/;
            Debug.DrawLine(new Vector3(personaje.position.x, personaje.position.y, -1f), new Vector3(pos_raton.x, pos_raton.y, -1), Color.red, 2.5f);
            direccion_lanzamiento = new Vector3(pos_raton.x, pos_raton.y, 10) - new Vector3(personaje.position.x, personaje.position.y, 10);
            direccion_lanzamiento.Normalize();
            velocidad = new Vector3(direccion_lanzamiento.x * vel_needel, direccion_lanzamiento.y * vel_needel, 0f);
            delay_act = true;
        }

        if(timerCont <= 0)
        {
            //imagen.enabled = true;
            rope.enabled = true;
        }
        
    }

    public void Shot()
    {
        if (personaje.GetComponent<character_controller>().muerto) return;

        if (/*Input.GetKeyDown(KeyCode.Mouse0) &&*/ !ataque && !delay_act && Time.timeScale == 1 && !control_jugador.muerto)
        {
            personaje.GetComponent<character_controller>().cantMove = false;
            imagen.enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
            ataque = true;
            control_jugador.atacando = true;
            var pos_raton = personaje.GetComponent<character_controller>().needelTarget.position/*Camera.main.ScreenToWorldPoint(Input.mousePosition)*/;
            Debug.DrawLine(new Vector3(personaje.position.x, personaje.position.y, -1f), new Vector3(pos_raton.x, pos_raton.y, -1), Color.red, 2.5f);
            direccion_lanzamiento = new Vector3(pos_raton.x, pos_raton.y, 10) - new Vector3(personaje.position.x, personaje.position.y, 10);
            direccion_lanzamiento.Normalize();
            velocidad = new Vector3(direccion_lanzamiento.x * vel_needel, direccion_lanzamiento.y * vel_needel, 0f);
            delay_act = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enganche" && timerFuerza <= 0)
        {
            timerCont = 0.6f;

            audioSource.PlayOneShot(lanzarAguja);
            imagen.enabled = false;
            if (rope.enabled)
                rope.enabled = false;
            //ataque = false;
            contador = 0;
            transform.position = new Vector3(personaje.position.x, personaje.position.y, 10);
            //control_jugador.atacando = false;
            GetComponent<BoxCollider2D>().enabled = false;

            Vector2 forceDir = collision.transform.position - personaje.transform.position;
            personaje.GetComponent<character_controller>().enganchado = true;
            personaje.GetComponent<Rigidbody2D>().AddForce(forceDir.normalized * 1050);

            timerFuerza = 0.5f;
            //Vector2 forceDir = collision.transform.position - personaje.transform.position;
        }
    }

}
