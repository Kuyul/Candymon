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
    private Animator anim;

    private void Start()
    {
        //Add animator to mouth so that it command it to animate when candies hit
        Mouth.SetAnimator(GetComponent<Animator>());
    }


    public void SetColor(Color color)
    {
        Mouth.SetColor(color);
    }

    //Called from the monster script class to pass down particle information down to mouthscript
    public void SetParticle(GameObject particle)
    {
        Mouth.SetParticle(particle);
        
    }

    public void SetLayerProperties(int layerNumber)
    {
        pe = GetComponent<PointEffector2D>();
        pe.colliderMask = LayerMask.GetMask(LayerMask.LayerToName(layerNumber));
        Mouth.SetLayer(layerNumber);
    }

    //Called from MonsterScript class to start eating animation
    public void Eating()
    {
        //anim.SetTrigger("Eating");
    }

    //Called from MonsterScript class to stop eating animation
    public void Done()
    {
        //anim.SetTrigger("Done");
    }
}
