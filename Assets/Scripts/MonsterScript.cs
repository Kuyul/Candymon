using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonsterScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //Declare UI Components
    public Text LevelText;
    public Text ExpText;
    public Slider ExpBar;

    //Declare public variables
    public GameObject[] Monsters;

    //Declare private variables
    private List<Monster> MonsterProperties = new List<Monster>(); 

    // Use this for initialization
    void Start () {
        for (int i = 0; i < Monsters.Length; i++)
        {
            var lvl = GameControl.Instance.GetCandyInfos()[i].GetLevel(); //Only activate a certain monster if the corresponding candy lvl >= 1
            if (lvl >= 1) {
                Monsters[i].SetActive(true);
            }
            else
            {
                Monsters[i].SetActive(false);
            }
            MonsterProperties.Add(Monsters[i].GetComponent<Monster>());
            MonsterProperties[i].Open.SetActive(false);
            MonsterProperties[i].Closed.SetActive(true);
        }
    }

    public void ActivateMonster(int i)
    {
        Monsters[i].SetActive(true);
    }

    //When mouth is open: 
    //1. point effector is enabled
    //2. Mouth Open Sprite is active
    //Vice versa
    private void SetMouthOpen(bool b)
    {
        for (int i = 0; i < Monsters.Length; i++)
        {
            Monsters[i].GetComponent<PointEffector2D>().enabled = b;
            MonsterProperties[i].Open.SetActive(b);
            MonsterProperties[i].Closed.SetActive(!b);
            MonsterProperties[i].BounceOff.SetActive(!b);
        }
    }

    //On clicking the monster, its mouth will open and absorb all the candies on the screen
    public void OnPointerDown(PointerEventData eventData)
    {
        SetMouthOpen(true);
    }

    //On mousebutton up, its mouth will close
    public void OnPointerUp(PointerEventData eventData)
    {
        SetMouthOpen(false);
        GameControl.Instance.FinishAccumulating();
    }
}
