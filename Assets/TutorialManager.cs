using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private int currentTutorial = 1;
    private bool _active = false;
    [SerializeField] private Button LeftButton;
    [SerializeField] private Button RightButton;
    [SerializeField] private List<GameObject> Tutorials;
    private bool isButtonActive = false;

    public void Activate()
    {
        _active = true;
        currentTutorial = 1;
        ChangeTutorial();
    }

    public void Deactivate()
    {
        _active = false;
        isButtonActive = false;
    }

    private void ChangeTutorial()
    {
        for(int i = 0; i < Tutorials.Count; i++)
        {
            if(i == currentTutorial - 1)
            {
                Tutorials[i].SetActive(true);
            }
            else
            {
                Tutorials[i].SetActive(false);
            }
        }

        if(currentTutorial >= Tutorials.Count)
        {
            RightButton.gameObject.SetActive(false);

            LeftButton.Select();
        }
        else
        {
            RightButton.gameObject.SetActive(true);
        }

        if (currentTutorial <= 1)
        {
            LeftButton.gameObject.SetActive(false);
            if(isButtonActive) RightButton.Select();
        }
        else
        {
            LeftButton.gameObject.SetActive(true);
        }
    }

    public void NextTutorial()
    {
        if (!_active) return;
        isButtonActive = true;

        currentTutorial++;
        ChangeTutorial();      
    }

    public void PreviousTutorial()
    {
        if (!_active) return;

        currentTutorial--;
        ChangeTutorial(); 
    }
}
