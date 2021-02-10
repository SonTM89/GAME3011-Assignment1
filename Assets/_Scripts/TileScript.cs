using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileScript : MonoBehaviour, IPointerClickHandler
{
    public int row, col;

    private GridGenerator gameGrid;


    // Start is called before the first frame update
    void Start()
    {
        gameGrid = transform.parent.GetComponent<GridGenerator>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetGridIndices(int r, int c)
    {
        row = r;
        col = c;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        // Process clicking behaviour in Scan Mode
        if(gameGrid.scanMode == true && gameGrid.scanTimes < 6)
        {
            if (gameObject.tag == "Normal")
            {
                gameGrid.message.text = "You've just scanned!";

                gameGrid.scanTimes += 1;
                
                gameGrid.resources[row, col].SetActive(true);

                if (col - 1 >= 0)
                {
                    gameGrid.resources[row, col - 1].SetActive(true);
                }

                if (col + 1 < 20)
                {
                    gameGrid.resources[row, col + 1].SetActive(true);
                }

                if (row - 1 >= 0)
                {
                    gameGrid.resources[row - 1, col].SetActive(true);
                }

                if (row + 1 < 20)
                {
                    gameGrid.resources[row + 1, col].SetActive(true);                    
                }

                if (row - 1 >= 0 && col - 1 >= 0)
                {
                    gameGrid.resources[row - 1, col - 1].SetActive(true);                   
                }

                if (row - 1 >= 0 && col + 1 < 20)
                {
                    gameGrid.resources[row - 1, col + 1].SetActive(true);                   
                }

                if (row + 1 < 20 && col - 1 >= 0)
                {
                    gameGrid.resources[row + 1, col - 1].SetActive(true);                   
                }

                if (row + 1 < 20 && col + 1 < 20)
                {
                    gameGrid.resources[row + 1, col + 1].SetActive(true);
                }                
            }          
        }


        // Process clicking behaviour in Extract Mode
        if(gameGrid.extractMode == true && gameGrid.extractTimes < 3)
        {
            gameGrid.message.text = "You've just extracted!";

            gameGrid.extractTimes += 1;

            // Collect resources of clicked tile
            switch(gameGrid.resources[row, col].tag)
            {
                case "Gold":
                    gameGrid.totalResources += 4;
                    break;
                case "Silver":
                    gameGrid.totalResources += 2;
                    break;
                case "Sphere":
                    gameGrid.totalResources += 1;
                    break;
                default:
                    break;
            }

            // Change resource of clicked tile
            gameGrid.resources[row, col].tag = "None";
            gameGrid.resources[row, col].GetComponent<Image>().sprite = gameGrid.noneSprite;

            gameGrid.grid[row, col].SetActive(false);
            gameGrid.resources[row, col].SetActive(true);


            // Change resources of First Ring
            if (col - 1 >= 0)
            {
                ChangeResource(row, col - 1);
            }

            if (col + 1 < 20)
            {
                ChangeResource(row, col + 1);
            }

            if (row - 1 >= 0)
            {
                ChangeResource(row - 1, col);
            }

            if (row + 1 < 20)
            {
                ChangeResource(row + 1, col);
            }

            if (row - 1 >= 0 && col - 1 >= 0)
            {
                ChangeResource(row - 1, col - 1);
            }

            if (row - 1 >= 0 && col + 1 < 20)
            {
                ChangeResource(row - 1, col + 1);
            }

            if (row + 1 < 20 && col - 1 >= 0)
            {
                ChangeResource(row + 1, col - 1);
            }

            if (row + 1 < 20 && col + 1 < 20)
            {
                ChangeResource(row + 1, col + 1);
            }


            // Change resources of Second Ring
            if (col - 2 >= 0)
            {
                ChangeResource(row, col - 2);
            }

            if(col + 2 < 20)
            {
                ChangeResource(row, col + 2);
            }
            
            if(row - 2 >= 0)
            {
                ChangeResource(row - 2, col);
            }

            if (row + 2 < 20)
            {
                ChangeResource(row + 2, col);
            }

            if (row - 2 >= 0 && col - 2 >= 0)
            {
                ChangeResource(row - 2, col - 2);            
            }

            if (row - 2 >= 0 && col + 2 < 20)
            {
                ChangeResource(row - 2, col + 2);       
            }

            if (row + 2 < 20 && col - 2 >= 0)
            {
                ChangeResource(row + 2, col - 2);       
            }

            if (row + 2 < 20 && col + 2 < 20)
            {
                ChangeResource(row + 2, col + 2);         
            }

            if (row - 1 >= 0 && col - 2 >= 0)
            {
                ChangeResource(row - 1, col - 2);         
            }

            if (row - 1 >= 0 && col + 2 < 20)
            {
                ChangeResource(row - 1, col + 2);         
            }

            if (row + 1 < 20 && col - 2 >= 0)
            {
                ChangeResource(row + 1, col - 2);      
            }

            if (row + 1 < 20 && col + 2 < 20)
            {
                ChangeResource(row + 1, col + 2);        
            }

            if (row - 2 >= 0 && col - 1 >= 0)
            {
                ChangeResource(row - 2, col - 1);          
            }

            if (row - 2 >= 0 && col + 1 < 20)
            {
                ChangeResource(row - 2, col + 1);         
            }

            if (row + 2 < 20 && col - 1 >= 0)
            {
                ChangeResource(row + 2, col - 1);            
            }

            if (row + 2 < 20 && col + 1 < 20)
            {
                ChangeResource(row + 2, col + 1);
            }           
        }
    }


    public void ChangeResource(int row, int col)
    {
        switch (gameGrid.resources[row, col].tag)
        {
            case "Gold":
                gameGrid.resources[row, col].tag = "Silver";
                gameGrid.resources[row, col].GetComponent<Image>().sprite = gameGrid.silverSprite;
                break;
            case "Silver":
                gameGrid.resources[row, col].tag = "Sphere";
                gameGrid.resources[row, col].GetComponent<Image>().sprite = gameGrid.sphereSprite;
                break;
            case "Sphere":
                gameGrid.resources[row, col].tag = "None";
                gameGrid.resources[row, col].GetComponent<Image>().sprite = gameGrid.noneSprite;
                break;
            default:
                break;
        }
    }

}
