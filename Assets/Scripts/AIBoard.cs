using System;
using UnityEngine;

public enum PlayerType { NONE, RED, GREEN }

public class AIBoard : MonoBehaviour
{
    public PlayerType[][] board;
    public int rows = 6;
    public int cols = 7;
    public Action<PlayerType> OnGameWon;
    public Action OnGameDraw;

    void Start()
    {
        ResetBoard();
    }

    public void ResetBoard()
    {
        board = new PlayerType[rows][];
        for (int i = 0; i < rows; i++)
        {
            board[i] = new PlayerType[cols];
            for (int j = 0; j < cols; j++)
            {
                board[i][j] = PlayerType.NONE;
            }
        }
    }

    public bool MakeMove(int col, PlayerType player)
    {
        if (col < 0 || col >= cols)
            return false; // Column out of bounds

        for (int row = rows - 1; row >= 0; row--)
        {
            if (board[row][col] == PlayerType.NONE)
            {
                board[row][col] = player;
                CheckGameState(row, col, player);
                return true; // Move was successful
            }
        }
        return false; // Column is full
    }

    private void CheckGameState(int lastRow, int lastCol, PlayerType player)
    {
        if (CheckForWin(lastRow, lastCol, player))
        {
            OnGameWon?.Invoke(player);
        }
        else if (IsBoardFull())
        {
            OnGameDraw?.Invoke();
        }
    }

    public bool CheckForWin(int row, int col, PlayerType player)
    {
        // Horizontal, Vertical, Diagonal checks for a win
        // Simplified for clarity, you need to implement the actual win check logic
        return CheckLineWin(row, col, 1, 0, player) || // Horizontal
               CheckLineWin(row, col, 0, 1, player) || // Vertical
               CheckLineWin(row, col, 1, 1, player) || // Diagonal /
               CheckLineWin(row, col, 1, -1, player);  // Diagonal \
    }

    private bool CheckLineWin(int row, int col, int dRow, int dCol, PlayerType player)
    {
        int count = 1;
        int r = row + dRow;
        int c = col + dCol;

        // Check one direction
        while (r >= 0 && r < rows && c >= 0 && c < cols && board[r][c] == player)
        {
            count++;
            r += dRow;
            c += dCol;
        }

        // Check the opposite direction
        r = row - dRow;
        c = col - dCol;
        while (r >= 0 && r < rows && c >= 0 && c < cols && board[r][c] == player)
        {
            count++;
            r -= dRow;
            c -= dCol;
        }

        return count >= 4;
    }

    private bool IsBoardFull()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (board[i][j] == PlayerType.NONE)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool IsColumnValid(int col)
    {
        // Check if the column index is within bounds
        if (col < 0 || col >= cols)
            return false;

        // Check if the top cell of the column is empty, indicating the column is not full
        return board[0][col] == PlayerType.NONE;
    }

}
