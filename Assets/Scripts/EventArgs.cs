using UnityEngine;
using System;

public class Collision2DArgs: EventArgs
{
    public GameObject CollisionObject;
    public GameObject Sender;

    public Collision2DArgs(Collider2D col, GameObject sender)
    {
        Sender = sender;
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

public class MessageArgs : EventArgs
{
    public Vector3 Position;
    public Color MsgColor;
    public string Msg;

    public MessageArgs(Vector3 position, Color color, string msg)
    {
        Position = position;
        Msg = msg;
        MsgColor = color;
    }
}

public class LevelWaveArgs : EventArgs
{
    public GhostState State;
    public LevelWaveArgs(GhostState state)
    {
        State = state;
    }
}
