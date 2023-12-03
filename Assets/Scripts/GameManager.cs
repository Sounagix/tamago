using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private static int seleccion;

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
        print(seleccion);
        return seleccion;
    }

    public void SetSeleccion(int _seleccion)
    {
        seleccion = _seleccion;
        print(seleccion);
    }
}
