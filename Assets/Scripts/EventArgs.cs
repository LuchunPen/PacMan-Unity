using UnityEngine;
using System;

public class Collision2DArgs: EventArgs
{
    public GameObject CollisionObject;

    public Collision2DArgs(Collider2D col)
    {
        CollisionObject = col.gameObject;
    }
}

public class ScoreArgs: EventArgs 
{
    public int Score;
    public ScoreArgs(int score)
    {
        Score = score;
    }
}

public class LevelMsgArgs : EventArgs
{
    public Vector3 Position;
    public string Msg;

    public LevelMsgArgs(Vector3 position, string msg)
    {
        Position = position;
        Msg = msg;
    }
}
