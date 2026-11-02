using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class PriorityQueueMin<Tkey,Value> where Tkey:IComparable<Tkey> where Value:IComparable<Value>
{
    private Tkey[] keys;
    private Value[] values;

    private int keyCount;
    public PriorityQueueMin()
    {
        keyCount = 0;
        keys = new Tkey[4];
        values = new Value[4];
    }


    public Value Min()
    {
        return values[1];
    }

    public Value delMin()
    {
        Value val = values[1];
        //Debug.Log("Delete min");
        //Debug.Log("Min value"+ val);
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

    public void AddKey(Tkey key, Value value)
    {

        keyCount++;
        //Debug.Log("Keys length: " + keys.Length);
        //Debug.Log("Number of keys: " + keyCount);
        //Debug.Log("Value: " + value);
        keys[keyCount] = key;
        values[keyCount] = value;
        //Debug.Log("Min value in queue: " + values[keyCount]);
        Swim(keyCount);
        //Debug.Log("Min value after swim: " + values[keyCount]);
        Rezise();
        //Debug.Log("Min value after rezise: " + values[keyCount]);


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

    private void Sink(int i)
    {
        while (2*i<=keyCount)
        {
            int j = 2 * i;
            if (j < keyCount&&less(j+1,j))
                j++;

            if (less(i, j))
                 break;
            //Debug.Log("Sink");
            Exchange(j, i);
            i = j;

        }
    }

    public bool Contains(Value val)
    {
        bool contains = false;
        for(int i = 1; i < keyCount+1; i++)
        {
            if(values[i] == null)
            {
                continue;
            }
            if (values[i].CompareTo(val)==0)
            {
                contains=true; 
                break;
            }
        }

        return contains;
    }

    private bool less(int i, int j)
    {
        return keys[i].CompareTo(keys[j])<0;
    }

    private void Exchange(int i, int j)
    {
        Tkey k = keys[i];
        Value v = values[i];

        keys[i] = keys[j];
        values[i] = values[j];

        keys[j]= k;
        values[j] = v;
       
    }
    private int GetIndex(Tkey key, Value val)
    {

        int index = 1;
        for(int i=1; i<=keyCount; i++)
        {
            if (values[i].CompareTo(val) == 0)
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

    public void DecreaseKey(Tkey oldKey, Tkey newKey,Value val)
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
            Value[] newValues = new Value[values.Length*2];

            for(int i=0; i<= keyCount; i++)
            {
                newKeys[i] = keys[i];
                newValues[i] = values[i];
            }

            keys= newKeys;
            values= newValues;

        }
        else if (keyCount <= keys.Length / 4&&keys.Length>4)
        {
            Tkey[] newKeys = new Tkey[keys.Length/2];
            Value[] newValues = new Value[values.Length/2];

            for (int i = 0; i <= keyCount; i++)
            {
                newKeys[i] = keys[i];
                newValues[i] = values[i];
            }

            keys = newKeys;
            values = newValues;
        }
    }
}
