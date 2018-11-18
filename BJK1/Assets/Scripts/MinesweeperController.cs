using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinesweeperController : MonoBehaviour {

    public int width;
    public int height;
    public int mineCount;
    public int countDirection = 1;

    public bool useMovementInput = true;

    private float blinkTime = .5f;
    private float blinkCount = 0f;

    int currentX = 0;
    int currentY = 0;

    public GameObject displayObject;
    private Renderer rend;

    private int blockWidth;
    private int blockHeight;
    private int scaledWidth;
    private int scaledHeight;

    public List<Texture2D> numbers;

    public Texture2D closed;
    public Texture2D cursor;


    public bool numberDisplay = true;


    private MinesweeperModel game;

    Texture2D number;//global to fix nasty memory leak with temp tex2d in unity
    Texture2D board;
    Texture2D perlinBoard;
    // Use this for initialization
    void Start () {
       
        initGame();
	}

    public void initGame()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        rend = displayObject.GetComponent<Renderer>();
        game = new MinesweeperModel(width, height, mineCount);


        blockWidth = numbers[0].width;
        blockHeight = numbers[0].height;

        scaledWidth = width * blockWidth;
        scaledHeight = height * blockHeight;
        board = new Texture2D(scaledWidth, scaledHeight);

        game.getSafeStart(out currentX, out currentY);        
    }

	// Update is called once per frame
	void Update () {
        procInput();
        blinkCount += Time.deltaTime * countDirection;     
       
       
        if (blinkCount>=blinkTime)
        {
            countDirection = -1;
        }else if(blinkCount<=0)
        {
            countDirection = 1;
        }       


        if (numberDisplay)
        {

            board = getNumberBoard();
            if (countDirection > 0)
            {
                drawCursor(board);
            }
            rend.material.mainTexture = board;
        }
        else
        {
            perlinBoard = game.getPerlinMapTexture();
            rend.material.mainTexture = perlinBoard;
        }

        
	}
    public void procInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) || useMovementInput)
        {
            clickCurrentPosition();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            currentX++;
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            currentX--;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            currentY--;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentY++;
        }

       currentX = Mathf.Clamp(currentX, 0, width - 1);
       currentY = Mathf.Clamp(currentY, 0, height - 1);

        
    }

    public void clickCurrentPosition()
    {
        if(game.clickSpace(currentX, currentY))
        {
            Debug.Log("Game Over");
        }
    }

    public Texture2D getNumberBoard()
    {       
        for (int y=0;y< height; y++)
        {
            for(int x=0;x< width; x++)
            {
                int count = game.getCount(x, y);
                number = count==-1?closed : numbers[count];
                board.SetPixels(x*blockWidth, scaledHeight - ((y+1)*blockHeight), blockWidth, blockHeight, number.GetPixels());
                          
            }
        }
        board.Apply();
          
        return board;
        
    }

    public void drawCursor(Texture2D tex)
    {
      tex.SetPixels(currentX*blockWidth, scaledHeight - ((currentY + 1) * blockHeight), blockWidth, blockHeight,cursor.GetPixels());
      tex.Apply();
    }
}
