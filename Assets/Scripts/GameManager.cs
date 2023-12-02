using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int seleccion = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int DameNumeroPet()
    {
        return seleccion;
    }

    public void SetSeleccion(int _seleccion)
    {
        seleccion = _seleccion;
    }
}
