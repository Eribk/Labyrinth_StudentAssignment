using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class PriorityQueueMin<Tkey> where Tkey:IComparable<Tkey> 
{
    private Tkey[] keys;
    private int[] vertices;
    private bool[] visited;
    private int keyCount;
    public PriorityQueueMin(int maxSize)
    {
        keyCount = 0;
        keys = new Tkey[4];
        vertices = new int[4];
        visited = new bool[maxSize];
    }


    public int Min()
    {
        return vertices[1];
    }

    public int delMin()
    {
        int val = vertices[1];
        Exchange(1, keyCount--);
        Sink(1);
        Rezise();
        return val;
    }

    public Boolean isEmpty()
    {
        if(keyCount == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int Size()
    {
        return keyCount;
    }

    public void AddKey(Tkey key, int value)
    {

        keyCount++;

        keys[keyCount] = key;
        vertices[keyCount] = value;
        Swim(keyCount);
        visited[value] = true;
        Rezise();


    }


    private void Swim(int i)
    {
        while (i > 1 && less(i, i/2))
        {
            //Debug.Log("Swim");
            Exchange(i / 2, i);
            i = i / 2;

        }
    }

    //The sink method takes a specific index, comparest the key value to the smallest child key, and if the current index key is larger, swap places with it. 
    private void Sink(int i)
    {
        while (2*i<=keyCount)
        {
            int j = 2 * i;
            if (j < keyCount&&less(j+1,j))
                j++;

            if (less(i, j))
                 break;
            Exchange(j, i);
            i = j;

        }
    }

    //Checks if a specific value is in the priority queue. 
    public bool Visited(int val)
    {
        
        return visited[val];
    }

    private bool less(int i, int j)
    {
        return keys[i].CompareTo(keys[j])<0;
    }

    private void Exchange(int i, int j)
    {
        Tkey k = keys[i];
        int v = vertices[i];

        keys[i] = keys[j];
        vertices[i] = vertices[j];

        keys[j]= k;
        vertices[j] = v;
       
    }
    private int GetIndex(Tkey key, int val)
    {

        int index = 1;
        for(int i=1; i<=keyCount; i++)
        {
            if (vertices[i].CompareTo(val) == 0)
            {
                index = i;
                break;
            }
            
        }

        return index;



        //if (values[1].CompareTo(val)==0)
        //{
        //    return 1;
        //}
        //else
        //{
        //    if (keys[1].CompareTo(key) < 0)
        //    {
        //        return BinarySearch(key, val, 2);
        //    }
        //    else
        //    {
        //        return BinarySearch(key, val, 3);
        //    }
        //}
    }

    //private int BinarySearch(Tkey key,Value val, int i)
    //{
    //    if(values[i].CompareTo(val) == 0)
    //    {
    //        return i;
    //    }
    //    else if (keys[1].CompareTo(key) < 0)
    //    {
    //        return BinarySearch(key, val, i*2);
    //    }
    //    else
    //    {
    //        return BinarySearch(key, val, i*2+1);
    //    }
    //}

    public void DecreaseKey(Tkey oldKey, Tkey newKey,int val)
    {
        int i=GetIndex(oldKey, val);
        if (i > 0)
        {
            keys[i] = newKey;
            Swim(i);
        }

     
    }



    private void Rezise()
    {
        if (keyCount >= keys.Length / 2)
        {
            Tkey[] newKeys = new Tkey[keys.Length*2];
            int[] newValues = new int[vertices.Length*2];

            for(int i=0; i<= keyCount; i++)
            {
                newKeys[i] = keys[i];
                newValues[i] = vertices[i];
            }

            keys= newKeys;
            vertices= newValues;

        }
        else if (keyCount <= keys.Length / 4&&keys.Length>4)
        {
            Tkey[] newKeys = new Tkey[keys.Length/2];
            int[] newValues = new int[vertices.Length/2];

            for (int i = 0; i <= keyCount; i++)
            {
                newKeys[i] = keys[i];
                newValues[i] = vertices[i];
            }

            keys = newKeys;
            vertices = newValues;
        }
    }
}
