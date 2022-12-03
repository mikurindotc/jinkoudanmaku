using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class DdaAgent : Agent
{
    [SerializeField]
    private Game_Manager gameManager;
    [SerializeField]
    private Player playerHandler;
    private int difficultyModifier = 0;
    public int maxMatches = 10;
    public int currentMatchesNumber = 0;

    public override void Initialize()
    {
        gameManager.Difficulty(130);
        maxMatches = 10;
        currentMatchesNumber = 0;
        difficultyModifier = 0;
    }

    public override void OnEpisodeBegin()
    {
        gameManager.Difficulty(130);
        maxMatches = 10;
        currentMatchesNumber = 0;
        difficultyModifier = 0;
    }

    private void Update()
    {
        if (currentMatchesNumber > maxMatches)
        {
            switch ((int)gameManager.CurrentGameState())
            {
                case 0:
                    break;
                case 1:
                    AddReward(-1f);
                    break;
                case 2:
                    AddReward(1f);
                    break;
                default:
                    break;
            }
            EndEpisode();
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        difficultyModifier = (int)vectorAction[0];
        Debug.Log(difficultyModifier);
        gameManager.ChangeDifficulty(difficultyModifier);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(gameManager.Difficulty());
        sensor.AddObservation(playerHandler.lives);
    }

    public override void Heuristic(float[] actionsOut)
    {
        int difficultyModifier = (int)gameManager.changeDifficulty;
        actionsOut[0] = difficultyModifier;
    }
}
