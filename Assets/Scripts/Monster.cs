using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public GameObject Open;
    public GameObject Closed;
    public MouthScript Mouth;
    public GameObject BounceOff;
    public int layer;

    void Start()
    {
        Mouth.SetLayer(layer);
    }
}
