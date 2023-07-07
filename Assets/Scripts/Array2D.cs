using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Array2D<T>
{
    public delegate T UpdateAll(T input, int x, int y);

    public Row<T>[] columns;
    
    public int width;
    public int height;

    public Array2D(int width, int height)
    {
        this.width = width;
        this.height = height;
        
        this.columns = new Row<T>[width];
        for (int i = 0; i < width; i++)
        {
            columns[i] = new Row<T>(height);
        }
    }

    public Array2D(int width, int height, T defaultValue)
    {
        this.width = width;
        this.height = height;
        
        this.columns = new Row<T>[width];
        for (int i = 0; i < width; i++)
        {
            columns[i] = new Row<T>(height, defaultValue);
        }
    }

    public T Get(int x, int y)
    {
        if (x < 0 || x > width || y < 0 || y > height)
        {
            Debug.LogError("Out of range");
            return default(T);
        }
        else
        {
            return columns[x].Get(y);
        }
    }

    public void Set(int x, int y, T value)
    {
        if (x < 0 || x > width || y < 0 || y > height)
        {
            Debug.LogError("Out of range");
        }
        else
        {
            columns[x].Set(y, value);
        }
    }

    public void ForEach(UpdateAll task)
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                columns[x].Set(y, task(Get(x, y), x, y));
            }
        }
    }
}

[System.Serializable]
public class Row<T>
{
    [SerializeField]
    public T[] items;

    public Row(int count)
    {
        items = new T[count];
    }

    public Row(int count, T defaultValue)
    {
        items = new T[count];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = defaultValue;
        }
    }

    public T Get(int y)
    {
        if (y < 0 || y > items.Length)
        {
            Debug.LogError("Out of range");
            return default(T);
        }
        else
        {
            return items[y];
        }
    }

    public void Set(int y, T value)
    {
        if (y < 0 || y > items.Length)
        {
            Debug.LogError("Out of range");
        }
        else
        {
            items[y] = value;
        }
    }
}