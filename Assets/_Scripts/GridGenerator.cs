using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GridGenerator : MonoBehaviour
{
    private Vector3 defaultTransform;
    private Vector2 mapSize;

    public bool scanMode;
    public bool extractMode;

    [Range(0,1)]
    public float outline;

    public GameObject[,] grid      = new GameObject[20, 20];
    public GameObject[,] resources = new GameObject[20, 20];

    List<Tuple<int, int>> goldList = new List<Tuple<int, int>>();
    List<Tuple<int, int>> silverList = new List<Tuple<int, int>>();
    List<Tuple<int, int>> sphereList = new List<Tuple<int, int>>();

    [Header("Statistics")]
    public int totalResources   = 0;
    public int scanTimes        = 0;
    public int extractTimes     = 0;


    [Header("UI References")]
    public Canvas mainCanvas;
    public TextMeshProUGUI resourcesAmount;
    public TextMeshProUGUI scanAmount;
    public TextMeshProUGUI extractAmount;
    public TextMeshProUGUI currentMode;
    public TextMeshProUGUI message;
    public GameObject      actionPanel;

    [Header("Sprite References")]
    public Sprite goldSprite;
    public Sprite silverSprite;
    public Sprite sphereSprite;
    public Sprite noneSprite;


    private void Start()
    {
        mapSize.x = mapSize.y = 20;

        defaultTransform = transform.position;

        ResetGame();
    }


    private void Update()
    {
        // Setting Game Over state
        if(extractTimes >= 3)
        {
            scanMode    = false;
            extractMode = false;

            actionPanel.SetActive(false);
            message.text = "Your total resources are: " + totalResources + "\n\nPress 'Restart' Button to play again!";
        }


        // Update UI elements
        resourcesAmount.text    = totalResources.ToString();
        scanAmount.text         = scanTimes.ToString() + " / 6";
        extractAmount.text      = extractTimes.ToString() + " / 3";

        if(scanMode == true)
        {
            currentMode.text = "SCAN";
        }
        else if(extractMode == true)
        {
            currentMode.text = "EXTRACT";
        }
    }


    // Reset all values and Set up a new game
    public void ResetGame()
    {
        transform.position = defaultTransform;

        scanMode    = false;
        extractMode = true;

        actionPanel.SetActive(true);
        message.text = "";

        totalResources = 0;
        scanTimes = 0;
        extractTimes = 0;

        goldList    = new List<Tuple<int, int>>();
        silverList  = new List<Tuple<int, int>>();
        sphereList  = new List<Tuple<int, int>>();

        // Clear old grid and resource
        for (int r = 0; r < 20; r++)
        {
            for (int c = 0; c < 20; c++)
            {
                if (grid[r, c] != null)
                {
                    Destroy(grid[r, c]);
                }

                if (resources[r, c] != null)
                {
                    Destroy(resources[r, c]);
                }
            }
        }


        GenerateMap();


        PlaceResources();


        GenerateResource();


        // Set grid transform to be at center
        GameObject refTile = (GameObject)Instantiate(Resources.Load("Normal"));
        float sizeS = refTile.GetComponent<RectTransform>().rect.width;
        Destroy(refTile);

        float gridW = mapSize.x * sizeS;
        float gridH = mapSize.y * sizeS;

        transform.position = new Vector3(mainCanvas.GetComponent<RectTransform>().rect.width - (gridW / 2), mainCanvas.GetComponent<RectTransform>().rect.height - (gridH / 2), 0);
    }


    // Update when clicking Scan Button
    public void OnClickedScan()
    {
        scanMode    = true;
        extractMode = false;

        currentMode.text = "SCAN";
    }


    // Update when clicking Extract Button
    public void OnClickedExtract()
    {
        scanMode = false;
        extractMode = true;

        currentMode.text = "EXTRACT";
    }


    // Generate cover grid
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

                float posX = c * size * (1 + outline) + size/2;
                float posY = -r * size * (-1 - outline) + size/2;

                tile.transform.position = new Vector3(posX, posY, 0);
                //Debug.Log(posX);

                grid[r, c] = tile;
                grid[r, c].GetComponent<TileScript>().SetGridIndices(r, c);
            }
        }

        Destroy(refTile);

        
    }


    // Generate Resources Grid
    public void GenerateResource()
    {
        GameObject refGold      = (GameObject)Instantiate(Resources.Load("Gold"));
        GameObject refSilver    = (GameObject)Instantiate(Resources.Load("Silver"));
        GameObject refSphere    = (GameObject)Instantiate(Resources.Load("Sphere"));
        GameObject refNone      = (GameObject)Instantiate(Resources.Load("None"));

        float size = refGold.GetComponent<RectTransform>().rect.width;


        // Set None Resource
        for (int r = 0; r < mapSize.x; r++)
        {
            for (int c = 0; c < mapSize.y; c++)
            {
                    GameObject tile = (GameObject)Instantiate(refNone, transform);

                    float posX = c * size * (1 + outline) + size / 2;
                    float posY = -r * size * (-1 - outline) + size / 2;

                    tile.transform.position = new Vector3(posX, posY, 0);

                    resources[r, c] = tile;

                    resources[r, c].GetComponent<TileScript>().SetGridIndices(r, c);
                    resources[r, c].SetActive(false);

            }
        }


        // Set Gold Resource
        for (int i = 0; i < goldList.Count; i++)
        {
            int r = goldList[i].Item1;
            int c = goldList[i].Item2;

            GameObject tile = (GameObject)Instantiate(refGold, transform);
            float posX = c * size * (1 + outline) + size / 2;
            float posY = -r * size * (-1 - outline) + size / 2;

            tile.transform.position = new Vector3(posX, posY, 0);

            resources[r, c] = tile;

            resources[r, c].GetComponent<TileScript>().SetGridIndices(r, c);
            resources[r, c].SetActive(false);
        }


        // Set Silver Resource
        for(int i = 0; i < silverList.Count; i++)
        {
            int r = silverList[i].Item1;
            int c = silverList[i].Item2;

            GameObject tile = (GameObject)Instantiate(refSilver, transform);
            float posX = c * size * (1 + outline) + size / 2;
            float posY = -r * size * (-1 - outline) + size / 2;

            tile.transform.position = new Vector3(posX, posY, 0);

            resources[r, c] = tile;

            resources[r, c].GetComponent<TileScript>().SetGridIndices(r, c);
            resources[r, c].SetActive(false);
        }


        // Set Sphere Resource
        for (int i = 0; i < sphereList.Count; i++)
        {
            int r = sphereList[i].Item1;
            int c = sphereList[i].Item2;

            GameObject tile = (GameObject)Instantiate(refSphere, transform);
            float posX = c * size * (1 + outline) + size / 2;
            float posY = -r * size * (-1 - outline) + size / 2;

            tile.transform.position = new Vector3(posX, posY, 0);

            resources[r, c] = tile;

            resources[r, c].GetComponent<TileScript>().SetGridIndices(r, c);
            resources[r, c].SetActive(false);
        }


        Destroy(refGold);
        Destroy(refSilver);
        Destroy(refSphere);
        Destroy(refNone);
    }

    public void PlaceResources()
    {
        for(int i = 0; i < 5; i ++)
        {
            RandomizeCentralResource();
            Debug.Log(goldList[i]);

            int goldRowPos = goldList[i].Item1;
            int goldColPos = goldList[i].Item2;

            
            // Place Silver Resoucres around Gold Resources
            silverList.Add(new Tuple<int, int>(goldRowPos, goldColPos - 1));
            silverList.Add(new Tuple<int, int>(goldRowPos, goldColPos + 1));
            silverList.Add(new Tuple<int, int>(goldRowPos - 1, goldColPos));
            silverList.Add(new Tuple<int, int>(goldRowPos + 1, goldColPos));
            silverList.Add(new Tuple<int, int>(goldRowPos - 1, goldColPos - 1));
            silverList.Add(new Tuple<int, int>(goldRowPos - 1, goldColPos + 1));
            silverList.Add(new Tuple<int, int>(goldRowPos + 1, goldColPos - 1));
            silverList.Add(new Tuple<int, int>(goldRowPos + 1, goldColPos + 1));


            // Place Sphere Resoucres around Silver Resources
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


    // Randomize positons of Gold Resources
    public void RandomizeCentralResource()
    {
        int x = UnityEngine.Random.Range(2, 18);
        int y = UnityEngine.Random.Range(2, 18);

        if(goldList.Count > 0)
        {
            bool placeable = true;

            for(int i = 0; i < goldList.Count; i++)
            {
                if(x > goldList[i].Item1 - 6 && x < goldList[i].Item1 + 6)
                {
                    if( y > goldList[i].Item2 - 6 && y < goldList[i].Item2 + 6)
                    {
                        placeable = false;
                        break;
                    }
                }
            }

            if(placeable == true)
            {
                goldList.Add(new Tuple<int, int>(x, y));
            } 
            else
            {
                RandomizeCentralResource();
            }
        }
        else
        {
            goldList.Add(new Tuple<int, int>(x, y));
        }
    }
}
