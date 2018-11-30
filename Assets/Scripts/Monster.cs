using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    //Declare Public Variables
    public GameObject Open;
    public GameObject Closed;
    public MouthScript Mouth;
    public GameObject BounceOff;

    //Declare Private Variables
    private PointEffector2D pe;
    private SpriteRenderer sr;

    void Start()
    {
        pe = GetComponent<PointEffector2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetColor(Color color)
    {
        Mouth.SetColor(color);
    }

    public void SetLayerProperties(int layerNumber)
    {
        pe.colliderMask = LayerMask.GetMask(LayerMask.LayerToName(layerNumber));
        Mouth.SetLayer(layerNumber);
    }

    public void SetParticle(GameObject particle)
    {
        Mouth.SetParticle(particle);
    }
}
