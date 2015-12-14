using UnityEngine;
using System.Collections.Generic;

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

    public PacManController PacManPref;
    public Persona InkyPref;
    public Persona PinkyPref;
    public Persona BlinkyPref;
    public Persona ClydePref;

    private List<Persona> personas = new List<Persona>();
    private List<Vector3> _playerSpawn = new List<Vector3>();
    private List<Vector3> _ghostSpawn = new List<Vector3>();

    public bool LevelStarted = false;

    void Awake()
    {
        if (_instance == null) { _instance = this; }
        else { Destroy(this.gameObject); }
    }

	void Start ()
    {
        CreateMap();
        CreateGamePersona();
        Invoke("StartLevel", 3);
    }

    void Update()
    {
        if (LevelStarted)
        {
            foreach(Persona p in personas)
            {
                p.OnUpdate();
            }
        }
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

    private void CreateMap()
    {
        _map = MapCreator.GetMap();
        if (MapVisualizer != null)
        {
            MapVisualizer.CreateOrRefreshVisualData();

            for (int x = 0; x < Map.SizeX; x++)
            {
                for (int y = 0; y < Map.SizeY; y++)
                {
                    if (Map[x,y].CType == CellType.PacManSpawn)
                    {
                        _playerSpawn.Add(new Vector3(x + 0.5f, y + 0.5f, 0));
                    }
                    else if (Map[x,y].CType == CellType.GhostHome)
                    {
                        _ghostSpawn.Add(new Vector3(x + 0.5f, y + 0.5f, 0));
                    }
                }
            }
        }
        else { Debug.Log("No map visualizator"); }
    }

    private void CreateGamePersona()
    {
        if (_playerSpawn.Count > 0)
        {
            PacManController go = Instantiate(PacManPref, _playerSpawn[Random.Range(0, _playerSpawn.Count)], Quaternion.identity) as PacManController;
            go.CollisionEvent += PacManCollisionHandler;
            personas.Add(go);
        }

        if (_ghostSpawn.Count > 0)
        {
            //Instantiate(InkyPref, _ghostSpawn[Random.Range(0, _ghostSpawn.Count)], Quaternion.identity);
            //Instantiate(PinkyPref, _ghostSpawn[Random.Range(0, _ghostSpawn.Count)], Quaternion.identity);
            //Instantiate(ClydePref, _ghostSpawn[Random.Range(0, _ghostSpawn.Count)], Quaternion.identity);
            //Instantiate(BlinkyPref, _ghostSpawn[Random.Range(0, _ghostSpawn.Count)], Quaternion.identity);
        }
    }

    void StartLevel()
    {
        LevelStarted = true;
    }

    private void PacManCollisionHandler(object sender, Collision2DArgs arg)
    {
        FoodPoint fp = arg.collisionObject.GetComponent<FoodPoint>();
        if (fp != null)
        {
            Vector3 position = fp.transform.position;
            int ix = Mathf.FloorToInt(position.x);
            int iy = Mathf.FloorToInt(position.y);

            _map[ix, iy].CType = CellType.None;
            MapVisualizer.RefreshVisualData(position);
        }
    }
}
