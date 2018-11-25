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
    public GameObject CandyBag;
    public int NumberOfBags = 8;

    //Bomb related components
    public GameObject CandyJar;
    public float SpawnFreq = 5.0f;
    public int NumberOfJars = 1;
    public Collider2D JarSpawn;

    //Declare private variables
    private Camera cam;
    private float MaxX;
    private float MaxY;
    private int BagCount = 0;
    private List<int> ActiveCandyTypes = new List<int>(); //List of candies between level 1 ~ 9
    private List<int> MaxCandyTypes = new List<int>(); //List of candies greater than or equal to level 10

	// Use this for initialization
	void Start () {
        for (int i = 0; i < Candies.Length; i++)
        {
            Candies[i].Initialise(CandyLevelTexts[i], CandyCostTexts[i]); //Initialise candy info stuff
        }
        PopulateCandyTypes();
        StartCoroutine(RunBombs());
    }

    private void Update()
    {
        if (BagCount == 0)
        {
            for (int i = 0; i < NumberOfBags; i++)
            {
                //Create a random candy bag from active candy types array
                if (ActiveCandyTypes.Count > 0)
                {
                    CreateBag(ActiveCandyTypes[Random.Range(0, ActiveCandyTypes.Count)]);
                }
                else
                {
                    CreateBag(MaxCandyTypes[Random.Range(0, MaxCandyTypes.Count)]);
                }
            }
        }
    }

    //Organise candies into two different criteria based on its level
    private void PopulateCandyTypes()
    {
        ActiveCandyTypes.Clear();
        MaxCandyTypes.Clear();

        for(int i = 0; i < Candies.Length; i++)
        {
            var level = Candies[i].GetLevel();
            if (level > 0 && level < 10) //candy types of levels between 1~9
            {
                ActiveCandyTypes.Add(Candies[i].Type);
            }else if(level >= 10) //Maxed out candies
            {
                MaxCandyTypes.Add(Candies[i].Type);
            }
        }
    }

    //This method is used from the UI buttons and its invoked when clicked.
    //takes candy index as an input parameter, and levels up the candy.
    public void LevelUpCandy(int i)
    {
        var success = Candies[i].Upgrade();
        if(success)
        PopulateCandyTypes();
    }

    //Spawn a candybag on a random spawn point
    private void CreateBag(int i)
    {
        var position = GetValidPosition(Spawn);
        position.z = -2.0f; //Bag must be in front of monster click
        var obj = Instantiate(CandyBag, position, Quaternion.identity);
        BagCount++; //increment counter
        obj.GetComponent<CandyBagScript>().SetCandy(Candies[i]);
    }

    //For maxed out candies
    IEnumerator RunBombs()
    {
        while (true) {
            yield return new WaitForSeconds(SpawnFreq);
            //Bombs are only spawned when there are more than 0 maximum candies
            if (MaxCandyTypes.Count > 0)
            {
                var position = GetValidPosition(JarSpawn);
                var obj = Instantiate(CandyJar, position, Quaternion.identity);
                var type = MaxCandyTypes[Random.Range(0, MaxCandyTypes.Count)];
                obj.GetComponent<CandyJarScript>().SetCandy(Candies[type]); //Set which candy type the bomb holds
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
    private Vector3 GetValidPosition(Collider2D spawn)
    {
        //Candy must be generated within Spawn boundary, but it must be generated outside the No spawn boundary at the same time
        //Below code handles the described logic
        var xPos = Random.Range(spawn.bounds.min.x, spawn.bounds.max.x);
        var yPos = Random.Range(spawn.bounds.min.y, spawn.bounds.max.y);
        //Check if its within the nospawn box
        var xBound = NoSpawn.bounds.min.x < xPos && NoSpawn.bounds.max.x > xPos;
        var yBound = NoSpawn.bounds.min.y < yPos && NoSpawn.bounds.max.y > yPos;

        //I don't know how to make this one smarter....
        while (xBound && yBound)
        {
            xPos = Random.Range(spawn.bounds.min.x, spawn.bounds.max.x);
            yPos = Random.Range(spawn.bounds.min.y, spawn.bounds.max.y);
            xBound = NoSpawn.bounds.min.x < xPos && NoSpawn.bounds.max.x > xPos;
            yBound = NoSpawn.bounds.min.y < yPos && NoSpawn.bounds.max.y > yPos;
        }
        var position = new Vector3(xPos, yPos, 0);
        return position;
    }

    //called from GameControl class
    public void DecrementCounter()
    {
        BagCount--;
    }

    //Calculate exp gained while in idle mode
    public int IdleExperienceGained(int idleSeconds)
    {
        var count = NumberOfJars * (idleSeconds / SpawnFreq); //How many jars spawned during that period?
        var avgExp = 0;
        //Go through all the maxed candies, and calculate what the average exp of a jar would be.
        for(int i = 0; i < MaxCandyTypes.Count; i++)
        {
            var candy = Candies[MaxCandyTypes[i]];
            var spawnCount = candy.GetCandyCount();
            var exp = candy.Exp;
            avgExp += spawnCount * exp;
        }
        if (MaxCandyTypes.Count > 0)
        {
            avgExp = avgExp / MaxCandyTypes.Count;
        }
        return avgExp * (int)count;
    }
}
