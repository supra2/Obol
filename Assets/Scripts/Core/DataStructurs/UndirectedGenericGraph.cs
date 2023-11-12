using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a generic, unweighted, undirected graph
/// </summary>
[Serializable]
public class UndirectedGenericGraph<T>
{

    // The list of vertices in the graph
    private List< Vertex<T> > vertices;

    // The number of vertices
    [SerializeField]
    int size;

    public List<Vertex<T>> Vertices { get { return vertices; } }
    public int Size { get { return vertices.Count; } }

    public UndirectedGenericGraph(int initialSize)
    {
        if (size < 0)
        {
            throw new ArgumentException("Number of vertices cannot be negative");
        }
        size = initialSize;
        vertices = new List<Vertex<T>>(initialSize);
    }

    public UndirectedGenericGraph(List<Vertex<T>> initialNodes)
    {
        vertices = initialNodes;
        size = vertices.Count;
    }

    public void AddVertex(Vertex<T> vertex)
    {
        vertices.Add(vertex);
    }

    public void RemoveVertex(Vertex<T> vertex)
    {
        vertices.Remove(vertex);
    }

    public bool HasVertex(Vertex<T> vertex)
    {
        return vertices.Contains(vertex);
    }

    public void DepthFirstSearch(Vertex<T> root)
    {
        ResetVertex();
        if (!root.IsVisited)
        {
            root.Visit();

            foreach (Vertex<T> neighbor in root.Neighbors)
            {
                DepthFirstSearch(neighbor);
            }

        }
    }


    public void BreadthFirstSearch(Vertex<T> root)
    {
        ResetVertex();
        Queue<Vertex<T>> queue = new Queue<Vertex<T>>();

        root.Visit();

        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            Vertex<T> current = queue.Dequeue();

            foreach (Vertex<T> neighbor in current.Neighbors)
            {
                if (!neighbor.IsVisited)
                {
                    neighbor.Visit();
                    queue.Enqueue(neighbor);
                }
            }
        }

    }

    protected  void ResetVertex()
    {
        foreach(Vertex<T> vertex in Vertices)
        {
            vertex.IsVisited = false;
        }
    }

}

