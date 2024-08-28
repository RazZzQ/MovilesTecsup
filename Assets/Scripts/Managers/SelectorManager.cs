using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SelectorManager : MonoBehaviour
{
    public GameObject[] ButtonTest;
    public Color color;
    public GameObject character;
    public bool MouseFree = true;
    public void SelectorCharacter(string Character)
    {
        try
        {

        if(Character == "pj1")
        {
            character = ButtonTest[0].transform.GetChild(0).gameObject;
        }
        else if(Character == "pj2")
        {
            character = ButtonTest[1].transform.GetChild(0).gameObject;
        }
        else if(Character == "pj3")
        {
            character = ButtonTest[2].transform.GetChild(0).gameObject;
        }
        else if(Character == "pj4")
        {
            character = ButtonTest[3].transform.GetChild(0).gameObject;
        }
        else if(Character == "pj5")
        {
            character = ButtonTest[4].transform.GetChild(0).gameObject;
        }
        else if(Character == "pj6")
        {
            character = ButtonTest[5].transform.GetChild(0).gameObject;
        }
        }
        catch
        {
            Debug.LogError("");    
        }
        MouseFree = false;
    }
    public void SelectorColor(string victim)
    {
        if(victim == "Red")
        {
            color = Color.red;
        }
        else if(victim == "Blue")
        {
            color = Color.blue;
        }else if(victim == "Green")
        {
            color = Color.green;
        }else if(victim == "Yellow")
        {
            color = Color.yellow;
        }else if(victim == "Orange")
        {
            color = new Color(1.0f, 0.64f, 0.0f);
        }
    }
    public void CleanCharacterandColor()
    {
        MouseFree = true;
        character = null;
    }
}
