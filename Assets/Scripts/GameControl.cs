using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    public static GameControl Instance;

    //Declare UI Components
    public Text GoldText;
    public Text LevelText;
    public Text ExpText;
    public Slider ExpBar;

    //Declare public variables
    public MonsterScript Mon;
    public LevelControl Level;
    public float FallSpeed = 3.0f;

    public GameObject bagPop;
    public GameObject eat;

    //Declare private variables
    private List<GameObject> Candies;
    private int Gold;
    private int Multiplier = 1;
    private int TimeDiff;
    private int ExpGained = 0;

    //Monster Related variables
    //Level - Exp list
    public int[] LevelExp;
    //private variables
    private int MonsterLevel;
    private int Experience;

    // Use this for initialization
    void Start () {

		if(Instance == null)
        {
            Instance = this;
        }
        AddGold(999999);
        Candies = new List<GameObject>();
        Gold = GetGoldAmount();
        FormatNumberB(Gold);

        //Set Monster level properties
        MonsterLevel = GetMonsterLevel();
        LevelText.text = "Lv " + MonsterLevel;

        //Set Experience Properties
        Experience = GetMonsterExperience();
        ExpText.text = Experience + "/" + LevelExp[MonsterLevel - 1];

        //Get time difference
        DateTime timeNow = System.DateTime.Now;
        String timeEnded = PlayerPrefs.GetString("SystemTime", "");
        if (timeEnded != "") //Idle progression not applicable on running the game first time
        {
            var diff = (timeNow - Convert.ToDateTime(timeEnded)).TotalSeconds;
            TimeDiff = Convert.ToInt32(diff);
            var expGained = Level.IdleExperienceGained(TimeDiff);
            Debug.Log(TimeDiff);
            Debug.Log(expGained);
            ExpGained = expGained; //Monster script will access it later
            AddGold(expGained * Multiplier);
        }
        //Run system time logging
        StartCoroutine(LogSystemTime());
    }

    void Update()
    {
        ExpBar.value = (float)Experience / (float)LevelExp[MonsterLevel - 1];
    }

    IEnumerator LogSystemTime()
    {
        while (true)
        {
            DateTime time = System.DateTime.Now;
            PlayerPrefs.SetString("SystemTime", System.DateTime.Now.ToString());
            yield return new WaitForSeconds(10.0f);
        }
        
    }

    //Triggered from Levelcontrol script to add a new candy to the list
    public void AddCandy(GameObject candy)
    {
        Candies.Add(candy);
    }

    //Called from mouth script to destroy candy when the monster eats it!
    public void EatCandy(GameObject candy) {
        Destroy(candy);
        Candies.Remove(candy);
        var exp = candy.GetComponent<Candy>().Exp;
        AddExp(exp);
        AddGold(exp * Multiplier);
    }

    //Monster Related Methods
    public int GetMonsterLevel()
    {
        return PlayerPrefs.GetInt("MonsterLevel", 1);
    }

    public int GetMonsterExperience()
    {
        return PlayerPrefs.GetInt("Experience", 0);
    }

    public void AddExp(int value)
    {
        Experience += value;
        if (Experience >= LevelExp[MonsterLevel - 1])
        {
            LevelUp();
        }
        PlayerPrefs.SetInt("Experience", Experience);
        ExpText.text = Experience + "/" + LevelExp[MonsterLevel - 1];
    }

    //On leveling up, update level text and reset exp to 0 and update playerprefs accordingly.
    public void LevelUp()
    {
        Experience -= LevelExp[MonsterLevel - 1];
        MonsterLevel++;
        PlayerPrefs.SetInt("MonsterLevel", MonsterLevel);
        LevelText.text = "Lv " + MonsterLevel;
        PlayerPrefs.SetInt("Experience", Experience);
        ExpText.text = Experience + "/" + LevelExp[MonsterLevel - 1];
    }

    public void AddEvolutionCount()
    {
        int evo = GetEvolutionLevel();
        PlayerPrefs.SetInt("Evolution", evo + 1);
    }

    public int GetEvolutionLevel()
    {
        return PlayerPrefs.GetInt("Evolution", 0);
    }

    public int GetExpGained()
    {
        return ExpGained;
    }

    public int GetGoldAmount()
    {
        return PlayerPrefs.GetInt("GoldAmount");
    }

    //Input negative number to subtract the amount
    public void AddGold(int amount)
    {
        Gold += amount;
        GoldText.text = FormatNumberB(Gold);
        PlayerPrefs.SetInt("GoldAmount", Gold);
    }

    public void DecrementBagCount()
    {
        Level.DecrementCounter();
    }

    public int GetCandyCount()
    {
        return Candies.Count;
    }

    //Formats the number from thousand onwards - used for candy cost formatting
    public static string FormatNumberKM(int number)
    {
        string r;
        float val;
        if (number > 1000000000)
        {
            val = number / 1000000000f;
            r = val.ToString("#.##B");
        }
        else  if (number > 1000000)
        {
            val = number / 1000000f;
            r = val.ToString("#.##M");
        }
        else if (number > 1000)
        {
            val = number / 1000f;
            r = (number / 1000).ToString("#.##K");
        }
        else
            r = number.ToString();

        return r;
    }

    //Formats the number from billion onwards - used for Money formatting
    public static string FormatNumberB(int number)
    {
        string r;
        float val;
        if (number > 1000000000)
        {
            val = number / 1000000000f;
            r = val.ToString("#.##B");
        }
        else
            r = number.ToString("### ### ###");

        return r;
    }
}
