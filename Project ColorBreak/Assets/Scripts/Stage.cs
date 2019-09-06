using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Obstacle[]    obstacles;
    public Item[]        items;

    void Awake()
    {
        obstacles = transform.GetComponentsInChildren<Obstacle>();
        items = transform.GetComponentsInChildren<Item>();
    }

    void OnEnable()
    {
        StageManager.instance.StartStage();
    }

    
}
