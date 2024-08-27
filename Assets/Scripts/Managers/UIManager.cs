using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] RectTransform PanelCharacters;
    [SerializeField] RectTransform PanelColors;
    [SerializeField] Vector3 VictimPointPanelCharacters;
    [SerializeField] Vector3 VictimPointPanelColors;
    [SerializeField] GameObject[] Personajes;
    private Vector3 PanelVelocityCharacters = new Vector3(0,1,0);
    private Vector3 PanelVelocityColors = new Vector3(1,0,0);

    public void PanelCharactersAnimation()
    {
        PanelCharacters.GetComponent<RectTransform>().localPosition = Vector3.SmoothDamp(PanelCharacters.localPosition, VictimPointPanelCharacters, 
                                                           ref PanelVelocityCharacters, 0.5f);
        if (PanelCharacters.GetComponent<RectTransform>().localPosition.y <= 335.00f)
        {
            for (int i = 0; i < Personajes.Length; i++)
            {
                Personajes[i].gameObject.SetActive(true);
                Personajes[i].gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            //return;
        }
    }
    public void PanelColorsAnimation()
    {
        PanelColors.GetComponent<RectTransform>().localPosition = Vector3.SmoothDamp(PanelColors.localPosition, VictimPointPanelColors,
                                                           ref PanelVelocityColors, 0.5f);
    }
        
    // Update is called once per frame
    void Update()
    {
        PanelCharactersAnimation();
        PanelColorsAnimation();
    }
}
