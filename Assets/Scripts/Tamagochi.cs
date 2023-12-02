using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tamagochi : MonoBehaviour
{
    [SerializeField]
    private float comidaTiempo, limpiezaTiempo, sueñoTiempo;

    [SerializeField]
    private Scrollbar comidaBar, limpiezaBar, sueñoBar;

    [SerializeField]
    private Button comidaBoton, limpiezaBoton, sueñoBoton;

    [SerializeField]
    [Range(0.01f,1.0f)]
    private float cantidadComida, cantidadLimpieza, cantidadSueño;

    [SerializeField]
    private AudioClip comerSonido, dormirSonido, bañoSonido;

    [SerializeField]
    private int tamañoBar;

    [SerializeField]
    private ParticleSystem zPTC, corazonesPTC, burbujasPTC;

    [SerializeField]
    private GameObject pet1, pet2, pet3, pet4;

    private AudioSource audioSource;

    private bool conSueño = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();  

        comidaBar.numberOfSteps = tamañoBar;
        limpiezaBar.numberOfSteps = tamañoBar;
        sueñoBar.numberOfSteps = tamañoBar;
        SetBotones();
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
    }


    private void Start()
    {
        InvokeRepeating(nameof(BajaNecesidadComida), comidaTiempo, comidaTiempo);
        InvokeRepeating(nameof(BajaNecesidadLimpieza), limpiezaTiempo, limpiezaTiempo);
        InvokeRepeating(nameof(BajaNecesidadSueño), sueñoTiempo, sueñoTiempo);
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
    

}
