using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthScript : MonoBehaviour {

    private int Layer;
    private GameObject Particle;
    private Color c;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Layer) {
            GameControl.Instance.EatCandy(collision.gameObject);
            var a = Instantiate(Particle, transform.position, Quaternion.identity);
            var b = a.GetComponent<ParticleSystem>().main;
            b.startColor = c;
        }
    }

    public void SetColor(Color color)
    {
        c = color;
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
