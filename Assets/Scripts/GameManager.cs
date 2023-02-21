using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject playerPrefab;
    public TMP_Text scoreText;
    public TMP_Text ballsText;
    public TMP_Text levelText;
    public TMP_Text highScoreText;

    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelLevelCompleted;
    public GameObject panelGameOver;

    public GameObject[] levels;

    public static GameManager s_instance { get; private set; }

    public enum State
    {
        MENU,
        INIT,
        PLAY,
        LEVELCOMPLETED,
        LOADLEVEL,
        GAMEOVER
    }

    State m_state;
    GameObject m_currentBall;
    GameObject m_currentLevel;
    bool m_isSwitchingState;

    private int m_score;
    public int Score
    {
        get { return m_score; }
        set
        {
            m_score = value;
            scoreText.text = "SCORE: " + m_score;
        }
    }
    
    private int m_level;
    public int Level
    {
        get { return m_level; }
        set
        {
            m_level = value;
            levelText.text = "LEVEL: " + m_level;
        }
    }

    private int m_balls;
    public int Balls
    {
        get { return m_balls; }
        set
        {
            m_balls = value;
            ballsText.text = "BALLS: " + m_balls;
        }
    }
    
    

    public void playClicked()
    {
        switchState(State.INIT);
    }

    void Start()
    {
        s_instance = this;
        switchState(State.MENU);
        // @@@@ PlayerPrefs.DeleteKey("highscore");
    }

    void switchState(State newState, float delay = 0)
    {
        StartCoroutine(switchDelay(newState, delay));
    }

    IEnumerator switchDelay(State newState, float delay)
    {
        m_isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        endState();
        m_state = newState;
        beginState(newState);
        m_isSwitchingState = false;
    }

    void beginState(State newState)
    {
        switch (newState)
        {
        case State.MENU:
            Cursor.visible = true;
            highScoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("highscore");
            panelMenu.SetActive(true);
            break;
        case State.INIT:
            Cursor.visible = false;
            panelPlay.SetActive(true);
            Score = 0;
            Level = 0;
            Balls = 3;
            if (m_currentLevel != null)
                Destroy(m_currentLevel);
            Instantiate(playerPrefab);
            switchState(State.LOADLEVEL);
            break;
        case State.PLAY:
            break;
        case State.LEVELCOMPLETED:
            Destroy(m_currentBall);
            Destroy(m_currentLevel);
            Level++;
            panelLevelCompleted.SetActive(true);
            switchState(State.LOADLEVEL, 2.0f); 
            break;
        case State.LOADLEVEL:
            if (Level >= levels.Length)
                switchState(State.GAMEOVER);
            else
            {
                m_currentLevel = Instantiate(levels[Level]);
                switchState(State.PLAY);
            }
            break;
        case State.GAMEOVER:
            if (Score > PlayerPrefs.GetInt("highscore"))
                PlayerPrefs.SetInt("highscore", Score);
            panelGameOver.SetActive(true);
            break;
        }
    }

    void endState()
    {
        switch (m_state)
        {
        case State.MENU:
            panelMenu.SetActive(false);
            break;
        case State.INIT:
            break;
        case State.PLAY:
            break;
        case State.LEVELCOMPLETED:
            panelLevelCompleted.SetActive(false);
            break;
        case State.LOADLEVEL:
            break;
        case State.GAMEOVER:
            panelPlay.SetActive(false);
            panelGameOver.SetActive(false);
            break;
        }
    }

    void Update()
    {
        switch (m_state)
        {
        case State.MENU:
            break;
        case State.INIT:
            break;
        case State.PLAY:
            if (m_currentBall == null)
            {
                if (m_balls > 0)
                {
                    m_currentBall = Instantiate(ballPrefab);
                }
                else
                    switchState(State.GAMEOVER);
            }

            if (m_currentLevel != null && m_currentLevel.transform.childCount == 0 && !m_isSwitchingState)
                switchState(State.LEVELCOMPLETED);
            break;
        case State.LEVELCOMPLETED:
            break;
        case State.LOADLEVEL:
            break;
        case State.GAMEOVER:
            if (Input.anyKeyDown)
                switchState(State.MENU);
            break;
        }        
    }

}
