﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelControl : MonoBehaviour {

    //Declare public variables
    public GameObject[] Candies;
    public Text[] CandyLevelTexts; //UI that represents candy levels

    //Declare private variables
    private List<int> CandyLevels = new List<int>();
    private Camera cam;
    private float MaxX;
    private float MaxY;
    private float counter;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
        //Get the boundaries of camera
        Vector3 Boundary = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        MaxX = Boundary.x;
        MaxY = Boundary.y;
        counter = 0.2f;

        //Get current candy levels and assign them to a private variable
        for (int i = 0; i < Candies.Length; i++)
        {
            CandyLevels.Add(GetCandyLevel(i));
            CandyLevelTexts[i].text = CandyLevels[i] + "/10";
        }
    }
	
	// Update is called once per frame
	void Update () {
        var xPos = Random.Range(-MaxX, MaxX);
        var yPos = Random.Range(-MaxY, MaxY);
        var position = new Vector3(xPos, yPos, 0);
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            var a = Instantiate(Candies[0], position, Quaternion.identity);
            GameControl.Instance.AddCandy(a);
            counter = 0.2f;
        }
    }

    //This method is used from the UI buttons and its invoked when clicked.
    //takes candy index as an input parameter, and levels up the candy.
    public void LevelUpCandy(int i)
    {
        var r = CandyLevels[i] + 1;
        PlayerPrefs.SetInt("Candy" + i, r);
        RefreshCandyInfo();
    }

    private void RefreshCandyInfo()
    {
        for (int i = 0; i < Candies.Length; i++)
        {
            CandyLevels[i] = GetCandyLevel(i);
            CandyLevelTexts[i].text = CandyLevels[i] + "/10";
        }
    }

    //Returns candy level for a given index
    private int GetCandyLevel(int i)
    {
        int r;
        if(i == 0)
        {
            r = PlayerPrefs.GetInt("Candy" + i, 1);
        }
        else
        {
            r = PlayerPrefs.GetInt("Candy" + i);
        }

        return r;
    }

    public void DeleteAllPlayerprefs()
    {
        PlayerPrefs.DeleteAll();
        RefreshCandyInfo();
    }
}
