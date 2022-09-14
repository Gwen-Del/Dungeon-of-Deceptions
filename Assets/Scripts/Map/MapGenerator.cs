using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    [System.Serializable]
    public class Rule
    {
        public GameObject room;
        public GameObject[] SpawnerRoom;
        public GameObject[] LootRoom;
        public GameObject[] FinaleRoom;
        public Vector2Int minPosition;
        public Vector2Int maxPosition;

        public bool obligatory;

        public int ProbabilityOfSpawning(int x, int y)
        {
            // 0 - cannot spawn 1 - can spawn 2 - HAS to spawn

            if (x>= minPosition.x && x<=maxPosition.x && y >= minPosition.y && y <= maxPosition.y)
            {
                return obligatory ? 2 : 1;
            }

            return 0;
        }

    }

    public Vector2Int size;
    public int startPos = 0;
    public Rule[] rooms;
    public Vector2 offset;

    List<Cell> board;

    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }

    void GenerateDungeon()
    {

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[(i + j * size.x)];
                if (currentCell.visited)
                {
                    int randomRoom = -1;
                    List<int> availableRooms = new List<int>();

                    for (int k = 0; k < rooms.Length; k++)
                    {
                        int p = rooms[k].ProbabilityOfSpawning(i, j);

                        if(p == 2)
                        {
                            randomRoom = k;
                            break;
                        } else if (p == 1)
                        {
                            availableRooms.Add(k);
                        }
                    }

                    if(randomRoom == -1)
                    {
                        if (availableRooms.Count > 0)
                        {
                            randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                        }
                        else
                        {
                            randomRoom = 0;
                        }
                    }


                    var newRoom = Instantiate(rooms[randomRoom].room, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    newRoom.UpdateRoom(currentCell.status);
                    newRoom.name += " " + i + "-" + j;

                }
            }
        }

    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos;

        Stack<int> path = new Stack<int>();

        int k = 0;

        while (k<1000)
        {
            k++;

            board[currentCell].visited = true;

            if(currentCell == board.Count - 1)
            {
                break;
            }

            //Check the cell's neighbors
            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    //down or right
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    //up or left
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }

            }

        }
        GenerateDungeon();
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        //check up neighbor
        if (cell - size.x >= 0 && !board[(cell-size.x)].visited)
        {
            neighbors.Add((cell - size.x));
        }

        //check down neighbor
        if (cell + size.x < board.Count && !board[(cell + size.x)].visited)
        {
            neighbors.Add((cell + size.x));
        }

        //check right neighbor
        if ((cell+1) % size.x != 0 && !board[(cell +1)].visited)
        {
            neighbors.Add((cell +1));
        }

        //check left neighbor
        if (cell % size.x != 0 && !board[(cell - 1)].visited)
        {
            neighbors.Add((cell -1));
        }

        return neighbors;
    }

    /*public GameObject spawnBasePrefab;
    public GameObject playerPrefab;
    public GameObject basicWallPrefab;

    public GameObject prefabVide;
    public GameObject[] prefabsMaze;
    [Range(0,30)]
    public int percentSpanwer = 30;
    public GameObject[] prefabsSpawner;
    [Range(0,30)]
    public int percentLoot = 30;
    public GameObject[] prefabsLoot;
    public GameObject[] prefabsFinale;

    [Range(1,20)]
    public int nbSalleMin = 5;

    GameObject[][] maps = new GameObject[10][];

    int BaseX;
    int BaseZ;

    void Start()
    {
        for(int i=0;i<10;i++)
        {
            maps[i] = new GameObject[10];
        }

        BaseX = Random.Range(-4,5);
        BaseZ = Random.Range(-4,5);

        maps[BaseX+4][BaseZ+4] = Instantiate(spawnBasePrefab, new Vector3(BaseX*20-10,0,BaseZ*20-10), Quaternion.identity);
        GameObject player = Instantiate(playerPrefab, new Vector3(BaseX*20-10,0,BaseZ*20-10), Quaternion.identity);

        PutWall(BaseX,BaseZ);

        int SalleX = BaseX;
        int SalleZ = BaseZ;

        nbSalleMin = nbSalleMin + Random.Range(0,20-nbSalleMin);
        for(int i=0;i<nbSalleMin;i++)
        {
            int nbIter = 0;
            int XorZ = Random.Range(0,3);
            if(!MapSelector(XorZ, nbIter))
            {
                i--;
            }
        }

        for(int i=0;i<10;i++)
        {
            for(int j=0;j<10;j++)
            {
                if(maps[i][j] == null)
                {
                    maps[i][j] = Instantiate(prefabVide, new Vector3((i-4)*20-10,0,(j-4)*20-10), Quaternion.identity);
                }
            }
        }
    }

    bool MapSelector(int XorZ, int nbIter)
    {
        if(nbIter < 4)
        {
             if(XorZ == 0)
            {
                if(BaseX+1 > 5 || maps[BaseX+5][BaseZ+4] != null)
                {
                    if(MapSelector(XorZ++, nbIter++))
                    {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    BaseX++;
                    SalleGenerator(BaseX, BaseZ);
                    return true;
                }
            } 
            else if (XorZ == 1)
            {
                if(BaseZ+1 > 5 || maps[BaseX+4][BaseZ+5] != null)
                {
                    if(MapSelector(XorZ++, nbIter++))
                    {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    BaseZ++;
                    SalleGenerator(BaseX, BaseZ);
                    return true;
                }
            }
            else if (XorZ == 2)
            {
                if(BaseX-1 < -4 || maps[BaseX+3][BaseZ+4] != null)
                {
                    if(MapSelector(XorZ++, nbIter++))
                    {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    BaseX--;
                    SalleGenerator(BaseX, BaseZ);
                    return true;
                }
            }
            else
            {
                if(BaseZ-1 < -4 || maps[BaseX+4][BaseZ+3] != null)
                {
                    if(MapSelector(XorZ++, nbIter++))
                    {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    BaseZ--;
                    SalleGenerator(BaseX, BaseZ);
                    return true;
                }
            }
        } else {
            return false;
        }
    }

    void SalleGenerator(int SalleX, int SalleZ)
    {
        int typeSalle = Random.Range(0,99);
        if(typeSalle < percentSpanwer){
            if(prefabsSpawner.Length-1 >= 0){
                maps[SalleX+4][SalleZ+4] = Instantiate(prefabsSpawner[Random.Range(0,prefabsSpawner.Length)], new Vector3(SalleX*20-10,0,SalleZ*20-10), Quaternion.identity);
            } else {
                maps[SalleX+4][SalleZ+4] = Instantiate(prefabsMaze[Random.Range(0,prefabsMaze.Length)], new Vector3(SalleX*20-10,0,SalleZ*20-10), Quaternion.identity);
            }
        }
        else if(typeSalle < percentSpanwer + percentLoot){
            if(prefabsLoot.Length-1 >= 0){
                maps[SalleX+4][SalleZ+4] = Instantiate(prefabsLoot[Random.Range(0,prefabsLoot.Length)], new Vector3(SalleX*20-10,0,SalleZ*20-10), Quaternion.identity);
            } else {
                maps[SalleX+4][SalleZ+4] = Instantiate(prefabsMaze[Random.Range(0,prefabsMaze.Length)], new Vector3(SalleX*20-10,0,SalleZ*20-10), Quaternion.identity);
            }
        } else {
            maps[SalleX+4][SalleZ+4] = Instantiate(prefabsMaze[Random.Range(0,prefabsMaze.Length)], new Vector3(SalleX*20-10,0,SalleZ*20-10), Quaternion.identity);
        }
        PutWall(SalleX,SalleZ);
    }

    void PutWall(int SalleX, int SalleZ)
    {
        if(SalleX == -4)
        {
            Instantiate(basicWallPrefab, new Vector3(SalleX*20-20,2,SalleZ*20-10), Quaternion.identity).transform.Rotate(0.0f,90.0f,0.0f);
        }
        else if(SalleX == 5)
        {
            Instantiate(basicWallPrefab, new Vector3(SalleX*20,2,SalleZ*20-10), Quaternion.identity).transform.Rotate(0.0f,90.0f,0.0f);
        }
        if(SalleZ == -4)
        {
            Instantiate(basicWallPrefab, new Vector3(SalleX*20-10,2,SalleZ*20-20), Quaternion.identity);
        }
        else if(SalleZ == 5)
        {
            Instantiate(basicWallPrefab, new Vector3(SalleX*20-10,2,SalleZ*20), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
