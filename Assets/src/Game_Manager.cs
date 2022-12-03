using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Ongoing,
    Lost,
    Won
}

public enum ChangeDifficulty
{
    NoAction,
    DifDown,
    DifUp
}

public class Game_Manager : MonoBehaviour
{
    public Player playerHandler;
    public int playerFinalScore;
    public ChangeDifficulty changeDifficulty;

    [SerializeField] private Suwako suwakoHandler;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject gameOverScreenGO;
    [SerializeField] private DdaAgent ddaAgent;
    private GameState currentGameState;
    [SerializeField] private int difficultyRange;
    private int difficultyRangeMin;

    void CheckIfPlayerKilledSuwako()
    {
        if (suwakoHandler)
        {
            if (suwakoHandler.isSuwakoDead)
            {
                ddaAgent.currentMatchesNumber++;
                currentGameState = GameState.Won;
                if (ddaAgent.currentMatchesNumber > ddaAgent.maxMatches)
                {
                    ResetGame(true);
                }
                else
                {
                    ResetGame(false);
                }
            }
        }
    }

    void CheckIfPlayerIsDead()
    {
        if (playerHandler.lives <= 0)
        {
            ddaAgent.currentMatchesNumber++;
            currentGameState = GameState.Lost;
            if (ddaAgent.currentMatchesNumber > ddaAgent.maxMatches)
            {
                ResetGame(true);
            }
            else
            {
                ResetGame(false);
            }
        }
    }

    void ResetGame(bool shouldResetPlayerSkillLevel)
    {
        if (currentGameState == GameState.Lost || currentGameState == GameState.Won) 
        {
            suwakoHandler.ResetSuwako();
            playerHandler.ResetPlayer(shouldResetPlayerSkillLevel);

            currentGameState = GameState.Ongoing;
        }
    }

    void Start()
    {
        currentGameState = GameState.Ongoing;
        difficultyRangeMin = 0;
        difficultyRange = 130;
        playerHandler = playerPrefab.GetComponent<Player>();
    }

    void Update()
    {
        CheckIfPlayerIsDead();
        CheckIfPlayerKilledSuwako();
    }

    public GameState CurrentGameState()
    {
        return currentGameState;
    }

    public int Difficulty()
    {
        return difficultyRange;
    }
    public void Difficulty(int _difficulty)
    {
        difficultyRange = _difficulty;
    }

    public void ChangeDifficulty(int changeDifficulty)
    {
        switch (changeDifficulty)
        {
            case 0:
                break;
            case 1:
                difficultyRange -= 15;
                if (difficultyRange < difficultyRangeMin)
                {
                    difficultyRange = difficultyRangeMin;
                }
                break;
            case 2:
                difficultyRange += 15;
                if (difficultyRange < difficultyRangeMin)
                {
                    difficultyRange = difficultyRangeMin;
                }
                break;
            default:
                break;
        }
    }
}