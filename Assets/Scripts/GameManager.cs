using UnityEngine;
using System;
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
                GameManager gm = Resources.Load("Prefabs/GameManager", typeof(GameManager)) as GameManager;
                gm = Instantiate(gm);
                _instance = gm;
            }
            return _instance;
        }
    }

    public EventHandler<ScoreArgs> OnCurrentScoreChange;
    public EventHandler<LevelMsgArgs> OnLevelMessageChange;
    public EventHandler<ScoreArgs> OnPlayerLifeChange;

    private int _currentScore;
    private int _playerLife;

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
        PrepareLevel();
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


    private void PrepareLevel()
    {
        CreateMap();
        CreateGamePersona();

        OnTriggerPlayerLifeChange();
        OnTriggerLevelMessageChange(new Vector3(14, 15.5f, 0), "READY!");

        Invoke("StartLevel", 3);
    }

    void StartLevel()
    {
        LevelStarted = true;
        OnTriggerLevelMessageChange(new Vector3(14, 15.5f, 0), "");
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
            PacManController go = Instantiate(PacManPref, _playerSpawn[UnityEngine.Random.Range(0, _playerSpawn.Count)], Quaternion.identity) as PacManController;
            go.CollisionEvent += PacManCollisionHandler;
            go.PackManDie += OnPlayerDieHandler;
            personas.Add(go);

            _playerLife = 2;
        }

        if (_ghostSpawn.Count > 0)
        {
            //Instantiate(InkyPref, _ghostSpawn[Random.Range(0, _ghostSpawn.Count)], Quaternion.identity);
            //Instantiate(PinkyPref, _ghostSpawn[Random.Range(0, _ghostSpawn.Count)], Quaternion.identity);
            //Instantiate(ClydePref, _ghostSpawn[Random.Range(0, _ghostSpawn.Count)], Quaternion.identity);
            //Instantiate(BlinkyPref, _ghostSpawn[Random.Range(0, _ghostSpawn.Count)], Quaternion.identity);
        }
    }

    private void PacManCollisionHandler(object sender, Collision2DArgs arg)
    {
        FoodPoint fp = arg.CollisionObject.GetComponent<FoodPoint>();
        if (fp != null)
        {
            _currentScore += fp.Point; OnTriggerCurrentScoreChange();

            Vector3 position = fp.transform.position;
            int ix = Mathf.FloorToInt(position.x);
            int iy = Mathf.FloorToInt(position.y);

            _map[ix, iy].CType = CellType.None;
            MapVisualizer.RefreshVisualData(position);
        }
    }

    private void OnTriggerCurrentScoreChange()
    {
        if (OnCurrentScoreChange != null) { OnCurrentScoreChange(this, new ScoreArgs(_currentScore)); }
    }
    private void OnTriggerLevelMessageChange(Vector3 position, string msg)
    {
        if (OnLevelMessageChange != null) { OnLevelMessageChange(this, new LevelMsgArgs(position, msg)); }
    }
    private void OnTriggerPlayerLifeChange()
    {
        if (OnPlayerLifeChange != null) { OnPlayerLifeChange(this, new ScoreArgs(_playerLife)); }
    }
    private void OnPlayerDieHandler(object sender, EventArgs args)
    {
        _playerLife--; OnTriggerPlayerLifeChange();
    }
}
