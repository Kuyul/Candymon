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
    public Text IdleExp;
    public Slider ExpBar;
    public GameObject PanelIdle;
    public RectTransform Content;
    public AudioSource CandyHitSound;

    //Level up properties
    public GameObject LevelUpPanel;
    public Text RewardText;
    private long RewardAmount;

    //Declare public variables
    public MonsterScript Mon;
    public LevelControl Level;
    public float FallSpeed = 3.0f;

    public GameObject bagPop;

    //Declare private variables
    private List<GameObject> Candies;
    private long Gold;
    private int Multiplier = 1;
    private int TimeDiff;
    private long ExpGained = 0;
    private long AccumulatedExp = 0;
    private long AccumulatedGold = 0;

    //Monster Related variables
    //Level - Exp list
    public long[] LevelExp;
    //private variables
    private int MonsterLevel;
    private long Experience;

    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 300;

		if(Instance == null)
        {
            Instance = this;
        }

        Time.timeScale = 0;

        Candies = new List<GameObject>();
        Gold = GetGoldAmount();
        AddGold(9999999);
        FormatNumberB(Gold);

        //Set Monster level properties
        MonsterLevel = GetMonsterLevel();
        LevelText.text = "Lv " + MonsterLevel;

        //Set Experience Properties
        Experience = GetMonsterExperience();
        SetExpText();
        GetContentPos();

        //Get time difference
        DateTime timeNow = System.DateTime.Now;
        String timeEnded = PlayerPrefs.GetString("SystemTime", "");
        if (timeEnded != "") //Idle progression not applicable on running the game first time
        {
            var diff = (timeNow - Convert.ToDateTime(timeEnded)).TotalSeconds;
            TimeDiff = Convert.ToInt32(diff);
            var expGained = Level.IdleExperienceGained(TimeDiff);
            ExpGained = expGained; //Monster script will access it later
            IdleExp.text = "" + FormatNumberB(expGained);
        }
        else
        {
            ButtonIdle();
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
            SetContentPos(Content.anchoredPosition.x);
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
        CandyHitSound.Play();
        Destroy(candy);
        Candies.Remove(candy);
        var exp = candy.GetComponent<Candy>().Exp;
        AddExp(exp);
        AddGold(exp * Multiplier);
    }

    public void FinishAccumulating()
    {
        AddExp(AccumulatedExp);
        AddGold(AccumulatedGold);
        AccumulatedExp = 0;
        AccumulatedGold = 0;
    }

    //Called to auto adjust slider bar
    private void SetContentPos(float value)
    {
        PlayerPrefs.SetFloat("ContentPos", value);
    }

    private void GetContentPos()
    {
        var pos = PlayerPrefs.GetFloat("ContentPos", 0);
        if (pos != 0)
        Content.anchoredPosition = new Vector2(PlayerPrefs.GetFloat("ContentPos"), Content.anchoredPosition.y);
    }

    //Monster Related Methods
    public int GetMonsterLevel()
    {
        return PlayerPrefs.GetInt("MonsterLevel", 1);
    }

    public long GetMonsterExperience()
    {
        return (long)PlayerPrefs.GetFloat("Experience", 0);
    }

    private void SetExpText()
    {
        ExpText.text = FormatNumberKM(Experience) + "/" + FormatNumberKM(LevelExp[MonsterLevel - 1]);
    }

    public void AddExp(long value)
    {
        Experience += value;
        if (Experience >= LevelExp[MonsterLevel - 1])
        {
            LevelUp();
        }
        PlayerPrefs.SetFloat("Experience", Experience);
        SetExpText();
    }

    //On leveling up, update level text and reset exp to 0 and update playerprefs accordingly.
    public void LevelUp()
    {
        Experience -= LevelExp[MonsterLevel - 1];
        MonsterLevel++;
        PlayerPrefs.SetInt("MonsterLevel", MonsterLevel);
        LevelText.text = "Lv " + MonsterLevel;
        PlayerPrefs.SetFloat("Experience", Experience);
        SetExpText();

        LevelUpPanel.SetActive(true);
        RewardAmount = LevelExp[MonsterLevel - 1]/4;
        RewardText.text = "" + RewardAmount;
        Time.timeScale = 0;
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

    public long GetExpGained()
    {
        return ExpGained;
    }

    public long GetGoldAmount()
    {
        return (long)PlayerPrefs.GetFloat("GoldAmount");
    }

    //Input negative number to subtract the amount
    public void AddGold(long amount)
    {
        Gold += amount;
        GoldText.text = FormatNumberB(Gold);
        PlayerPrefs.SetFloat("GoldAmount", Gold);
    }

    public void DecrementBagCount()
    {
        Level.DecrementCounter();
    }

    public void StopCandies()
    {
        for (int i = 0; i < Candies.Count; i++)
        {
            Candies[i].GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        }
    }

    public int GetCandyCount()
    {
        return Candies.Count;
    }


    //Required from MonsterScript class to determine which monsters should be set active
    public CandyInfo[] GetCandyInfos()
    {
        return Level.GetCandyInfos();
    }


    //Called from the candyinfo class to activate monster when candy level goes from 0 -> 1
    public void ActivateMonster(int i)
    {
        Mon.ActivateMonster(i);
    }

    //Formats the number from thousand onwards - used for candy cost formatting
    public static string FormatNumberKM(long number)
    {
        string r;
        float val;
        if (number > 1000000000000000)
        {
            val = number / 1000000000000000;
            r = val.ToString("#.##Q");
        }
        else if (number > 1000000000000)
        {
            val = number / 1000000000000;
            r = val.ToString("#.##T");
        }
        else if (number > 1000000000)
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
            r = val.ToString("#.##K");
        }
        else
            r = number.ToString();

        return r;
    }

    //Formats the number from billion onwards - used for Money formatting
    public static string FormatNumberB(long number)
    {
        string r;
        float val;
        if (number > 1000000000000000)
        {
            val = number / 1000000000000000;
            r = val.ToString("#.##Q");
        }
        else if (number > 1000000000000)
        {
            val = number / 1000000000000;
            r = val.ToString("#.##T");
        }
        else if (number > 1000000000)
        {
            val = number / 1000000000f;
            r = val.ToString("#.##B");
        }
        else
            r = number.ToString("### ### ###");

        return r;
    }

    //called when pressed button coming back from idle
    public void ButtonIdle()
    {
        Time.timeScale = 1;
        PanelIdle.SetActive(false);
        AddGold(ExpGained * Multiplier);
    }

    //called when button clicked for collecting reward after lvlup
    public void LevelUpReward()
    {
        Time.timeScale = 1;
        LevelUpPanel.SetActive(false);
        AddGold(RewardAmount);
    }
}
