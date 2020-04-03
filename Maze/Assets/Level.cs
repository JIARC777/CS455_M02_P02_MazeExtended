using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Level
{
    void StartAt(Location location);
    Location MakeConnection(Location location);
}
public class Location
{
    public int x;
    public int y;
    public Location(int xToUse, int yToUse)
    {
        x = xToUse;
        y = yToUse;
    }
}
public class Connections
{
    public bool inMaze = false;
    public bool[] directions = { false, false, false, false };
}
public class GridLevel: Level
{
    Vector3[] neighbors = { new Vector3(1, 0, 0), new Vector3(0, 1, 1), new Vector3(0, -1, 2), new Vector3(-1, 0, 3) };
    int width;
    int height;

    public Connections[,] cells;
    public GridLevel(int w, int h)
    {
        width = w;
        height = h;
        cells = new Connections[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                cells[i, j] = new Connections();
            }
        }
    }
    public void StartAt(Location location)
    {
        cells[location.x, location.y].inMaze = true;
    }
    bool canPlaceCorridor(int x, int y, int dirn)
    {
        return (0 <= x && x < width) && (0 <= y && y < height) && !cells[x, y].inMaze;
    }

    public void shuffleNeighbors()
    {
        int n = neighbors.Length;
        while (n > 1)
        {
            n--;
            int i = Random.Range(0, n);
            Vector3 v = neighbors[i];
            neighbors[i] = neighbors[n];
            neighbors[n] = v;
        }
    }
    public Location MakeConnection(Location location)
    {
        shuffleNeighbors();
        int x = location.x;
        int y = location.y;
        foreach (Vector3 v in neighbors)
        {
            int nx = x + (int)v.x;
            int ny = y + (int)v.y;
            int dirn = (int)v.z;
            int fromDirn = 3 - dirn;
            if (canPlaceCorridor(nx, ny, fromDirn))
            {
                cells[x, y].directions[dirn] = true;
                cells[nx, ny].inMaze = true;
                cells[nx, ny].directions[fromDirn] = true;
                return new Location(nx, ny);
            }
        }
        return null;
    }
}
