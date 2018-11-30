using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthScript : MonoBehaviour {

    private int Layer;
    private GameObject Particle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Layer) {
            GameControl.Instance.EatCandy(collision.gameObject);
            //TODO: create particle
        }
    }

    public void SetLayer(int l)
    {
        Layer = l;
    }

    public void SetParticle(GameObject particle)
    {
        Particle = particle;
    }
}
