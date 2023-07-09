using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGrid : MonoBehaviour
{
    [SerializeField]
    private int exitx;
    [SerializeField]
    public int exity;

    public int width;
    public int height;

    public float terrainHeightThreshold = 0.5f;

    public Array2D<int> grid;

    List<List<int>> directions = new List<List<int>> {
        new List<int>{0, 1},
        new List<int>{1, 0},
        new List<int>{0, -1},
        new List<int>{-1, 0}
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoGridRebuild()
    {
        Debug.Log("Rebuilding grid");
        if(grid.width != width || grid.height != height)
        {
            grid = new Array2D<int>(width, height, 0);
        }
        else
        {
            grid.ForEach((x, y, t) => 0);
        }

        ExitObject exitObject = GameObject.FindObjectOfType<ExitObject>();
        exitx = exitObject.GetX();
        exity = exitObject.GetZ();
        Debug.Log("exitx " + exitx + " exity " + exity);
        
        // Apply blocking objects
        BlockingObject[] blockers = GameObject.FindObjectsOfType<BlockingObject>();
        Debug.Log("Found " + blockers.Length + " blockers");
        for(int i = 0; i < blockers.Length; i++)
        {
            BlockingObject blocker = blockers[i];
            Debug.Log("Found blocker of size " + blocker.width + ", " + blocker.height);
            blocker.SnapToGrid();
            blocker.UpdateGeometry();
            for(int x = 0; x < blocker.width; x++)
            {
                for(int y = 0; y < blocker.height; y++)
                {
                    Debug.Log("applying grid spot " + (blocker.GetX()+x) + ", " + (blocker.GetZ()+y));
                    grid.Set(blocker.GetX() + x, blocker.GetZ() + y, 1);
                }
            }
        }

        // Apply landscape blocking
        if(Terrain.activeTerrain != null)
        {
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    if(grid.Get(x, y) == 0)
                    {
                        float height = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, y));
                        if(height > terrainHeightThreshold)
                        {
                            grid.Set(x, y, 1);
                        }
                    }
                }
            }
        }
    }

    public void removeBlocker(BlockingObject blocker)
    {
        for(int x = 0; x < blocker.width; x++)
        {
            for(int y = 0; y < blocker.height; y++)
            {
                Debug.Log("applying grid spot " + (blocker.GetX()+x) + ", " + (blocker.GetZ()+y));
                grid.Set(blocker.GetX() + x, blocker.GetZ() + y, 0);
            }
        }
    }

    public List<int> NextPos(int startRow, int startCol)
    {
        int n = grid.width;
        int m = grid.height;
        //Debug.Log("Grid Width" + n);

        Array2D<bool> seen = new Array2D<bool>(grid.width, grid.height, false);

        Queue<List<int>> queue = new Queue<List<int>>();

        foreach (List<int> attempt in directions)
        {
            int dx = startRow + attempt[0];
            int dy = startCol + attempt[1];
            
            if (dx >= 0 && dx < n && dy < m && dy >= 0 && !seen.Get(dx, dy) && (grid.Get(dx, dy) != 1))
            {
                seen.Set(dx, dy, true);
                queue.Enqueue(new List<int>{dx, dy, dx, dy});
            }
        }

        while (queue.TryDequeue(out List<int> node))
        {
            int row = node[0];
            int col = node[1];
            int answerX = node[2];
            int answerY = node[3];

            if (startRow != row || startCol != col)
            {
                if (row == exitx && col == exity)
                {
                    return new List<int> {answerX, answerY};
                }
            }
            foreach (List<int> attempt in directions)
            {
                int dx = row + attempt[0];
                int dy = col + attempt[1];
            
                if (dx >= 0 && dx < n && dy < m && dy >= 0 && !seen.Get(dx, dy) && (grid.Get(dx, dy) != 1))
                {
                    seen.Set(dx, dy, true);
                    queue.Enqueue(new List<int>{dx, dy, answerX, answerY});
                }
            }
        }
        

        return new List<int> {-1, -1};
    }
}
