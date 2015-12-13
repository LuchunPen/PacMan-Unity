using UnityEngine;

public class GameManager: MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameManager gm = new GameObject("GameManager").AddComponent<GameManager>();
                _instance = gm;
            }
            return _instance;
        }
    }


    void Awake()
    {
        if (_instance == null) { _instance = this; }
        else { Destroy(this.gameObject); }
    }

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}
}
