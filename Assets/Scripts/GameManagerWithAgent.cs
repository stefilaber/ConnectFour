using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;

public class GameManagerWithAgent : MonoBehaviour
{
    [SerializeField] GameObject redToken, yellowToken;
    [SerializeField] TMP_Text messageText;

    const string redMessage = "Red's Turn";
    const string yellowMessage = "Agent's Turn";

    Color redColor = new Color(231, 72, 77, 255) / 255;
    Color yellowColor = new Color(212, 149, 69, 255) / 255;

    bool isPlayerTurn = true;
    bool isGameOver = false;
    Board board;

    // Reference to the agent script
    public ConnectFourAgent agent;

    void Awake()
    {
        messageText.text = redMessage;
        messageText.color = redColor;
        board = new Board();
    }

    void Update()
    {
        if (isGameOver)
        {
            return;
        }

        if (isPlayerTurn)
        {
            PlayerTurn();
        }
        else
        {
            // Let the agent take its turn
            AgentTurn();
        }
    }

    void PlayerTurn()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast to detect clicks on the game board
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Click"))
            {
                // Extract column selection from the hit object
                int column = hit.collider.gameObject.GetComponent<Column>().col;
                MakeMove(column, true); // true indicating it's the player's turn
            }
        }
    }

    void AgentTurn()
    {
        // Request a decision from the agent
        // Note: You'll need to implement the logic for the agent to decide on its move based on the game state
        agent.RequestDecision();
    }

    // This method is called by the agent script when it decides on a column
    public void AgentMakesMove(int column)
    {
        MakeMove(column, false); // false indicating it's the agent's turn
    }

    void MakeMove(int column, bool isPlayer)
    {
        // Perform the move logic similar to your original Update method
        // Make sure to update this method to handle both player and agent moves

        // Example implementation:
        if (board.UpdateBoard(column, isPlayer))
        {
            // Update the board visually
            Vector3 spawnLocation = // Determine based on column;
            Vector3 targetLocation = // Determine based on column;
            GameObject token = Instantiate(isPlayer ? redToken : yellowToken, spawnLocation, Quaternion.identity);
            token.GetComponent<Mover>().targetLocation = targetLocation;

            // Check for a win or a draw
            if (board.Result(isPlayer))
            {
                messageText.text = (isPlayer ? "Player" : "Agent") + " Wins!";
                messageText.color = isPlayer ? redColor : yellowColor;
                isGameOver = true;
            }
            else
            {
                // Change turns
                isPlayerTurn = !isPlayerTurn;
                messageText.text = isPlayerTurn ? redMessage : yellowMessage;
                messageText.color = isPlayerTurn ? redColor : yellowColor;
            }
        }
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameQuit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
