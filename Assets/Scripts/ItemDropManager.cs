using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    [SerializeField]
    private GameObject carameloPrefab, piñaPrefab;

    [SerializeField]
    private float dropTiempo, gravedadFactor;

    [SerializeField]
    private Vector2 minPos, maxPos;

    private void Awake()
    {
        Physics.gravity = new Vector3(0, gravedadFactor, 0);
    }

    private void Start()
    {
        InvokeRepeating(nameof(EmpiezaDrop), 0.0f, dropTiempo);
    }

    private void EmpiezaDrop()
    {
        int rnd = Random.Range(0,2);
        GameObject actualDrop = Instantiate(rnd == 0 ? carameloPrefab : piñaPrefab);
        actualDrop.transform.position = new Vector3(Random.Range(minPos.x, maxPos.x), minPos.y, 3.0f);
    }
}
