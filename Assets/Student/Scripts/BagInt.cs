using UnityEngine;
using System.Collections.Generic

public class BagInt
{ 
    int[] ints;
    int minSize;
    int currentIndex;
    public BagInt()
    {
        minSize = 4;
        ints = new int[minSize];
        currentIndex = 0;
    }

    public void add(int value)
    {
        ints[currentIndex++] = value;

    }


    private void reSizeBigger()
    {
        int[] newInts= new int[ints.Length*2];

        
    }

    private void reSizeSmaller()
    {
        int[] newInts

    }
    
}
