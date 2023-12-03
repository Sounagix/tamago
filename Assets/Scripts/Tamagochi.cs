using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Tamagochi : MonoBehaviour
{
    [SerializeField]
    private float comidaTiempo, limpiezaTiempo, sue�oTiempo, entretenimientoTiempo;

    [SerializeField]
    private Scrollbar comidaBar, limpiezaBar, sue�oBar, entretenimientoBar;

    [SerializeField]
    private Button comidaBoton, limpiezaBoton, sue�oBoton, entretenimientoBoton, movDerecha, movIzquierda;

    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float cantidadComida, cantidadLimpieza, cantidadSue�o, cantidadEntretenimiento;

    [SerializeField]
    private float velocidadMov;

    [SerializeField]
    private AudioClip comerSonido, dormirSonido, ba�oSonido, pi�aImpacto, carameloImpacto;

    [SerializeField]
    private int tama�oBar;

    [SerializeField]
    private ParticleSystem zPTC, corazonesPTC, burbujasPTC;

    [SerializeField]
    private GameObject pet1, pet2, pet3, pet4;

    private AudioSource audioSource;

    private bool conSue�o = false;

    private Rigidbody rb;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();  
        rb = GetComponent<Rigidbody>();

        comidaBar.numberOfSteps = tama�oBar;
        limpiezaBar.numberOfSteps = tama�oBar;
        sue�oBar.numberOfSteps = tama�oBar;
        entretenimientoBar.numberOfSteps = tama�oBar;
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
        InvokeRepeating(nameof(BajaNecesidadSue�o), sue�oTiempo, sue�oTiempo);
        InvokeRepeating(nameof(BajaNecesidadEntretenimiento), entretenimientoTiempo, entretenimientoTiempo);
    }

    private void Update()
    {   
        if (!conSue�o && sue�oBar.size < 0.25f)
        {
            conSue�o = true;
            audioSource.clip = dormirSonido;
            audioSource.loop = true;
            audioSource.Play();
            zPTC.Play();
        }
    }

    // Si la pet se sale de los l�mites se setea a la posici�n m�xima para evitar salidas de la pantalla
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
        sue�oBoton.onClick.AddListener(
            delegate ()
            {
                sue�oBar.size += cantidadSue�o;
                if (sue�oBar.size >= 0.25f )
                {
                    conSue�o = false;
                    audioSource.loop = false;
                    audioSource.Stop();
                    zPTC.Stop();
                }
            });
        limpiezaBoton.onClick.AddListener(
            delegate ()
            {
                audioSource.PlayOneShot(ba�oSonido);
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

    private void BajaNecesidadSue�o()
    {
        sue�oBar.size -= 0.1f;
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
        if (collTag == "Pi�a" || collTag == "Caramelo")
        {
            Destroy(collision.gameObject);
            if (entretenimientoBar.size < 1.0f)
            {
                entretenimientoBar.size += collTag == "Pi�a" ? (-cantidadEntretenimiento) : cantidadEntretenimiento;
                audioSource.PlayOneShot(collTag == "Pi�a" ? pi�aImpacto : carameloImpacto);
            }
        }
    }
}
