using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Maze : MonoBehaviour
{
    private Slider widthSlider;
    private Slider heightSlider;
    private Text widthText;
    private Text heightText;

    public int width;
    public int height;
    public Location mazeStart = new Location(0, 0);
    GridLevel maze;
    bool hasExit = false;

    public void createMaze()
    {
        maze = new GridLevel(width, height);
        GenerateMaze(maze, mazeStart);
    }
    public void GenerateMaze(Level maze, Location startingPoint)
    {
        Stack<Location> locations = new Stack<Location>();
        locations.Push(startingPoint);
        maze.StartAt(startingPoint);
        while (locations.Count > 0)
        {
            Location currentLoc = locations.Peek();
            Location next = maze.MakeConnection(currentLoc);
            if (next != null)
                locations.Push(next);
            else
                locations.Pop();
        }

    }
    public void Start()
    {
        widthSlider = GameObject.Find("widthSlider").GetComponent<Slider>();
        heightSlider = GameObject.Find("heightSlider").GetComponent<Slider>();
        widthText = GameObject.Find("widthText").GetComponent<Text>();
        heightText = GameObject.Find("heightText").GetComponent<Text>();
    }
    public void Update()
    {
        width = (int)widthSlider.value;
        height = (int)heightSlider.value;
        widthText.text = width.ToString();
        heightText.text = height.ToString();
        if (maze != null)
            debugLineMaze(maze);
    }

    // Debug line from professor Slease's code example
    public void debugLineMaze(GridLevel maze)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Connections currentCell = maze.cells[x, y];
                if (maze.cells[x, y].inMaze)
                {
                    Vector3 cellPos = new Vector3(x, 0, y);
                    float lineLength = 1f;
                    if (currentCell.directions[0])
                    {
                        // positive x
                        Vector3 neighborPos = new Vector3(x + lineLength, 0, y);

                        Debug.DrawLine(cellPos, neighborPos, Color.cyan);
                    }
                    if (currentCell.directions[1])
                    {
                        // positive y
                        Vector3 neighborPos = new Vector3(x, 0, y + lineLength);
                        Debug.DrawLine(cellPos, neighborPos, Color.cyan);
                    }
                    if (currentCell.directions[2])
                    {
                        // negative y
                        Vector3 neighborPos = new Vector3(x, 0, y - lineLength);
                        Debug.DrawLine(cellPos, neighborPos, Color.cyan);
                    }
                    if (currentCell.directions[3])
                    {
                        // negative x
                        Vector3 neighborPos = new Vector3(x - lineLength, 0, y);
                        Debug.DrawLine(cellPos, neighborPos, Color.cyan);
                    }
                }
            }
        }
    }

    public void clearMaze()
    {
        maze = null;
    }
}
