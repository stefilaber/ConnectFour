using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            Console.WriteLine("clicked");
            // Game over
            if (isGameOver)
            {
                return;
            }

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

                //Change the Message
                Message.text = isPlayer ? yellowMessage : redMessage;
                Message.color = isPlayer ? yellowColor : redColor;

                //Change the player
                isPlayer = !isPlayer;


                // if(hit.collider.gameObject.GetComponent<Column>().col < 7)
                // {
                //     if(isPlayer)
                //     {
                //         GameObject newRed = Instantiate(red, hit.collider.gameObject.GetComponent<Column>().spawnLocation, Quaternion.identity);
                //         newRed.GetComponent<Red>().targetSpawnLocation = hit.collider.gameObject.GetComponent<Column>().targetSpawnLocation;
                //         hit.collider.gameObject.GetComponent<Column>().col++;
                //         isPlayer = false;
                //         turnMessage.text = yellow;
                //         turnMessage.color = yellowColor;
                //     }
                //     else
                //     {
                //         GameObject newYellow = Instantiate(yellow, hit.collider.gameObject.GetComponent<Column>().spawnLocation, Quaternion.identity);
                //         newYellow.GetComponent<Yellow>().targetSpawnLocation = hit.collider.gameObject.GetComponent<Column>().targetSpawnLocation;
                //         hit.collider.gameObject.GetComponent<Column>().col++;
                //         isPlayer = true;
                //         turnMessage.text = red;
                //         turnMessage.color = redColor;
                //     }
                // }
            }

        }
    }

    // public void GameStart()
    // {
    //     UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    // }

    public void GameQuit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
