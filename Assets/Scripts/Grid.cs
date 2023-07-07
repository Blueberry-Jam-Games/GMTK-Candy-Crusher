using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    int exitx = 0;
    int exity = 0;
    List<List<int>> grid = new List<List<int>> {
        new List<int>{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        new List<int>{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        new List<int>{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        new List<int>{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        new List<int>{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        new List<int>{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        new List<int>{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        new List<int>{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        new List<int>{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    };

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

    List<int> NextPos(int startRow, int startCol)
    {
        int n = grid.Count;
        int m = grid[0].Count;

        List<List<bool>> seen = new List<List<bool>>{
            new List<bool>{false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new List<bool>{false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new List<bool>{false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new List<bool>{false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new List<bool>{false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new List<bool>{false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new List<bool>{false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new List<bool>{false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new List<bool>{false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false}
        };

        Queue<List<int>> queue = new Queue<List<int>>();

        foreach (List<int> attempt in directions)
        {
            int dx = startRow + attempt[0];
            int dy = startCol + attempt[1];
            
            if (dx >= 0 && dx < n && dy < m && dy >= 0 && !seen[dx][dy] && (grid[dx][dy] != '1'))
            {
                seen[dx][dy] = true;
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
            
                if (dx >= 0 && dx < n && dy < m && dy >= 0 && !seen[dx][dy] && (grid[dx][dy] != '1'))
                {
                    seen[dx][dy] = true;
                    queue.Enqueue(new List<int>{dx, dy, answerX, answerY});
                }
            }
        }
        

        return new List<int> {-1, -1};
    }
}
