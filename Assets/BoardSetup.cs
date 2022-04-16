using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connect4;

namespace Connect4
{
    public class BoardSetup : MonoBehaviour
    {
        public SquareState[][] squareStates = GameState.InitJaggedArray(new SquareState[7][], 6); //height by width
        public GameObject[][] squares = GameState.InitJaggedArray(new GameObject[7][], 6);
        public GameObject[] rows = new GameObject[6];

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Debug.Log($"{rows[i].transform.Find((j+1).ToString()).parent.name}");
                    squares[j][i] = rows[i].transform.Find((j+1).ToString()).gameObject;

                    squares[j][i].GetComponent<SpriteRenderer>().sortingOrder = 1;
                }
            }
            Debug.Log("Set squares");
        }
    }
}