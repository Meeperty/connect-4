using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connect4
{
    public class PositionEval
    {
        public static bool HasPlayerWon(Vector2Int lastMove, int player)
        {
            SquareState[,] squareStates = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameState>().squareStates;
            SquareState playerState;

            if (player == 1) { playerState = SquareState.Yellow; }
            else { playerState = SquareState.Red; }

            #region Horizontal & Vertical

            var horizontalNeighbors = new List<SquareState>();
            for (int i = lastMove.x - 3; i <= lastMove.x + 3; i++)
            {
                try
                {
                    horizontalNeighbors.Add(squareStates[lastMove.y, i]);
                }
                catch (IndexOutOfRangeException e)
                {
                    //Debug.LogError(e.Message + $"in horizontal check at index {i}");
                }
            }
            if (LongestRun(horizontalNeighbors, playerState) >= 4)
            {
                return true;
            }

            var verticalNeighbors = new List<SquareState>();
            for (int i = lastMove.y - 3; i <= lastMove.y + 3; i++)
            {
                try
                {
                    verticalNeighbors.Add(squareStates[i, lastMove.x]);
                }
                catch (IndexOutOfRangeException e)
                {
                    //Debug.LogError(e.Message + $"in vertical check at index {i}");
                }
            }
            if (LongestRun(verticalNeighbors, playerState) >= 4)
            {
                return true;
            }

            #endregion

            #region Diagonal

            //top left -> bottom right
            var TLBRDiagonal = new List<SquareState>();
            for (int i = -3; i <= 3; i++)
            {
                try
                {
                    TLBRDiagonal.Add(squareStates[lastMove.y + i, lastMove.x + i]);
                }
                catch (IndexOutOfRangeException e) { }
            }
            if (LongestRun(TLBRDiagonal, playerState) >= 4) { return true; }

            //top right -> bottom left
            var BLTRDiagonal = new List<SquareState>();
            for (int i = -3; i <= 3; i++)
            {
                try
                {
                    BLTRDiagonal.Add(squareStates[lastMove.y - i, lastMove.x + i]);
                }
                catch (IndexOutOfRangeException e) { }
            }
            if (LongestRun(BLTRDiagonal, playerState) >= 4) { return true; }
            #endregion

            return false;
        }

        public static int LongestRun(List<SquareState> states, SquareState desiredState)
        {
            int n = 0;
            int largestN = 0;
            foreach (var state in states)
            {
                if (state == desiredState) { n++; }
                if (largestN < n) { largestN = n;}
            }
            if (largestN < n) { largestN = n;}

            return largestN;
        }
    }
}
