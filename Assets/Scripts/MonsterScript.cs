using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonsterScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    //Declare public variables
    public GameObject MouthOpen;
    public GameObject MouthClosed;
    public float Speed = 100;

    //Declare private variables
    private bool IsMouthOpen = false;
    private PointEffector2D PE;

	// Use this for initialization
	void Start () {
        MouthOpen.SetActive(false);
        MouthClosed.SetActive(true);

        PE = GetComponent<PointEffector2D>();

    }
	
	// Update is called once per frame
	void Update () {
        if (IsMouthOpen) {
            /*
            var count = GameControl.Instance.GetCandies().Count;
            for (int i = 0; i < count; i++) {
                //For each active candy on the screen, suck them into the monster's mouth
                var candyObj = GameControl.Instance.GetCandies()[i];
                var pos = candyObj.GetComponent<Transform>().position;
                var origin = new Vector3(0,0,0);
                candyObj.GetComponent<Transform>().position = Vector3.MoveTowards(pos, origin, Speed * Time.deltaTime);
                if (pos == origin) {
                    GameControl.Instance.RemoveCandy(candyObj);
                    count--;
                }
            }*/
        }
	}

    //On clicking the monster, its mouth will open and absorb all the candies on the screen
    public void OnPointerDown(PointerEventData eventData)
    {
        SetMouthOpen(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetMouthOpen(false);
    }

    private void SetMouthOpen(bool b)
    {
        IsMouthOpen = b;
        PE.enabled = b;
        MouthOpen.SetActive(b);
        MouthClosed.SetActive(!b);
    }
}
