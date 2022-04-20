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
            SquareState[][] squareStates = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameState>().squareStates;
            SquareState playerState;

            playerState = Utils.PlayerToSquareState(player);

            #region Horizontal & Vertical

            var horizontalNeighbors = new List<SquareState>();
            for (int i = lastMove.x - 3; i <= lastMove.x + 3; i++)
            {
                try
                {
                    horizontalNeighbors.Add(squareStates[i][lastMove.y]);
                }
                catch (IndexOutOfRangeException)
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
                    verticalNeighbors.Add(squareStates[lastMove.x][i]);
                }
                catch (IndexOutOfRangeException)
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
                    TLBRDiagonal.Add(squareStates[lastMove.x + 1][lastMove.y + i]);
                }
                catch (IndexOutOfRangeException) { }
            }
            if (LongestRun(TLBRDiagonal, playerState) >= 4) { return true; }

            //top right -> bottom left
            var BLTRDiagonal = new List<SquareState>();
            for (int i = -3; i <= 3; i++)
            {
                try
                {
                    //BLTRDiagonal.Add(squareStates[lastMove.y - i, lastMove.x + i]);
                    BLTRDiagonal.Add(squareStates[lastMove.x + 1][lastMove.y - i]);
                }
                catch (IndexOutOfRangeException) { }
            }
            if (LongestRun(BLTRDiagonal, playerState) >= 4) { return true; }
            #endregion

            return false;
        }

        public static int HasPlayerWon(SquareState[][] boardStates)
        {
            for (int player = 1; player <= 2; player++)
            {
                SquareState playerState = Utils.PlayerToSquareState(player);

                if (LongestRunInPosition(boardStates, playerState) >= 4)
                {
                    return player;
                }
            }
            return 0;
        }

        //public static float PositionEvaluation(SquareState[][] boardStates, int depth)
        //{
        //    if (depth > 1)
        //    {
                
        //    }
        //}

        public static int LongestRunInPosition(SquareState[][] boardStates, SquareState desiredState)
        {
            int longestLen = 0;

            //vertical
            for (int i = 0; i < 7; i++)
            {
                int run = LongestRun(boardStates[i], desiredState);
                if (run > longestLen)
                {
                    longestLen = run;
                }
            }
            //horizontal
            for (int i = 0; i < 6; i++)
            {
                List<SquareState> row = new List<SquareState>();
                for (int j = 0; j < 7; j++)
                {
                    row.Add(boardStates[j][i]);
                }
                int run = LongestRun(row, desiredState);
                if (run > longestLen)
                {
                    longestLen = run;
                }
            }
            //TLBR diagonal
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    List<SquareState> diagonal = new List<SquareState>();
                    for (int k = 0; k < 4; k++)
                    {
                        diagonal.Add(boardStates[k + i][k + j]);
                    }
                    int run = LongestRun(diagonal, desiredState);
                    if (run > longestLen)
                    {
                        longestLen = run;
                    }
                }
            }
            //BLTR diagonal
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    List<SquareState> diagonal = new List<SquareState>();
                    for (int k = 0; k < 4; k++)
                    {
                        diagonal.Add(boardStates[k + i][-k + j + 3]);
                    }
                    int run = LongestRun(diagonal, desiredState);
                    if (run > longestLen)
                    {
                        longestLen = run;
                    }
                }
            }

            return longestLen;
        }

        public static int LongestRunInPosition(SquareState[][] boardStates, SquareState desiredState, out int numRuns)
        {
            int longestLen = 0;
            numRuns = 0;

            //vertical
            for (int i = 0; i < 7; i++)
            {
                int run = LongestRun(boardStates[i], desiredState);
                if (run > longestLen)
                {
                    longestLen = run;
                    numRuns = 0;
                }
                if (run == longestLen) { numRuns++; }
            }
            //horizontal
            for (int i = 0; i < 6; i++)
            {
                List<SquareState> row = new List<SquareState>();
                for (int j = 0; j < 7; j++)
                {
                    row.Add(boardStates[j][i]);
                }
                int run = LongestRun(row, desiredState);
                if (run > longestLen)
                {
                    longestLen = run;
                    numRuns = 0;
                }
                if (run == longestLen) { numRuns++; }
            }
            //TLBR diagonal
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    List<SquareState> diagonal = new List<SquareState>();
                    for (int k = 0; k < 4; k++)
                    {
                        diagonal.Add(boardStates[k + i][k + j]);
                    }
                    int run = LongestRun(diagonal, desiredState);
                    if (run > longestLen)
                    {
                        longestLen = run;
                        numRuns = 0;
                    }
                    if (run == longestLen) { numRuns++; }
                }
            }
            //BLTR diagonal
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    List<SquareState> diagonal = new List<SquareState>();
                    for (int k = 0; k < 4; k++)
                    {
                        diagonal.Add(boardStates[k + i][-k + j + 3]);
                    }
                    int run = LongestRun(diagonal, desiredState);
                    if (run > longestLen)
                    {
                        longestLen = run;
                        numRuns = 0;
                    }
                    if (run == longestLen) { numRuns++; }
                }
            }

            return longestLen;
        }

        public static int LongestRun(SquareState[] statesArray, SquareState desiredState)
        {
            List<SquareState> states = new List<SquareState>();
            for (int i = 0; i < states.Count; i++)
            {
                states.Add(states[i]);
            }
            return LongestRun(states, desiredState);
        }

        public static int LongestRun(List<SquareState> states, SquareState DesiredState)
        {
            int n = 0;
            int largestN = 0;
            foreach (var state in states)
            {
                if (state == DesiredState) { n++; }
                if (largestN < n) { largestN = n;}
            }
            if (largestN < n) { largestN = n;}

            return largestN;
        }
    }
}
