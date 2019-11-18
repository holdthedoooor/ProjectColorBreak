using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Obstacle[]    obstacles;
    public Item[]        items;

    [HideInInspector]
    public Coroutine coroutine;

    void Awake()
    {
        obstacles = transform.GetComponentsInChildren<Obstacle>();
        items = transform.GetComponentsInChildren<Item>();
    }

    void Start()
    {
        StageManager.instance.StartStage();
    }
}
