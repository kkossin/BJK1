using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinesweeperController : MonoBehaviour {

    public int width;
    public int height;
    public int mineCount;
    public int countDirection = 1;

    private float blinkTime = .5f;
    private float blinkCount = 0f;

    int currentX = 0;
    int currentY = 0;

    public GameObject displayObject;
    private Renderer rend;

    public List<Texture2D> numbers;

    public bool numberDisplay = true;


    private MinesweeperModel game;

    Texture2D number;//global to fix nasty memory leak with temp tex2d in unity
    Texture2D board;
    // Use this for initialization
    void Start () {
        Random.InitState(System.DateTime.Now.Millisecond);
        rend = displayObject.GetComponent<Renderer>();
        game = new MinesweeperModel(width, height, mineCount);
        initTextures();
	}

    public void initTextures()
    {
        int blockWidth = numbers[0].width;
        int blockHeight = numbers[0].height;

        int scaledWidth = width * blockWidth;
        int scaledHeight = height * blockHeight;
        board = new Texture2D(scaledWidth, scaledHeight);
    }

	// Update is called once per frame
	void Update () {
        blinkCount += Time.deltaTime * countDirection;
        Texture2D board=getNumberBoard();

        if (!numberDisplay)
        {
            board= game.getPerlinMapTexture();
        }
       
       
        if (blinkCount>=blinkTime)
        {
            countDirection = -1;
        }else if(blinkCount<=0)
        {
            countDirection = 1;
        }

        if(countDirection>0)
        {
            drawCursor(board);
        }


        rend.material.mainTexture = board;
	}

    public Texture2D getNumberBoard()
    {

        int blockWidth = numbers[0].width;
        int blockHeight = numbers[0].height;

        int scaledWidth = width * blockWidth;
        int scaledHeight = height * blockHeight;
        for (int y=0;y< height; y++)
        {
            for(int x=0;x< width; x++)
            {
                number= numbers[game.getCount(x, y)];
                board.SetPixels(x*blockWidth, scaledHeight - ((y+1)*blockHeight), blockWidth, blockHeight, number.GetPixels());
                number.hideFlags = HideFlags.HideAndDontSave;               
            }
        }
        board.Apply();
        board.hideFlags = HideFlags.HideAndDontSave;      
        return board;
        
    }

    public void drawCursor(Texture2D tex)
    {
        //tex.SetPixel(currentX, height - currentY - 1, Color.yellow);
       // tex.Apply();
    }
}
