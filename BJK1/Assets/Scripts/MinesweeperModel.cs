using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinesweeperModel {
    public int width;
    public int height;
    public int mineCount;    
    private int seed;
    public float minSpawnValue = 0;

    private int[,] mines;
    private int[,] counts;
    private float[,] perlinMap;
    private Texture2D perlinMapTexture;


    public MinesweeperModel(int width, int height, int mineCount)
    {
        this.width = width;
        this.height = height;
        mines = new int[this.height, this.width];
        counts= new int[this.height, this.width];
        seed = Random.Range(0, 10000);
        this.mineCount = mineCount;
        initBoard();
        generateBoard();
        placeMines();        
    }

    public void initBoard()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                counts[y, x] = 0;
                mines[y, x] = 0;
            }
        }
    }

    public void generateBoard()
    {
        perlinMap = PerlinNoise.GeneratePerlinMap(width, height, seed);       
    }

    public void placeMines()
    {
        int placed = 0;
        while (placed < mineCount)
        {
            int y = Random.Range(0, height);
            int x = Random.Range(0, width);
            if (placed < mineCount)
            {
                if (perlinMap[y, x] > minSpawnValue && mines[y,x]!=1 && randomChance(perlinMap[y, x]))
                {
                    mines[y, x] = 1;
                    placed++;
                    updateCounts(x, y);
                }          

            }
        }
        generatePerlinMapTexture();
    }

    public int getCount(int x, int y)
    {
        return counts[y,x];
    }

    public void updateCounts(int x, int y)
    {
        for(int iy=y-1;iy<=y+1;iy++)
        {
            for(int ix = x - 1; ix <= x + 1; ix++)
            {
                if(ix >= 0 && ix < width && iy >= 0 && iy < height)
                {
                    counts[iy, ix]++;
                }
            }
        }
    }

  
    private bool randomChance(float successChance)
    {
        return Random.Range(0f, 1f) <= successChance;       
    }

    public void generatePerlinMapTexture()
    {
        perlinMapTexture = new Texture2D(width, height);
        for(int y=0;y<height;y++)
        {
            for(int x =0;x<width;x++)
            {
                float grey = perlinMap[y, x];
                Color c = new Color(grey, grey, grey);
                if(mines[y,x]==1)
                {
                    c = Color.red;
                }
                perlinMapTexture.SetPixel(x, toTextureHeight(y), c);
            }
        }
        perlinMapTexture.filterMode = FilterMode.Point;
        perlinMapTexture.Apply();
    }

    public int toTextureHeight(int heightInBoardCoords)
    {
        return height - heightInBoardCoords - 1;
    }

    public Texture2D getPerlinMapTexture()
    {
        return perlinMapTexture;
    }

    public int getWidth()
    {
        return width;
    }

    public int getHeight()
    {
        return height;
    }
}
