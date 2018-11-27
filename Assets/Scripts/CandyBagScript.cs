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

    //Declare private variables
    void Start()
    {
        pe = GetComponent<PointEffector2D>();
    }

    public void SetCandy(CandyInfo candy)
    {
        Candy = candy;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pop();
        Instantiate(GameControl.Instance.bagPop, transform.position, Quaternion.identity);
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

        //Decrement total number of bag count
        GameControl.Instance.DecrementBagCount();
    }
}
