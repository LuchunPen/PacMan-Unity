using UnityEngine;

public class BlinkyBeh: GhostBehaviour
{
	protected override void Start ()
    {
        GetSpeedData();
        BehaviourAction = ScatterBehaviour;
    }
}
