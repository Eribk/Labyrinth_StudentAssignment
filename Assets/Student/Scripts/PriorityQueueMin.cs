using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

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
        return values[0];
    }

    public Value delMin()
    {
        
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
        keys[keyCount] = key;
        values[keyCount] = value;
        keyCount++;
        Swim(keyCount-1);
    }


    private void Swim(int i)
    {
        while (i > 0 && less(i, i/2))
        {
            Exchange(i / 2, i);
            i = i / 2;

        }
    }

    private void Sink(int i)
    {
        while (2*i<keyCount-1)
        {
            int j = 2 * i;
            if (j < keyCount - 1&&less(j,j+1)
                j++;

            if (less(i, j))
                 break;

            Exchange(j, i);
            keyCount = j;

        }
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
        values[j] = values[i];

        keys[j])= k;
        values[j] = v;
       
    }



    private void Rezise()
    {
        if (keyCount >= keys.Length / 2)
        {
            Tkey[] newKeys = new Tkey[keys.Length*2];
            Value[] newValues = new Value[values.Length*2];

            for(int i=0; i<keyCount-1; i++)
            {
                newKeys[i] = keys[i];
                newValues[i] = values[i];
            }

            keys= newKeys;
            values= newValues;

        }
        else if (keyCount <= keys.Length / 4)
        {
            Tkey[] newKeys = new Tkey[keys.Length/2];
            Value[] newValues = new Value[values.Length/2];

            for (int i = 0; i < keyCount - 1; i++)
            {
                newKeys[i] = keys[i];
                newValues[i] = values[i];
            }

            keys = newKeys;
            values = newValues;
        }
    }
}
