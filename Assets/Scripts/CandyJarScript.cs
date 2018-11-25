using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CandyJarScript : MonoBehaviour, IPointerDownHandler
{
    //Declare public variables
    public GameObject Sprite;

    //Declare private variables
    private CircleCollider2D cc;
    private PointEffector2D pe;
    private CandyInfo Candy;
    private bool Popped = false;

    // Use this for initialization
    void Start () {
        cc = GetComponent<CircleCollider2D>();
        pe = GetComponent<PointEffector2D>();
    }
	
	public void SetCandy(CandyInfo candy)
    {
        Candy = candy;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pop();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!Popped)
        {
            Popped = true;
            cc.isTrigger = true; //Starts off as a collider it must be changed to a trigger for pointeffector to become active
            Pop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "JarConvert")
        {
            cc.isTrigger = false;
        }
    }

    public void Pop()
    {
        //Spawn Level + 4 amount of Candies
        int amount = Candy.GetCandyCount();
        for (int i = 0; i < amount; i++)
        {
            //Spawn them on the bag's position
            var newPos = transform.position;
            newPos.x += Random.Range(-0.01f, 0.01f);
            newPos.y += Random.Range(-0.01f, 0.01f);
            Candy.InstantiateAtPos(newPos);
        }

        //activate point effector for cool physics effect in action
        Sprite.SetActive(false);
        pe.forceMagnitude = 30;
        Destroy(this.gameObject, 0.1f);
    }
}
