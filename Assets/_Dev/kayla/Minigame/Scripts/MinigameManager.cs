using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private List<Mole> moles;

    [Header("UI Objects")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject outOfTimeText;
    [SerializeField] private GameObject bombText;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    private float startingTime = 30f;

    private float timeRemaining;
    private HashSet<Mole> currentMoles = new HashSet<Mole>();
    private int score;
    private bool playing = false;

    public void StartGame()
    {
        playButton.SetActive(false);
        outOfTimeText.SetActive(false);
        bombText.SetActive(false);
        gameUI.SetActive(true);

        for (int i = 0; i < moles.Count; i++)
        {
            moles[i].Hide();
            moles[i].SetIndex(i);
        }
        currentMoles.Clear();
        timeRemaining = startingTime;
        score = 0;
        scoreText.text = "0";
        playing = true;
    }

    public void GameOver(int type)
    {
        if(type == 0)
        {
            outOfTimeText.SetActive(true);
        }
        else
        {
            bombText.SetActive(true);
        }

        foreach(Mole mole in moles)
        {
            mole.StopGame();
        }

        playing = false;
        playButton.SetActive(true);
    }

    void Update()
    {
        if (playing)
        {
            timeRemaining -= Time.deltaTime;
            if(timeRemaining <= 0)
            {
                timeRemaining = 0;
                GameOver(0);
            }
            timeText.text = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60:D2}";
            if (currentMoles.Count <= (score / 10))
            {
                int index = Random.Range(0, moles.Count);
                if (!currentMoles.Contains(moles[index]))
                {
                    currentMoles.Add(moles[index]);
                    moles[index].Activate(score / 10);
                }
            }
        }
    }

    public void AddScore(int moleIndex)
    {
        score += 1;
        scoreText.text = $"{score}";
        timeRemaining += 1;
        currentMoles.Remove(moles[moleIndex]);
    }

    public void Missed(int moleIndex, bool isMole)
    {
        if (isMole)
        {
            timeRemaining -= 2;
        }
        currentMoles.Remove(moles[moleIndex]);
    }
}
