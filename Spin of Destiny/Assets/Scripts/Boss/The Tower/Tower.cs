using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tower : Tarot
{
    private GameObject Grid;
    private GameObject newMap;

    private Tilemap towerMap;

    public Tile obstacle;
    public Vector3Int pos;

    private int maxX = 16, maxY = 9;


    [Header("Obstacle Generation")]
    private int nbObstacles = 8;
    private float intervalElapsed = 0f;
    private float interval = 0.5f;
    private Vector3Int lastObstPos = Vector3Int.zero;

    [Header("Player")]
    private GameObject Player;
    private Vector3Int playerPos;

    // Start is called before the first frame update
    void Start()
    {
        Grid = GameObject.Find("Grid");
        newMap =  (GameObject)Resources.Load("TowerMap", typeof(GameObject));
        GameObject temp = Instantiate(newMap,Grid.transform);
        towerMap = temp.GetComponent<Tilemap>();

        Player = GameObject.Find("Player");


    }

    // Update is called once per frame
    void Update()
    {

        setPlayerPos();

        if (nbObstacles > 0)
        {
            intervalElapsed += Time.deltaTime;

            if (intervalElapsed > interval) { 
                obstaclesGeneration();
                nbObstacles--;
                intervalElapsed = 0f;
            }
        }





    }

    private void obstaclesGeneration()
    {
        Vector3Int spawnPos = Vector3Int.zero;
        int spawnZone;
        

        
            int temp = 0;
            int i = Mathf.Abs(nbObstacles - 8);

            if (i < 3)
            {
                spawnZone = i + 1;
            }
            else
            {
                spawnZone = Random.Range(1, 4);
            }


            do { 
                spawnPos = generateSpawnPos(spawnZone);
                Debug.Log("zone = "+spawnZone+" Pos = "+spawnPos);
                temp++;
            }
            while (spawnPos == playerPos && temp < 200 && Vector3Int.Distance(lastObstPos, spawnPos) < 5);
            
            lastObstPos = spawnPos;
            pos = spawnPos;


            int chooseObstacle = Random.Range(0, 5);
            chooseSpawnObstacle(chooseObstacle);

        

    }
    

    private Vector3Int generateSpawnPos(int zone)
    {
        switch (zone)
        {
            case 1:
                return new Vector3Int(Random.Range(-15, -8), Random.Range(-9, 9));
                
            case 2:
                return  new Vector3Int(Random.Range(-8, 5), Random.Range(-9, 2));
                
            case 3:
                return new Vector3Int(Random.Range(5, 16), Random.Range(-9, 9));

            default:
                return Vector3Int.zero;
                
        }

    }

    private void chooseSpawnObstacle(int choice)
    {

        switch (choice)
        {
            case 0:
                spawnCross();
                break;
            case 1:
                spawnVoidCross();
                break;
            case 2:
                spawnSquare();
                break;
            case 3:
                int dir = Random.Range(0, 4);
                spawnDiagonal(dir);

                break;
            case 4:
                int dir1 = Random.Range(0, 2);
                spawnY(dir1);
                break;
        }
    }

    private void setPlayerPos()
    {
        int x = (int)Player.transform.position.x;
        int y = (int)Player.transform.position.y;


        Vector3Int temp = new Vector3Int(x,y,0);
        playerPos = temp;

    }

    private void spawnCross()
    {
        Vector3Int tempPos;
        for (int i = pos.x - 2; i <= pos.x + 2; i++) {

            if (i != pos.x) { 
                tempPos = new Vector3Int(i, pos.y);
                towerMap.SetTile(tempPos, obstacle);
            }
        }

        for (int i = pos.y - 2; i <= pos.y + 2; i++)
        {
            if (i != pos.y) { 
                tempPos = new Vector3Int(pos.x, i);
                towerMap.SetTile(tempPos, obstacle);
            }
        }
    }

    private void spawnVoidCross()
    {
        Vector3Int tempPos;
        for (int i = pos.x - 2; i <= pos.x + 2; i += 2)
        {
            if (i != pos.x) { 

                tempPos = new Vector3Int(i, pos.y);
                towerMap.SetTile(tempPos, obstacle);
            }
        }

        for (int i = pos.y - 2; i <= pos.y + 2; i += 2)
        {
            if (i != pos.y){

                tempPos = new Vector3Int(pos.x, i);
                towerMap.SetTile(tempPos, obstacle);
            }
        }
    }


    private void spawnDiagonal(int dir)
    {
        Vector3Int tempPos;
        int i = pos.x, j = pos.y;

        switch (dir)
        {
            case 0:

                for (i -= 1; i <= pos.x + 3; i++)
                {
                    if (i != pos.x)
                    {
                        tempPos = new Vector3Int(i, j);
                        towerMap.SetTile(tempPos, obstacle);
                    }

                    j++;
                }

                break;
            case 1:

                for (i -= 1; i <= pos.x + 3; i++)
                {
                    if (i != pos.x)
                    {
                        tempPos = new Vector3Int(i, j);
                        towerMap.SetTile(tempPos, obstacle);
                    }

                    j--;
                }

                break;
            case 2:

                for (i += 1; i >= pos.x - 3; i--)
                {
                    if (i != pos.x)
                    {
                        tempPos = new Vector3Int(i, j);
                        towerMap.SetTile(tempPos, obstacle);
                    }

                    j--;
                }

                break;
            case 3:

                for (i += 1; i >= pos.x - 3; i--)
                {
                    if (i != pos.x)
                    {
                        tempPos = new Vector3Int(i, j);
                        towerMap.SetTile(tempPos, obstacle);
                    }

                    j++;
                }

                break;

              
        }

        

    }


    private void spawnY(int dir)
    {
        int  a = 1;

        if (dir != 0) {
            a = -1;
        }

        Vector3Int tempPos;

        tempPos = new Vector3Int(pos.x-1 * a ,pos.y +1 );

        towerMap.SetTile(tempPos, obstacle);

        towerMap.SetTile(pos, obstacle);

        tempPos = new Vector3Int(pos.x + 3 * a, pos.y);
        towerMap.SetTile(tempPos, obstacle);
        tempPos = new Vector3Int(pos.x + 4 * a, pos.y);
        towerMap.SetTile(tempPos, obstacle);

        tempPos = new Vector3Int(pos.x, pos.y - 3);
        towerMap.SetTile(tempPos, obstacle);
        tempPos = new Vector3Int(pos.x, pos.y - 4);
        towerMap.SetTile(tempPos, obstacle);

    }


    private void spawnSquare()
    {
        Vector3Int tempPos;

        for (int i = pos.y + 2; i >= pos.y - 2; i--)
        {
            if (i == pos.y + 2 || i == pos.y - 2) {

                for (int j = pos.x - 1; j <= pos.x + 1; j++)
                {
                    if (i != pos.y && j != pos.x)
                    {
                        tempPos = new Vector3Int(i, j);
                        towerMap.SetTile(tempPos, obstacle);
                    }
                }
            }
            else { 

                for (int j = pos.x - 2; j <= pos.x + 2; j++)
                {
                    if (i != pos.y && j != pos.x)
                    {
                        tempPos = new Vector3Int(i, j);
                        towerMap.SetTile(tempPos, obstacle);
                    }
                } 
            }


        }

    }




}
