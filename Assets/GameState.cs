using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Connect4
{
    public class GameState : MonoBehaviour
    {
        public GameObject[][] squares = InitJaggedArray(new GameObject[7][], 6);
        public SquareState[][] squareStates = InitJaggedArray(new SquareState[7][], 6);
        public int currentPlayer = 1; // who's turn is it? 1/red, 2/yellow
        public bool playing = true;

        public Sprite empty;
        public Sprite yellow;
        public Sprite red;

        public static Sprite spriteEmpty;
        public static Sprite spriteYellow;
        public static Sprite spriteRed;

        public GameObject winTextGameObject;
        public GameObject winUI;

        public static GameObject[][] InitJaggedArray(GameObject[][] array, int secondLength)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new GameObject[secondLength];
            }
            return array;
        }

        public static SquareState[][] InitJaggedArray(SquareState[][] array, int secondLength)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new SquareState[secondLength];
            }
            return array;
        }

        // Start is called before the first frame update
        void Start()
        {
            BoardSetup setup = GetComponent<BoardSetup>();
            squares = setup.squares;
            squareStates = setup.squareStates;

            spriteEmpty = empty;
            spriteYellow = yellow;
            spriteRed = red;

            Debug.Log(Utils.TimeFunction(() => PositionEval.HasPlayerWon(squareStates)));
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


        /// <summary>
        /// Places a token belonging to currentPlayer using mousePosition
        /// </summary>
        /// <param name="mousePosition">The position of the mouse</param>
        /// <param name="currentPlayer">Who's token is being placed (int)</param>
        public void PlaceToken(Vector2 mousePosition, int currentPlayer)
        {
            if (playing)
            {
                int column = (int)Mathf.Floor(Camera.main.ScreenToWorldPoint(mousePosition).x);
                PlaceToken(column, currentPlayer);
            }
        }

        /// <summary>
        /// Places a token belonging to currentPlayer using a column
        /// </summary>
        /// <param name="column">Which column to place in (left to right)</param>
        /// <param name="currentPlayer">Who's token is being placed (int)</param>
        public void PlaceToken(int column, int currentPlayer)
        {
            for (int r = 5; r > -1; r--)
            {
                if (squareStates[column][r] == SquareState.Empty)
                {
                    SpriteRenderer squareRenderer = squares[column][r].GetComponent<SpriteRenderer>();

                    squareStates[column][r] = Utils.PlayerToSquareState(currentPlayer);
                    squareRenderer.sprite = Utils.PlayerToSprite(currentPlayer);

                    if (PositionEval.HasPlayerWon(new Vector2Int(column, r), currentPlayer))
                    {
                        GameWin(currentPlayer);
                        playing = false;
                    }
                    NextTurn();
                    break;
                }
            }
        }

        public SquareState[][] PlaceToken(SquareState[][] boardStates, int column, int player)
        {
            for (int i = 0; i < 6; i++)
            {
                if (boardStates[column][i] == SquareState.Empty)
                {
                    boardStates[column][i] = Utils.PlayerToSquareState(player);
                    return boardStates;
                }
            }
            return null;
        }

        public void GameWin(int winner)
        {
            Debug.Log("player " + winner + " has won");
            if (winner == 1)
            {
                winTextGameObject.GetComponent<Text>().text = "Yellow player has won";
            }
            else
            {
                winTextGameObject.GetComponent<Text>().text = "Red player has won";
            }
            winUI.SetActive(true);
        }

        public void NextTurn()
        {
            if (currentPlayer == 1) { currentPlayer++; }
            else { currentPlayer = 1; }
            MatchGraphicsToStates();
        }

        public void ShowHideSquares(bool activated)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    squares[j][i].SetActive(activated);
                }
            }
        }

        public void ResetBoard()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    squareStates[j][i] = SquareState.Empty;
                    squares[j][i].GetComponent<SpriteRenderer>().sprite = empty;
                    currentPlayer = 1;
                }
            }
            winUI.SetActive(false);
            playing = true;
        }

        public void MatchGraphicsToStates()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    squares[j][i].GetComponent<SpriteRenderer>().sprite = Utils.SquareStateToSprite(squareStates[j][i]);
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