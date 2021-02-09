using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MiniGame : MonoBehaviour
{
    private System.Random rand = new System.Random();

    public Transform tilePrefab;
    public Vector2 mapSize;

    public Canvas mainCanvas;

    List<Tuple<int, int>> goldList = new List<Tuple<int, int>>();
    List<Tuple<int, int>> silverList = new List<Tuple<int, int>>();
    List<Tuple<int, int>> sphereList = new List<Tuple<int, int>>();

    [Range(0, 1)]
    public float outline;

    GameObject[,] grid = new GameObject[20, 20];
    GameObject[,] resources = new GameObject[20, 20];


    private void Start()
    {


        //GenerateGrid();
        //GenerateResource();

        GenerateMap();

        //for (int r = 0; r < 20; r++)
        //{
        //    for (int c = 0; c < 20; c++)
        //    {
        //        if (r % 2 == 0)
        //        {
        //            grid[r, c].SetActive(false);
        //        }
        //        //Debug.Log(grid[r, c].transform.position.x);
        //    }
        //}

        PlaceResources();

        GenerateResource();

        GameObject refTile = (GameObject)Instantiate(Resources.Load("Normal"));
        float sizeS = refTile.GetComponent<RectTransform>().rect.width;
        Destroy(refTile);

        float gridW = mapSize.x * sizeS;
        float gridH = mapSize.y * sizeS;

        transform.position = new Vector3(mainCanvas.GetComponent<RectTransform>().rect.width - (gridW / 2), mainCanvas.GetComponent<RectTransform>().rect.height - (gridH / 2), 0);

    }


    public void GenerateGrid()
    {
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + i, -mapSize.y / 2 + 0.5f + j, 0);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.one)) as Transform;
            }
        }
    }

    public void GenerateMap()
    {
        GameObject refTile = (GameObject)Instantiate(Resources.Load("Normal"));
        float size = refTile.GetComponent<RectTransform>().rect.width;

        for (int r = 0; r < mapSize.x; r++)
        {
            for (int c = 0; c < mapSize.y; c++)
            {
                GameObject tile = (GameObject)Instantiate(refTile, transform);
                //Debug.Log(tile.GetComponent<RectTransform>().rect.width);

                float posX = c * size * (1 + outline) + size / 2;
                float posY = -r * size * (-1 - outline) + size / 2;

                tile.transform.position = new Vector3(posX, posY, 0);
                //Debug.Log(posX);

                grid[r, c] = tile;
            }
        }

        Destroy(refTile);


    }

    public void GenerateResource()
    {
        GameObject refGold = (GameObject)Instantiate(Resources.Load("Gold"));
        GameObject refSilver = (GameObject)Instantiate(Resources.Load("Silver"));
        GameObject refSphere = (GameObject)Instantiate(Resources.Load("Sphere"));
        GameObject refNone = (GameObject)Instantiate(Resources.Load("None"));

        float size = refGold.GetComponent<RectTransform>().rect.width;

        for (int i = 0; i < goldList.Count; i++)
        {
            int r = goldList[i].Item1;
            int c = goldList[i].Item2;

            GameObject tile = (GameObject)Instantiate(refGold, transform);
            float posX = c * size * (1 + outline) + size / 2;
            float posY = -r * size * (-1 - outline) + size / 2;

            tile.transform.position = new Vector3(posX, posY, 0);

            resources[r, c] = tile;
        }



        for (int i = 0; i < silverList.Count; i++)
        {
            int r = silverList[i].Item1;
            int c = silverList[i].Item2;

            GameObject tile = (GameObject)Instantiate(refSilver, transform);
            float posX = c * size * (1 + outline) + size / 2;
            float posY = -r * size * (-1 - outline) + size / 2;

            tile.transform.position = new Vector3(posX, posY, 0);

            resources[r, c] = tile;
        }


        for (int i = 0; i < sphereList.Count; i++)
        {
            int r = sphereList[i].Item1;
            int c = sphereList[i].Item2;

            GameObject tile = (GameObject)Instantiate(refSphere, transform);
            float posX = c * size * (1 + outline) + size / 2;
            float posY = -r * size * (-1 - outline) + size / 2;

            tile.transform.position = new Vector3(posX, posY, 0);

            resources[r, c] = tile;
        }


        for (int r = 0; r < mapSize.x; r++)
        {
            for (int c = 0; c < mapSize.y; c++)
            {
                if (resources[r, c] == null)
                {
                    GameObject tile = (GameObject)Instantiate(refNone, transform);

                    float posX = c * size * (1 + outline) + size / 2;
                    float posY = -r * size * (-1 - outline) + size / 2;

                    tile.transform.position = new Vector3(posX, posY, 0);

                    resources[r, c] = tile;
                }

                resources[r, c].SetActive(false);

            }
        }



    }

    public void PlaceResources()
    {
        for (int i = 0; i < 5; i++)
        {
            //int x = UnityEngine.Random.Range(2, 18);
            //int y = UnityEngine.Random.Range(2, 18);

            //Debug.Log(centralList.Count);
            //Debug.Log(x + "-" + y);

            //centralList.Add(new Tuple<int, int>(x, y));

            RandomizeCentralResource();
            Debug.Log(goldList[i]);

            int goldRowPos = goldList[i].Item1;
            int goldColPos = goldList[i].Item2;


            silverList.Add(new Tuple<int, int>(goldRowPos, goldColPos - 1));
            silverList.Add(new Tuple<int, int>(goldRowPos, goldColPos + 1));
            silverList.Add(new Tuple<int, int>(goldRowPos - 1, goldColPos));
            silverList.Add(new Tuple<int, int>(goldRowPos + 1, goldColPos));
            silverList.Add(new Tuple<int, int>(goldRowPos - 1, goldColPos - 1));
            silverList.Add(new Tuple<int, int>(goldRowPos - 1, goldColPos + 1));
            silverList.Add(new Tuple<int, int>(goldRowPos + 1, goldColPos - 1));
            silverList.Add(new Tuple<int, int>(goldRowPos + 1, goldColPos + 1));


            sphereList.Add(new Tuple<int, int>(goldRowPos, goldColPos - 2));
            sphereList.Add(new Tuple<int, int>(goldRowPos, goldColPos + 2));
            sphereList.Add(new Tuple<int, int>(goldRowPos - 2, goldColPos));
            sphereList.Add(new Tuple<int, int>(goldRowPos + 2, goldColPos));
            sphereList.Add(new Tuple<int, int>(goldRowPos - 2, goldColPos - 2));
            sphereList.Add(new Tuple<int, int>(goldRowPos - 2, goldColPos + 2));
            sphereList.Add(new Tuple<int, int>(goldRowPos + 2, goldColPos - 2));
            sphereList.Add(new Tuple<int, int>(goldRowPos + 2, goldColPos + 2));
            sphereList.Add(new Tuple<int, int>(goldRowPos - 1, goldColPos - 2));
            sphereList.Add(new Tuple<int, int>(goldRowPos - 1, goldColPos + 2));
            sphereList.Add(new Tuple<int, int>(goldRowPos + 1, goldColPos - 2));
            sphereList.Add(new Tuple<int, int>(goldRowPos + 1, goldColPos + 2));
            sphereList.Add(new Tuple<int, int>(goldRowPos - 2, goldColPos - 1));
            sphereList.Add(new Tuple<int, int>(goldRowPos - 2, goldColPos + 1));
            sphereList.Add(new Tuple<int, int>(goldRowPos + 2, goldColPos - 1));
            sphereList.Add(new Tuple<int, int>(goldRowPos + 2, goldColPos + 1));
        }

    }

    public void RandomizeCentralResource()
    {
        int x = UnityEngine.Random.Range(2, 18);
        int y = UnityEngine.Random.Range(2, 18);

        if (goldList.Count > 0)
        {
            bool placeable = true;

            for (int i = 0; i < goldList.Count; i++)
            {
                if (x > goldList[i].Item1 - 6 && x < goldList[i].Item1 + 6)
                {
                    if (y > goldList[i].Item2 - 6 && y < goldList[i].Item2 + 6)
                    {
                        placeable = false;
                        break;
                    }
                }
            }

            if (placeable == true)
            {
                goldList.Add(new Tuple<int, int>(x, y));
            }
            else
            {
                Debug.Log("Ra");
                RandomizeCentralResource();
            }
        }
        else
        {
            goldList.Add(new Tuple<int, int>(x, y));
        }
    }
}
