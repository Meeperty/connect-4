using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connect4
{
    public class Utils : MonoBehaviour
    {
        static GameState mainState;
        
        static Sprite empty;
        static Sprite yellow;
        static Sprite red;

        public void Start()
        {
            mainState = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameState>();
            empty = mainState.empty;
            yellow = mainState.yellow;
            red = mainState.red;
        }

        public static double TimeFunction(Action function)
        {
            DateTime before = DateTime.Now;
            for (int i = 0; i < 1000; i++)
            {
                function();
            }
            DateTime after = DateTime.Now;
            return (after - before).TotalMilliseconds / 1000;
        }

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

        public static int SquareStateToPlayer(SquareState state)
        {
            if (state == SquareState.Yellow) { return 1; }
            if (state == SquareState.Red) { return 2; }
            return 0;
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

    [System.Serializable]
    public class ImpossibleException : System.Exception
    {
        public ImpossibleException() { }
        public ImpossibleException(string message) : base(message) { }
        public ImpossibleException(string message, System.Exception inner) : base(message, inner) { }
        protected ImpossibleException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}