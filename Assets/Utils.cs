using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connect4
{
    public class Utils : MonoBehaviour
    {
        static Sprite empty;
        static Sprite yellow;
        static Sprite red;

        public static Sprite SquareStateToSprite(SquareState state)
        {
            switch (state)
            {
                case SquareState.Empty:
                    return empty;
                case SquareState.Red:
                    return red;
                case SquareState.Yellow:
                    return yellow;
            }
            throw new ImpossibleException($"Recived squareState {state}");
        }

        public static SquareState PlayerToSquareState(int player)
        {
            if (player == 1) { return SquareState.Yellow; }
            if (player == 2) { return SquareState.Red; }
            return SquareState.Empty;
        }

        public static Sprite PlayerToSprite(int player)
        {
            if (player == 1) { return yellow; }
            if (player == 2) { return red; }
            return empty;
        }
    }
}