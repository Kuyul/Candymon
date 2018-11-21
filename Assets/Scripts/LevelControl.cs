using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelControl : MonoBehaviour {

    //Declare public variables
    public CandyInfo[] Candies;
    public Text[] CandyLevelTexts; //UI that represents candy levels
    public Text[] CandyCostTexts; //UI that represents candy costs
    public Collider2D Spawn;
    public Collider2D NoSpawn;

    //Declare private variables
    private Camera cam;
    private float MaxX;
    private float MaxY;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < Candies.Length; i++)
        {
            Candies[i].Initialise(CandyLevelTexts[i], CandyCostTexts[i]);
            StartCoroutine(CreateCandy(i));
        }
    }

    //This method is used from the UI buttons and its invoked when clicked.
    //takes candy index as an input parameter, and levels up the candy.
    public void LevelUpCandy(int i)
    {
        Candies[i].Upgrade();
    }

    //Instantiates a candy gameobject of a given candy level in a given boundary
    IEnumerator CreateCandy(int i)
    {
        while (true)
        {
            if (Candies[i].GetLevel() > 0)
            {
                float seconds = 1.0f / Candies[i].GetLevel();
                yield return new WaitForSeconds(seconds);
                var position = GetValidPosition();
                var candy = Candies[i].InstantiateAtPos(position);
                GameControl.Instance.AddCandy(candy);
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    //Temporary
    public void DeleteAllPlayerprefs()
    {
        PlayerPrefs.DeleteAll();
        for (int i = 0; i < Candies.Length; i++)
        {
            Candies[i].SetLevel();
            CandyLevelTexts[i].text = Candies[i].GetLevel() + "/10";
        }
    }

    //Called from the CreateCandy couroutine to get a valid spawnpoint
    private Vector3 GetValidPosition()
    {
        //Candy must be generated within Spawn boundary, but it must be generated outside the No spawn boundary at the same time
        //Below code handles the described logic
        var xPos = Random.Range(Spawn.bounds.min.x, Spawn.bounds.max.x);
        var yPos = Random.Range(Spawn.bounds.min.y, Spawn.bounds.max.y);
        //Check if its within the nospawn box
        bool xBound = NoSpawn.bounds.min.x < xPos && NoSpawn.bounds.max.x > xPos;
        bool yBound = NoSpawn.bounds.min.y < yPos && NoSpawn.bounds.max.y > yPos;

        //I don't know how to make this one smarter....
        while (xBound && yBound)
        {
            xPos = Random.Range(Spawn.bounds.min.x, Spawn.bounds.max.x);
            yPos = Random.Range(Spawn.bounds.min.y, Spawn.bounds.max.y);
            xBound = NoSpawn.bounds.min.x < xPos && NoSpawn.bounds.max.x > xPos;
            yBound = NoSpawn.bounds.min.y < yPos && NoSpawn.bounds.max.y > yPos;
        }
        var position = new Vector3(xPos, yPos, 0);
        return position;
    }
}
