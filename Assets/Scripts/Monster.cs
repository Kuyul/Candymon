using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    //Declare Public Variables
    public GameObject Open;
    public GameObject Closed;
    public MouthScript Mouth;
    public GameObject BounceOff;
    public GameObject Particle;

    //Declare Private Variables
    private PointEffector2D pe;
    private SpriteRenderer sr;

    void Start()
    {
        pe = GetComponent<PointEffector2D>();
        sr = GetComponent<SpriteRenderer>();
        Mouth.SetParticle(Particle);
    }

    public void SetLayerProperties(int layerNumber)
    {
        pe.colliderMask = LayerMask.GetMask(LayerMask.LayerToName(layerNumber));
        Mouth.SetLayer(layerNumber);
    }

    public void SetColor(Color color)
    {
        sr.color = color;
    }
}
