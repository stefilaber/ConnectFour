using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Board = ConnectFour.Board;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject red, yellow;
    bool isPlayer, isGameOver;
    [SerializeField] TMP_Text Message;

    const string redMessage = "Red's Turn";
    const string yellowMessage = "Yellow's Turn";

    Color redColor = new Color(231, 72, 77, 255) / 255;
    Color yellowColor = new Color(212, 149, 69, 255) / 255;

    Board myBoard;

    void Awake()
    {
        isPlayer = true;
        isGameOver = false;
        Message.text = redMessage;
        Message.color = redColor;
        myBoard = new Board();
    }

    // Start is called before the first frame update
    public void GameStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Game over
            if (isGameOver)
            {
                return;
            }

            //Raycast2D
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);


            if(hit.collider.CompareTag("Click"))
            {
                Console.WriteLine("Hit");
                if(hit.collider.gameObject.GetComponent<Column>().targetLocation.y > 2.8f) return;

                Vector3 spawnLocation = hit.collider.gameObject.GetComponent<Column>().spawnLocation;
                Vector3 targetLocation = hit.collider.gameObject.GetComponent<Column>().targetLocation;
                GameObject circle = Instantiate(isPlayer ? red : yellow);
                circle.transform.position = spawnLocation;
                circle.GetComponent<Mover>().targetLocation = targetLocation;

                //Increase the target location height for the next token
                hit.collider.gameObject.GetComponent<Column>().targetLocation.y += 1.222f;

                //Update Board
                myBoard.UpdateBoard(hit.collider.gameObject.GetComponent<Column>().col - 1, isPlayer);
                if(myBoard.Result(isPlayer))
                {
                    Message.text = (isPlayer ? "Red" : "Yellow") + " Wins!";
                    isGameOver = true;
                    return;
                }
                //Change the Message
                Message.text = isPlayer ? yellowMessage : redMessage;
                Message.color = isPlayer ? yellowColor : redColor;

                //Change the player
                isPlayer = !isPlayer;
            }

        }
    }

    public void GameQuit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
