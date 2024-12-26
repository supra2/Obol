using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Implementation of a generic, unweighted, undirected graph
/// </summary>
[Serializable]
public class UndirectedGenericGraph<T>: ISerializationCallbackReceiver
{

    #region member
    private SerializableDictionary<Vertex<T>, List<Vertex<T>>> AdjacencyMatrices;
    #endregion

    public List<Vertex<T>> Vertices { get { return (List<Vertex<T>>)AdjacencyMatrices.Values; } }
    public int Size { get { return AdjacencyMatrices.Count; } }

    public UndirectedGenericGraph( )
    {
        AdjacencyMatrices = new  SerializableDictionary<Vertex<T>, List<Vertex<T>>>();
    }

    public UndirectedGenericGraph(List<Vertex<T>> initialNodes)
    {
        if (initialNodes != null)
        {
          foreach( Vertex<T> v in initialNodes )
          {
                AdjacencyMatrices.Add(v, new List<Vertex<T>>());
          }
        }
    }

    public void AddVertex(Vertex<T> vertex)
    {
        AdjacencyMatrices.Add(vertex,new List<Vertex<T>>());
    }

    public void RemoveVertex(Vertex<T> vertex)
    {
        AdjacencyMatrices.Remove(vertex);
    }

    public bool HasVertex(Vertex<T> vertex)
    {
        return AdjacencyMatrices.ContainsKey(vertex);
    }


    /// <summary>
    /// Reccursive search depth first
    /// </summary>
    /// <param name="root"></param>
    public List<Vertex<T>> RDepthFirstSearch(Vertex<T> root,Vertex<T> destination)
    {
       ResetVertex();
       return DepthFirstSearch( destination,new List<Vertex<T>>() { root });
    }

    protected List<Vertex<T>> DepthFirstSearch( Vertex<T> destination,List<Vertex<T>> path )
    {
        var last = path[path.Count - 1];
        if (!last.IsVisited)
        {
            last.Visit();
            if(destination == last)
            {
                path.Add(last);
                return path;
            }
            foreach (Vertex<T> neighbor in last.Neighbors)
            {
                var newPath = new List<Vertex<T>>(path);
                newPath.Add(neighbor);
                var resultpath = DepthFirstSearch(neighbor, newPath);
                if(resultpath != null)
                {
                    return resultpath;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// BFS search in an undirected graph implementation
    /// Non reccursive based on a queue
    /// </summary>
    /// <param name="root"> Starting Vertex</param>
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


    // remove visited flags on vertex 
    protected  void ResetVertex()
    {
        foreach(Vertex<T> vertex in Vertices)
        {
            vertex.IsVisited = false;
        }
    }

    public void OnBeforeSerialize()
    {

       
    }
    
    public void OnAfterDeserialize()
    {
        foreach (KeyValuePair<Vertex<T>, List<Vertex<T>>> adjacentMatrice in
            AdjacencyMatrices)
        {
           AddEdges(adjacentMatrice.Key, adjacentMatrice.Value);
        }
    }

    public void AddEdge(Vertex<T> from, Vertex<T> to)
    {
        if(!from.Neighbors.Contains(to))
            from.Neighbors.Add(to);
    }

    public void AddEdges(Vertex<T> from , List<Vertex<T>> newNeighbors)
    {
        if (newNeighbors != null)
        {
            foreach (Vertex<T> to in newNeighbors)
            { 
                AddEdge(from, to);
            }
        }
    }
    
    public void RemoveEdge(Vertex<T> from, Vertex<T> vertex)
    {
        from.Neighbors.Remove(vertex);
    }

    public int Distance(Vertex<T> v1, Vertex<T> v2)
    {
        return RDepthFirstSearch(v1,v2).Count;
    }

}