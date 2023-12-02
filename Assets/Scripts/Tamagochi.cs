using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tamagochi : MonoBehaviour
{
    [SerializeField]
    private float comidaTiempo, limpiezaTiempo, sue�oTiempo;

    [SerializeField]
    private Scrollbar comidaBar, limpiezaBar, sue�oBar;

    [SerializeField]
    private Button comidaBoton, limpiezaBoton, sue�oBoton;

    [SerializeField]
    [Range(0.01f,1.0f)]
    private float cantidadComida, cantidadLimpieza, cantidadSue�o;

    [SerializeField]
    private AudioClip comerSonido, dormirSonido, ba�oSonido;

    [SerializeField]
    private int tama�oBar;

    [SerializeField]
    private ParticleSystem zPTC, corazonesPTC, burbujasPTC;

    [SerializeField]
    private GameObject pet1, pet2, pet3, pet4;

    private AudioSource audioSource;

    private bool conSue�o = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();  

        comidaBar.numberOfSteps = tama�oBar;
        limpiezaBar.numberOfSteps = tama�oBar;
        sue�oBar.numberOfSteps = tama�oBar;
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
        InvokeRepeating(nameof(BajaNecesidadSue�o), sue�oTiempo, sue�oTiempo);
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
    

}
