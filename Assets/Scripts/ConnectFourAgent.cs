using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ConnectFourAgent : Agent
{
    public AIBoard board;
    public PlayerType agentPlayerType;
    public PlayerType opponentPlayerType;

    public override void Initialize()
    {
        // Optionally initialize your environment here
        board.OnGameWon += HandleGameWon;
        board.OnGameDraw += HandleGameDraw;
    }

    public override void OnEpisodeBegin()
    {
        // Reset the board and potentially other state at the beginning of each episode
        board.ResetBoard();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Provide the agent with observations from the board
        foreach (var row in board.board)
        {
            foreach (var cell in row)
            {
                sensor.AddObservation((int)cell);
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Interpret the agent's action
        int columnChoice = actions.DiscreteActions[0];

        if (!board.MakeMove(columnChoice, agentPlayerType))
        {
            // If the move is invalid (e.g., column is full), we give a negative reward
            AddReward(-1f);
            EndEpisode(); // Optionally end the episode on invalid move
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Provide a heuristic for testing without a trained model
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Random.Range(0, 7); // Example: choose a random column
    }

    void HandleGameWon(PlayerType winner)
    {
        if (winner == agentPlayerType)
        {
            // Agent wins
            AddReward(1.0f);
        }
        else
        {
            // Opponent wins
            AddReward(-1.0f);
        }
        EndEpisode();
    }

    void HandleGameDraw()
    {
        // Handle a draw with a smaller positive reward or no reward
        AddReward(0.5f); // Example: small positive reward for a draw
        EndEpisode();
    }
}
