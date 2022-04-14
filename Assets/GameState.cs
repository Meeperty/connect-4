using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Connect4
{
    public class GameState : MonoBehaviour
    {
        public GameObject[,] squares = new GameObject[7,6];
        public SquareState[,] squareStates = new SquareState[7,6];
        public int currentPlayer = 1; // who's turn is it? 1/red, 2/yellow

        public Sprite empty;
        public Sprite yellow;
        public Sprite red;

        // Start is called before the first frame update
        void Start()
        {
            BoardSetup setup = GetComponent<BoardSetup>();
            squares = setup.squares;
            squareStates = setup.squareStates;
        }

        private void OnGUI()
        {
            Event e = Event.current;

            if (e.button == 0 && e.isMouse && e.type == EventType.MouseUp)
            {
                Vector2 mousePos = e.mousePosition;
                PlaceToken(mousePos, currentPlayer);
                Debug.Log("Mouse Click");
            }
        }

        public void PlaceToken(Vector2 mousePosition, int currentPlayer)
        {
            Vector2Int mousePosInt = new Vector2Int();
            mousePosInt.x = (int)Mathf.Floor(mousePosition.x);
            int column = (int)Mathf.Floor(Camera.main.ScreenToWorldPoint(mousePosition).x);
            for (int r = 5; r > -1; r--)
            {
                if (squareStates[r, column] == SquareState.Empty)
                {
                    SpriteRenderer squareRenderer = squares[r, column].GetComponent<SpriteRenderer>();
                    if (currentPlayer == 1) 
                    { 
                        squareStates[r, column] = SquareState.Yellow;
                        squareRenderer.sprite = yellow;
                    }
                    else 
                    { 
                        squareStates[r, column] = SquareState.Red;
                        squareRenderer.sprite = red;
                    }
                    if (PositionEval.HasPlayerWon(new Vector2Int(column, r), currentPlayer))
                    {
                        Debug.Log("player " + currentPlayer + " has won");
                        ResetBoard();
                    }
                    else
                    {
                        NextTurn();
                    }
                    break;
                }
            }
        }

        public void NextTurn()
        {
            if (currentPlayer == 1) { currentPlayer++; }
            else { currentPlayer = 1; }
        }

        public void ShowHideSquares(bool activated)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    squares[i, j].SetActive(activated);
                }
            }
        }

        public void ResetBoard()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    squareStates[i, j] = SquareState.Empty;
                    squares[i, j].GetComponent<SpriteRenderer>().sprite = empty;
                    currentPlayer = 1;
                }
            }
        }
    }

    public enum SquareState
    {
        Empty,
        Red,
        Yellow
    }
}