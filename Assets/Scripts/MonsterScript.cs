﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonsterScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //Level - Exp list
    public int[] LevelExp;

    //Declare UI Components
    public Text LevelText;
    public Text ExpText;
    public Slider ExpBar;

    //Declare public variables
    public GameObject MouthOpen;
    public GameObject MouthClosed;
    public GameObject BounceOff;
    public float Speed = 100;

    //Declare private variables
    private PointEffector2D PE;
    private int Level;
    private int Experience;

	// Use this for initialization
	void Start () {
        MouthOpen.SetActive(false);
        MouthClosed.SetActive(true);

        PE = GetComponent<PointEffector2D>();
        //Set Level properties
        Level = GetMonsterLevel();
        LevelText.text = "Lv " + Level;

        //Set Experience Properties
        Experience = GetMonsterExperience();
        ExpText.text = Experience + "/" + LevelExp[Level - 1];
    }

    void Update()
    {
        if(Experience >= LevelExp[Level-1])
        {
            LevelUp();
        }
        ExpBar.value = (float)Experience / (float)LevelExp[Level - 1];
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
    }

    //When mouth is open: 
    //1. point effector is enabled
    //2. Mouth Open Sprite is active
    //3. 
    //Vice versa
    private void SetMouthOpen(bool b)
    {
        PE.enabled = b;
        MouthOpen.SetActive(b);
        MouthClosed.SetActive(!b);
        BounceOff.SetActive(!b);
    }

    public int GetMonsterLevel()
    {
        return PlayerPrefs.GetInt("MonsterLevel", 1);
    }

    public int GetMonsterExperience()
    {
        return PlayerPrefs.GetInt("Experience", 0);
    }

    //Called from Gamecontrol class - updates Exp components (playerprefs/texts)
    public void AddExp(int value)
    {
        Experience += value;
        PlayerPrefs.SetInt("Experience", Experience);
        ExpText.text = Experience + "/" + LevelExp[Level - 1];
    }

    //On leveling up, update level text and reset exp to 0 and update playerprefs accordingly.
    public void LevelUp()
    {
        Level++;
        PlayerPrefs.SetInt("MonsterLevel", Level);
        LevelText.text = "Lv " + Level;
        Experience = 0;
        PlayerPrefs.SetInt("Experience", Experience);
        ExpText.text = Experience + "/" + LevelExp[Level - 1];
    }
}
