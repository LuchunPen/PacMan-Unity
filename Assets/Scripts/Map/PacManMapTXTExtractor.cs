using UnityEngine;
using System;

public class PacManMapTXTExtractor: MonoBehaviour
{
    public TextAsset txtmap;

    void Start ()
    {

	}
	
	void Update ()
    {
	
	}

    public Map2DCyclic<PMNode> GetMap()
    {
        string[] maptext = txtmap.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        string[][] mapcells = new string[maptext.Length][];

        int rowlenght = 0;
        for (int i = 0; i < maptext.Length; i++)
        {
            mapcells[i] = maptext[i].Split(' ');
            if (rowlenght == 0) { rowlenght = mapcells[i].Length; continue; }

            if (rowlenght != mapcells[i].Length)
            {
                Debug.LogWarning(string.Format("Map error, cell rows is different {0} / {1} on {2} row", rowlenght, mapcells[i].Length, i));
                mapcells = null;
                break;
            }
        }
        return MapCreator(mapcells);
    }

    private Map2DCyclic<PMNode> MapCreator(string[][] maptext)
    {
        if (maptext == null) { return null; }
        if (maptext.Length == 0) { return null; }
        if (maptext[0].Length == 0) { return null; }

        int sizeX = maptext[0].Length;
        int sizeY = maptext.Length;

        Map2DCyclic<PMNode> map = new Map2DCyclic<PMNode>(sizeX * 2, sizeY);
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                int dx = map.SizeX - x - 1;
                int dy = map.SizeY - y - 1;

                map[x, dy] = GetNode(maptext[y][x]);

                //mirror half
                map[dx, dy] = GetNode(maptext[y][x]);
            }
        }
        return map;
    }

    private PMNode GetNode(string s)
    {
        PMNode node = new PMNode();
        switch (s)
        {
            case "W": node.CType = CellType.Wall; break;
            case "o": node.CType = CellType.Point; break;
            case "A": node.CType = CellType.Energizer; break;
            case "D": node.CType = CellType.Door; break;
            case "T": node.CType = CellType.Tunnel; break;
            case "G": node.CType = CellType.GhostHome; break;
            case "P": node.CType = CellType.PacManSpawn; break;
            case "E": node.CType = CellType.HomeEnter; break;
        }
        return node;
    }
}
