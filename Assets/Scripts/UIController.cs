using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [SerializeField]
    TextMeshProUGUI scoreTextbox;

    [SerializeField]
    Transform livesContainer;



    [SerializeField]
    int initialLives;

    bool hasLives = true;

   

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetLives();
      
    }

   

    public void DecreaseLives()
    {
        int maxLiveNumber = 0;
        Image maxLiveImage = null;

        Image[] liveImages = livesContainer.GetComponentsInChildren<Image>();

        foreach (Image liveImage in liveImages)
        {
            if (liveImage.name.StartsWith("Live-") && liveImage.enabled)
            {
                int liveNumber = int.Parse(liveImage.name.Remove(0, 5));
                if (maxLiveNumber == 0 || liveNumber > maxLiveNumber)
                {
                    maxLiveNumber = liveNumber;
                    maxLiveImage = liveImage;
                }
            }
        }

        if (maxLiveImage != null)
        {
            maxLiveImage.enabled = false;
        }

        hasLives = maxLiveNumber > 0;
    }

    public bool HasLives()
    {
        return hasLives;
    }

    public void IncreaseScore(float points)
    {
        float score = float.Parse(scoreTextbox.text);
        score += points;
        scoreTextbox.text = score.ToString();
    }



    public void ResetLives()
    {
       

        Image[] liveImages = livesContainer.GetComponentsInChildren<Image>();
        int livesToEnable = initialLives;

        foreach (Image liveImage in liveImages)
        {
            if (livesToEnable > 0)
            {
                liveImage.enabled = true;
                livesToEnable--;
            }
            else
            {
                liveImage.enabled = false;
            }
        }

        hasLives = initialLives > 0;
    }
   

}