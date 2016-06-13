// remember you can NOT have even numbers of height or width in this style of block maze
// to ensure we can get walls around all tunnels...  so use 21 x 13 , or 7 x 7 for examples.
 
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class MazeGenerator : MonoBehaviour {
    public int width, height;
    public Transform headUITransform, walls, sky;
    public GameObject finishObject;
    public Material wallMat, floorMat;
	public bool showCeiling = true;
	public int scale = 1;
    private int[,] Maze;
    private List<Vector3> pathMazes = new List<Vector3>();
    private Stack<Vector2> _tiletoTry = new Stack<Vector2>();
    private List<Vector2> offsets = new List<Vector2> { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
    private System.Random rnd = new System.Random();
    private Vector2 _currentTile;
    public String MazeString;

    public UIController uiController;

	private bool exitCreated = false;




	public Transform playerTransform;
 
    public Vector2 CurrentTile {
        get { return _currentTile; }
        private set {
            if (value.x < 1 || value.x >= this.width - 1 || value.y < 1 || value.y >= this.height - 1){
                throw new ArgumentException("Width and Height must be greater than 2 to make a maze");
            }
            _currentTile = value;
        }
    }

    void Start() {
		MakeBlocks();
	}
 
// ============= subroutines ============
 
    void MakeBlocks() {
   
        Maze = new int[width, height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++)  {
				Maze[x, y] = 1;
            }
        }
        CurrentTile = Vector2.one;
        _tiletoTry.Push(CurrentTile);
        // Maze = CreateMaze();  // generate the maze in Maze Array.

        // Maze = 
        int randomMazeIdx = UnityEngine.Random.Range(1, 6);
        if(randomMazeIdx == 1) {
            Maze = maze1;
        }else if (randomMazeIdx == 2) {
            Maze = maze2;
        }else if (randomMazeIdx == 3) {
            Maze = maze3;
        }else if (randomMazeIdx == 4) {
            Maze = maze4;
        }else if (randomMazeIdx == 5) {
            Maze = maze5;
        }

// };


        GameObject ptype = null;
        for (int i = 0; i <= Maze.GetUpperBound(0); i++)  {
            for (int j = 0; j <= Maze.GetUpperBound(1); j++) {
                if (Maze[i, j] == 1)  {
                    MazeString=MazeString+"X";  // added to create String

					bool shouldCreateExit = !exitCreated && i == Maze.GetUpperBound(0) && CanPutExitHere(i, j) && CanPutExitHere(i, j+1);
					if (shouldCreateExit) {
						exitCreated = true;
						// finishObject.transform.localScale = new Vector3(scale, scale, scale * 2);
						finishObject.transform.position = new Vector3(i * finishObject.transform.localScale.x, -4, j * scale + (finishObject.transform.localScale.z - scale)/2);
						finishObject.transform.parent = walls;

						j++;
					}else {
						ptype = GameObject.CreatePrimitive(PrimitiveType.Cube);
						ptype.transform.localScale = new Vector3(scale, scale, scale);
						ptype.transform.position = new Vector3(i * ptype.transform.localScale.x, -4, j * ptype.transform.localScale.z);
				
						if (wallMat != null)  { ptype.GetComponent<Renderer>().material = wallMat; }
						ptype.transform.parent = walls;
					}
                }
                else if (Maze[i, j] == 0) {
                    MazeString=MazeString+"0"; // added to create String
                    pathMazes.Add(new Vector3(i, 0, j));

					//ADD FLOOR

					ptype = GameObject.CreatePrimitive(PrimitiveType.Cube);
					ptype.transform.localScale = new Vector3(scale, scale, scale);
                    ptype.transform.position = new Vector3(i * ptype.transform.localScale.x, -4, j * ptype.transform.localScale.z);
               
                    if (wallMat != null)  { ptype.GetComponent<Renderer>().material = floorMat; }
                    ptype.transform.parent = transform;
                }
            }
            MazeString=MazeString+"\n";  // added to create String
        }

        //SETTING PLAYER
        if (UnityEngine.Random.value >= 0.5) {
            for (int j = 0; j <= Maze.GetUpperBound(1); j++) {
                if (Maze[1, j] == 0) {
                    PutPlayerInMaze(j);
                    break;
                }   
            }
        }else {
            for (int j = Maze.GetUpperBound(1); 0 <= j; j--) {
                if (Maze[1, j] == 0) {
                    PutPlayerInMaze(j);
                    break;
                }   
            }
        }


        sky.transform.localScale = new Vector3(width*scale * 3, 1, height * scale * 3);
        sky.transform.position = new Vector3(0,3, 0);
        uiController.ShowStartButton();
        print (MazeString);  // added to create String
    }

    void PutPlayerInMaze (int j) {
        playerTransform.position  =  new Vector3(scale, -1.02f, j * scale);
        foreach (var offset in offsets) {
            if (Maze[(int)(1 + offset.x), (int)(j + offset.y)] == 0) {
                playerTransform.LookAt(new Vector3((1 + offset.x) * scale, -1.02f, (j + offset.y) * scale));
                headUITransform.position = new Vector3((1 + offset.x) * scale, -4, (j + offset.y) * scale);
                headUITransform.rotation = playerTransform.rotation;
                break;
            }
        }

    }


	public bool CanPutExitHere (int i, int j) {
		if (height <= j) {
			return false;
		}

		if (Maze[i-1, j] == 0) {
			return true;
		}else {
			return false;
		}
	}

 
    // =======================================
    public int[,] CreateMaze() {
   
        //local variable to store neighbors to the current square as we work our way through the maze
            List<Vector2> neighbors;
            //as long as there are still tiles to try
            while (_tiletoTry.Count > 0)
            {
                //excavate the square we are on
                Maze[(int)CurrentTile.x, (int)CurrentTile.y] = 0;
                //get all valid neighbors for the new tile
                neighbors = GetValidNeighbors(CurrentTile);
                //if there are any interesting looking neighbors
                if (neighbors.Count > 0)
                {
                    //remember this tile, by putting it on the stack
                    _tiletoTry.Push(CurrentTile);
                    //move on to a random of the neighboring tiles
                    CurrentTile = neighbors[rnd.Next(neighbors.Count)];
                }
                else
                {
                    //if there were no neighbors to try, we are at a dead-end toss this tile out
                    //(thereby returning to a previous tile in the list to check).
                    CurrentTile = _tiletoTry.Pop();
                }
            }
            print("Maze Generated ...");
            return Maze;
        }
   
    // ================================================
        // Get all the prospective neighboring tiles "centerTile" The tile to test
        // All and any valid neighbors</returns>
        private List<Vector2> GetValidNeighbors(Vector2 centerTile) {
            List<Vector2> validNeighbors = new List<Vector2>();
            //Check all four directions around the tile
            foreach (var offset in offsets) {
                //find the neighbor's position
                Vector2 toCheck = new Vector2(centerTile.x + offset.x, centerTile.y + offset.y);
                //make sure the tile is not on both an even X-axis and an even Y-axis
                //to ensure we can get walls around all tunnels
                if (toCheck.x % 2 == 1 || toCheck.y % 2 == 1) {
               
                    //if the potential neighbor is unexcavated (==1)
                    //and still has three walls intact (new territory)
                    if (Maze[(int)toCheck.x, (int)toCheck.y]  == 1 && HasThreeWallsIntact(toCheck)) {
                   
                        //add the neighbor
                        validNeighbors.Add(toCheck);
                    }
                }
            }
            return validNeighbors;
        }
    // ================================================
        // Counts the number of intact walls around a tile
        //"Vector2ToCheck">The coordinates of the tile to check
        //Whether there are three intact walls (the tile has not been dug into earlier.
        private bool HasThreeWallsIntact(Vector2 Vector2ToCheck) {
       
            int intactWallCounter = 0;
            //Check all four directions around the tile
            foreach (var offset in offsets) {
           
                //find the neighbor's position
                Vector2 neighborToCheck = new Vector2(Vector2ToCheck.x + offset.x, Vector2ToCheck.y + offset.y);
                //make sure it is inside the maze, and it hasn't been dug out yet
                if (IsInside(neighborToCheck) && Maze[(int)neighborToCheck.x, (int)neighborToCheck.y] == 1) {
                    intactWallCounter++;
                }
            }
            //tell whether three walls are intact
            return intactWallCounter == 3;
        }
   
    // ================================================
        private bool IsInside(Vector2 p) {
            //return p.x >= 0  p.y >= 0  p.x < width  p.y < height;
           return p.x >= 0 && p.y >= 0 && p.x < width && p.y < height;
        }


         private int[,] maze1 = new int[25,25] {
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
{1,0,1,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,1,0,0,0,1},
{1,0,1,0,1,1,1,1,1,0,1,0,1,0,1,0,1,1,1,0,1,0,1,0,1},
{1,0,1,0,0,0,0,0,1,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,1},
{1,0,1,1,1,1,1,0,1,0,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1},
{1,0,0,0,0,0,0,0,1,0,1,0,0,0,1,0,0,0,1,0,0,0,0,0,1},
{1,1,1,1,1,1,1,1,1,0,1,1,1,0,1,1,1,0,1,0,1,1,1,1,1},
{1,0,0,0,0,0,1,0,0,0,0,0,1,0,1,0,0,0,1,0,1,0,0,0,1},
{1,0,1,1,1,1,1,0,1,0,1,0,1,1,1,0,1,1,1,0,1,0,1,0,1},
{1,0,0,0,0,0,0,0,1,0,1,0,0,0,1,0,1,0,0,0,1,0,1,0,1},
{1,0,1,1,1,1,1,1,1,1,1,1,1,0,1,0,1,0,1,1,1,0,1,1,1},
{1,0,0,0,1,0,0,0,0,0,1,0,1,0,0,0,1,0,0,0,1,0,0,0,1},
{1,1,1,0,1,1,1,0,1,0,1,0,1,1,1,1,1,1,1,0,1,1,1,0,1},
{1,0,0,0,1,0,0,0,1,0,1,0,1,0,0,0,0,0,1,0,0,0,0,0,1},
{1,0,1,1,1,0,1,1,1,0,1,0,1,0,1,1,1,0,1,1,1,1,1,1,1},
{1,0,1,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,1},
{1,0,1,1,1,1,1,0,1,1,1,0,1,1,1,1,1,0,1,0,1,1,1,0,1},
{1,0,0,0,1,0,0,0,1,0,1,0,1,0,0,0,1,0,1,0,0,0,1,0,1},
{1,1,1,0,1,0,1,1,1,0,1,0,1,0,1,0,1,0,1,1,1,0,1,0,1},
{1,0,0,0,1,0,0,0,0,0,1,0,1,0,1,0,1,0,0,0,0,0,1,0,1},
{1,0,1,0,1,1,1,1,1,1,1,0,1,0,1,1,1,0,1,1,1,1,1,0,1},
{1,0,1,0,0,0,1,0,0,0,1,0,1,0,0,0,1,0,1,0,0,0,0,0,1},
{1,0,1,1,1,1,1,0,1,0,1,0,1,0,1,0,1,1,1,0,1,1,1,1,1},
{1,0,0,0,0,0,0,0,1,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,1},
{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
    };
    private int[,] maze2 = new int[21,31] {
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1},
        {1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,1,1,1,0,0,1,0,0,1,0,0,1},
        {1,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,1,0,0,1},
        {1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,0,0,1,1,1,1,1,1,1,0,0,1},
        {1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,1},
        {1,1,1,1,0,0,1,0,0,1,0,0,1,1,1,1,1,1,1,0,0,1,1,1,1,0,0,1,0,0,1},
        {1,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,1},
        {1,0,0,1,0,0,1,1,1,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1},
        {1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1},
        {1,1,1,1,1,1,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,1,1,1,0,0,1,1,1,1},
        {1,0,0,1,0,0,0,0,0,1,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,1},
        {1,0,0,1,0,0,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,0,0,1,1,1,1,0,0,1},
        {1,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1},
        {1,0,0,1,0,0,1,1,1,1,0,0,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1},
        {1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1},
        {1,0,0,1,1,1,1,1,1,1,0,0,1,1,1,1,0,0,1,1,1,1,1,1,1,0,0,1,0,0,1},
        {1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,1},
        {1,0,0,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,0,0,1},
        {1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
    };

    private int[,] maze3 = new int[25,25] {
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,0,1,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,1,0,1},
        {1,0,1,0,1,0,1,0,1,0,1,0,1,1,1,1,1,0,1,1,1,0,1,0,1},
        {1,0,1,0,1,0,1,0,1,0,1,0,0,0,0,0,1,0,1,0,0,0,1,0,1},
        {1,0,1,1,1,0,1,0,1,0,1,1,1,1,1,1,1,0,1,0,1,1,1,0,1},
        {1,0,0,0,0,0,1,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,1},
        {1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,0,1,0,1,1,1,0,1,0,1},
        {1,0,0,0,1,0,0,0,1,0,1,0,0,0,1,0,1,0,1,0,0,0,1,0,1},
        {1,0,1,1,1,0,1,1,1,0,1,1,1,0,1,0,1,0,1,0,1,1,1,0,1},
        {1,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,1,0,1,0,1,0,0,0,1},
        {1,0,1,0,1,1,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,1,1},
        {1,0,0,0,1,0,0,0,1,0,0,0,1,0,1,0,1,0,0,0,1,0,0,0,1},
        {1,0,1,1,1,0,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,0,1},
        {1,0,0,0,1,0,0,0,1,0,0,0,0,0,1,0,1,0,1,0,0,0,1,0,1},
        {1,1,1,1,1,1,1,0,1,0,1,1,1,0,1,0,1,0,1,0,1,1,1,0,1},
        {1,0,0,0,0,0,0,0,1,0,0,0,1,0,1,0,1,0,1,0,0,0,1,0,1},
        {1,0,1,1,1,0,1,1,1,1,1,0,1,0,1,0,1,0,1,1,1,0,1,1,1},
        {1,0,0,0,1,0,1,0,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1},
        {1,1,1,0,1,0,1,1,1,0,1,1,1,1,1,1,1,0,1,0,1,1,1,0,1},
        {1,0,1,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,1,0,0,0,1,0,1},
        {1,0,1,0,1,1,1,1,1,1,1,0,1,1,1,0,1,0,1,1,1,0,1,0,1},
        {1,0,1,0,0,0,1,0,0,0,1,0,1,0,1,0,1,0,0,0,1,0,1,0,1},
        {1,0,1,1,1,0,1,0,1,0,1,0,1,0,1,0,1,1,1,0,1,1,1,0,1},
        {1,0,0,0,0,0,0,0,1,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
    };

    private int[,] maze4 = new int[25,25] {
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
{1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1},
{1,1,1,1,1,0,1,0,1,1,1,1,1,1,1,1,1,0,1,1,1,0,1,0,1},
{1,0,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,1},
{1,0,1,1,1,1,1,1,1,0,1,0,1,1,1,0,1,1,1,0,1,1,1,0,1},
{1,0,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,0,0,1},
{1,1,1,0,1,0,1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,1,1,1,1},
{1,0,1,0,1,0,0,0,0,0,1,0,1,0,1,0,0,0,1,0,0,0,1,0,1},
{1,0,1,0,1,1,1,1,1,1,1,0,1,0,1,0,1,1,1,0,1,0,1,0,1},
{1,0,1,0,0,0,0,0,0,0,1,0,1,0,0,0,1,0,0,0,1,0,0,0,1},
{1,0,1,0,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,0,1,1,1},
{1,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,1,0,1,0,1},
{1,0,1,1,1,0,1,1,1,0,1,1,1,0,1,0,1,1,1,1,1,0,1,0,1},
{1,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,1,0,0,0,1,0,1,0,1},
{1,1,1,0,1,1,1,0,1,1,1,1,1,1,1,0,1,0,1,0,1,0,1,0,1},
{1,0,1,0,1,0,1,0,0,0,0,0,1,0,0,0,1,0,1,0,1,0,1,0,1},
{1,0,1,0,1,0,1,1,1,1,1,1,1,0,1,1,1,0,1,0,1,0,1,0,1},
{1,0,0,0,1,0,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,1},
{1,0,1,1,1,1,1,0,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,0,1},
{1,0,1,0,0,0,1,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1},
{1,0,1,0,1,0,1,0,1,0,1,1,1,0,1,1,1,0,1,1,1,1,1,1,1},
{1,0,1,0,1,0,0,0,1,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,1},
{1,0,1,0,1,0,1,1,1,0,1,1,1,0,1,0,1,1,1,0,1,0,1,0,1},
{1,0,0,0,1,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,1},
{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
    };
    private int[,] maze5 = new int[25,25] {
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
{1,0,0,0,1,0,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0,1},
{1,1,1,0,1,1,1,0,1,0,1,0,1,1,1,0,1,0,1,0,1,0,1,0,1},
{1,0,1,0,0,0,1,0,1,0,1,0,0,0,0,0,1,0,0,0,1,0,0,0,1},
{1,0,1,1,1,0,1,1,1,0,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1},
{1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1},
{1,0,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1},
{1,0,1,0,0,0,0,0,1,0,1,0,0,0,1,0,0,0,1,0,1,0,0,0,1},
{1,0,1,1,1,1,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1},
{1,0,0,0,0,0,1,0,1,0,0,0,1,0,1,0,1,0,1,0,1,0,1,0,1},
{1,0,1,1,1,0,1,0,1,1,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1},
{1,0,0,0,1,0,0,0,1,0,0,0,1,0,1,0,1,0,0,0,0,0,1,0,1},
{1,1,1,0,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,0,1},
{1,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,1,0,1},
{1,0,1,1,1,0,1,1,1,0,1,1,1,1,1,0,1,1,1,0,1,0,1,0,1},
{1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,1,0,1,0,0,0,1},
{1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,0,1,0,1,0,1,1,1,1,1},
{1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,1,0,1},
{1,0,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,0,1,0,1},
{1,0,1,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,1,0,1,0,1,0,1},
{1,1,1,0,1,1,1,1,1,0,1,0,1,1,1,1,1,0,1,0,1,0,1,0,1},
{1,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,1,0,0,0,1,0,1},
{1,0,1,1,1,0,1,1,1,1,1,1,1,1,1,0,1,0,1,1,1,0,1,0,1},
{1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1},
{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
    };

}