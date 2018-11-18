using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise  {
   
    public static float scale = 1f;    

    public static float[,] GeneratePerlinMap(int width, int height, int seed)
    {
        float[,] map = new float[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float xCoord = ((float)x + seed) / width * scale;
                float yCoord = ((float)y + seed) / height * scale;

                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                map[y, x] = sample;
            }
        }
        return map;
    }  
}



