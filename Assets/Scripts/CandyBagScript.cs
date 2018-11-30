using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CandyBagScript : MonoBehaviour, IPointerDownHandler
{
    //Declare public variables
    public GameObject Sprite;

    //Declare private variables
    private PointEffector2D pe;
    private CandyInfo Candy;
    private bool Popped = false;

    //Declare private variables
    void Start()
    {
        pe = GetComponent<PointEffector2D>();
    }

    public void SetCandy(CandyInfo candy)
    {
        Candy = candy;
        Invoke("Pop", 8);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Popped)
        {
            Pop();
        }
    }

    public void Pop()
    {
        Instantiate(GameControl.Instance.bagPop, transform.position, Quaternion.identity);
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
        pe.forceMagnitude = 15;
        Destroy(this.gameObject, 0.1f);

        //Decrement total number of bag count
        GameControl.Instance.DecrementBagCount();
    }
}
