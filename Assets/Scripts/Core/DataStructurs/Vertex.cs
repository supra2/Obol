using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Implementation of a generic vertex to be used in any graph
/// </summary>
[Serializable]
public class Vertex<T>
{
    [NonSerialized]
    List<Vertex<T>> neighbors;

    T value;
    bool isVisited;

    public List<Vertex<T>> Neighbors { get { return neighbors; } set { neighbors = value; } }
    public T Value { get { return value; } set { this.value = value; } }
    public bool IsVisited { get { return isVisited; } set { isVisited = value; } }
    public int NeighborsCount { get { return neighbors.Count; } }

    public Vertex(T value)
    {
        this.value = value;
        isVisited = false;
        neighbors = new List<Vertex<T>>();
    }

    public Vertex(T value, List<Vertex<T>> neighbors)
    {
        this.value = value;
        isVisited = false;
        this.neighbors = neighbors;
    }

    public void Visit()
    {
        isVisited = true;
    }


    public override string ToString()
    {
        StringBuilder allNeighbors = new StringBuilder("");
        allNeighbors.Append(value + ": ");
        foreach (Vertex<T> neighbor in neighbors)
        {
            allNeighbors.Append(neighbor.value + "  ");
        }
        return allNeighbors.ToString();
    }

}
