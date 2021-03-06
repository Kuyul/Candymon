﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class holds information about individual candies.
//This class will be instantiated on game startup called from LevelControl script
//Some variables are hard set manually, and some are fetched from saved playerprefs
public class CandyInfo : MonoBehaviour
{
    //Declare UI components
    public Text LevelText;
    public Text CostText;

    //Declare public variables
    public GameObject InstantiatePrefab;
    public int Type;
    public long Exp;
    public long UpgradeCost;

    //Declare private variables
    private int Level;

    public void Initialise(Text levelText, Text costText)
    {
        SetLevel();
        LevelText = levelText;
        CostText = costText;
        UpdateTexts();
    }

    //Fetches current candy level from playerprefs
    public void SetLevel()
    {
        if (Type == 0)
        {
            Level = PlayerPrefs.GetInt("Candy" + Type, 1);
        }
        else
        {
            Level = PlayerPrefs.GetInt("Candy" + Type);
        }
    }

    //Called from this.Start and LevelControl to refresh UI texts
    public void UpdateTexts()
    {
        LevelText.text = Level + "/10";
        if (Level < 10)
        {
            CostText.text = "" + GameControl.FormatNumberKM(CalculateCost());
        }
        else
        {
            CostText.text = "MAX";
        }
    }

    public int GetLevel()
    {
        return Level;
    }

    //this method is called from the LevelControl class when upgrade button is clicked.
    //We check whether there is enough gold first.
    public bool Upgrade()
    {
        if (Level < 10)
        {
            //Check if we have enough cash
            var gold = GameControl.Instance.GetGoldAmount();
            if (gold >= CalculateCost())
            {
                GameControl.Instance.AddGold(-CalculateCost());
                LevelUp();
                UpdateTexts();
                return true;
            }
            else
            {
                Debug.Log("Not Enough Money");
                return false;
            }
        }
        else
        {
            Debug.Log("Already Max Level");
            return false;
        }
    }

    //Update Playerprefs
    public void LevelUp()
    {
        Level++;
        PlayerPrefs.SetInt("Candy" + Type, Level);
        if(Level == 1)
        {
            GameControl.Instance.ActivateMonster(Type); //Monster Type == Candy Type
        }
        if(Level == 10)
        {
            GameControl.Instance.AddEvolutionCount(); //When you level up a candy to 10, add an evolution counter that changes the character's looks
        }
    }
    
    //This method was created because we could add a complex logic in the future, but for now Level * initial cost
    public long CalculateCost()
    {
        //Logic - UpgradeCost * 1.7^Level
        
        var n = (long)(UpgradeCost * Mathf.Pow(1.4f, Level)); //ex 8796
        long digits = (long)Mathf.Ceil(Mathf.Log10(n)); //ex 4
        if (digits >= 4)
        {
            var b = digits - 3; //ex 1
            long n3 = (long)Mathf.Round(n / (int)Mathf.Pow(10, b)); //Ex 8796/(10*1) = 879.6 rounded = 880
            n = n3 * (int)Mathf.Pow(10, b);
        }
        return n;
    }

    //Number of candy spawned from bag/jar = level + 4 (when level > 0)
    public int GetCandyCount()
    {
        return Level + 1;
    }

    public GameObject InstantiateAtPos(Vector3 pos)
    {
        pos.z = 0;//Z axis should be 0
        var rotation = Quaternion.Euler(0,0,Random.Range(0,360)); //Add Random rotation
        var obj = Instantiate(InstantiatePrefab, pos, rotation);
        obj.AddComponent<Candy>().Exp = Exp;
        obj.GetComponent<Rigidbody2D>().velocity = Vector3.down * GameControl.Instance.FallSpeed;
        GameControl.Instance.AddCandy(obj);
        return obj;
    }
}
