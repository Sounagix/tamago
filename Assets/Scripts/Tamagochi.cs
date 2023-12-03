using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Tamagochi : MonoBehaviour
{
    [SerializeField]
    private float comidaTiempo, limpiezaTiempo, sueñoTiempo, entretenimientoTiempo;

    [SerializeField]
    private Scrollbar comidaBar, limpiezaBar, sueñoBar, entretenimientoBar;

    [SerializeField]
    private Button comidaBoton, limpiezaBoton, sueñoBoton, entretenimientoBoton, movDerecha, movIzquierda;

    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float cantidadComida, cantidadLimpieza, cantidadSueño, cantidadEntretenimiento;

    [SerializeField]
    private float velocidadMov;

    [SerializeField]
    private AudioClip comerSonido, dormirSonido, bañoSonido, piñaImpacto, carameloImpacto;

    [SerializeField]
    private int tamañoBar;

    [SerializeField]
    private ParticleSystem zPTC, corazonesPTC, burbujasPTC;

    [SerializeField]
    private GameObject pet1, pet2, pet3, pet4;

    private AudioSource audioSource;

    private bool conSueño = false;

    private Rigidbody rb;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();  
        rb = GetComponent<Rigidbody>();

        comidaBar.numberOfSteps = tamañoBar;
        limpiezaBar.numberOfSteps = tamañoBar;
        sueñoBar.numberOfSteps = tamañoBar;
        entretenimientoBar.numberOfSteps = tamañoBar;
        SetBotones();

    }


    private void Start()
    {
        switch (GameManager.instance.DameNumeroPet())
        {
            case 1:
                pet1.SetActive(true);
                break;
            case 2:
                pet2.SetActive(true);
                break;
            case 3:
                pet3.SetActive(true);
                break;
            case 4:
                pet4.SetActive(true);
                break;
            default:
                pet1.SetActive(true);
                break;

        }

        InvokeRepeating(nameof(BajaNecesidadComida), comidaTiempo, comidaTiempo);
        InvokeRepeating(nameof(BajaNecesidadLimpieza), limpiezaTiempo, limpiezaTiempo);
        InvokeRepeating(nameof(BajaNecesidadSueño), sueñoTiempo, sueñoTiempo);
        InvokeRepeating(nameof(BajaNecesidadEntretenimiento), entretenimientoTiempo, entretenimientoTiempo);
    }

    private void Update()
    {   
        if (!conSueño && sueñoBar.size < 0.25f)
        {
            conSueño = true;
            audioSource.clip = dormirSonido;
            audioSource.loop = true;
            audioSource.Play();
            zPTC.Play();
        }
    }

    // Si la pet se sale de los límites se setea a la posición máxima para evitar salidas de la pantalla
    private void LateUpdate()
    {
        if (Mathf.Abs(transform.position.x) >= 3.0f)
        {
            rb.velocity = Vector3.zero;
            Vector3 pos = transform.position;
            pos.x = Mathf.Sign(transform.position.normalized.x) * 3.0f;
            transform.position = pos;
        }
    }

    private void SetBotones()
    {
        comidaBoton.onClick.AddListener(
            delegate ()
            {
                audioSource.PlayOneShot(comerSonido);
                comidaBar.size += cantidadComida;
                corazonesPTC.Play();
            });
        sueñoBoton.onClick.AddListener(
            delegate ()
            {
                sueñoBar.size += cantidadSueño;
                if (sueñoBar.size >= 0.25f )
                {
                    conSueño = false;
                    audioSource.loop = false;
                    audioSource.Stop();
                    zPTC.Stop();
                }
            });
        limpiezaBoton.onClick.AddListener(
            delegate ()
            {
                audioSource.PlayOneShot(bañoSonido);
                limpiezaBar.size += cantidadLimpieza;
                burbujasPTC.Play();
            });
        //entretenimientoBoton.onClick.AddListener(
        //    delegate ()
        //    {
        //        entretenimientoBar.size += cantidadEntretenimiento;
        //    });
        movDerecha.onClick.AddListener(
            delegate ()
            {
                MuevePlayer(Vector3.right);
            });
        movIzquierda.onClick.AddListener(
            delegate ()
            {
                MuevePlayer(Vector3.left);
            });
    }

    private void MuevePlayer(Vector3 dir)
    {
        rb.AddForce(dir * velocidadMov, ForceMode.Impulse);
        //transform.position = transform.position + (dir * velocidadMov);
    }

    private void BajaNecesidadComida()
    {
        comidaBar.size -= 0.1f;
    }

    private void BajaNecesidadSueño()
    {
        sueñoBar.size -= 0.1f;
    }

    private void BajaNecesidadLimpieza()
    {
        limpiezaBar.size -= 0.1f;
    }

    private void BajaNecesidadEntretenimiento()
    {
        entretenimientoBar.size -= 0.1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        string collTag = collision.gameObject.tag;
        if (collTag == "Piña" || collTag == "Caramelo")
        {
            Destroy(collision.gameObject);
            if (entretenimientoBar.size < 1.0f)
            {
                entretenimientoBar.size += collTag == "Piña" ? (-cantidadEntretenimiento) : cantidadEntretenimiento;
                audioSource.PlayOneShot(collTag == "Piña" ? piñaImpacto : carameloImpacto);
            }
        }
    }
}
