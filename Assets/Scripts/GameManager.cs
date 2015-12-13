using UnityEngine;

public partial class GameManager: MonoBehaviour
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
        CreateMap();
    }
	
	void Update ()
    {
	
	}
}

public partial class GameManager
{
    private Map2DCyclic<PMNode> _map;
    public Map2DCyclic<PMNode> Map
    {
        get { return _map; }
    }

    public PacManMapTXTExtractor MapCreator;
    public PacManMapVisualizer MapVisualizer;

    public void CreateMap()
    {
        _map = MapCreator.GetMap();
        if (MapVisualizer != null)
        {
            MapVisualizer.CreateOrRefreshVisualData();
        }
        else { Debug.Log("No map visualizator"); }
    }
}
