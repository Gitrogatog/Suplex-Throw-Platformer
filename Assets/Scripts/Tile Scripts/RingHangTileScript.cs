using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RingHangTileScript : MonoBehaviour
{
    private RingHangTileScript ringScript;
    private Tilemap map;
    private Vector3 hangingTile = Vector3.zero;
    private bool isPlayerHanging = false;

    public int xRange; //Actual Range = 2 * xRange + 1
    public int yRange; //Actual Range = 2 * yRange + 1
    public float playerOffsetX;
    public float playerOffsetY;
    private Vector3 playerOffsetVect;

    public CharacterController2D charController;
    // Start is called before the first frame update
    void Start()
    {
        playerOffsetVect = new Vector3(playerOffsetX, playerOffsetY, 0);
        map = GetComponent<Tilemap>();
        ringScript = GetComponent<RingHangTileScript>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Epic");
        if (other.tag == "Player")
        {
            //Debug.Log("Win");
            
            Vector3 playerPos = other.gameObject.transform.position;
            Vector3Int playerTilePos = map.WorldToCell(playerPos);
            //Vector3Int epic = new Vector3Int(1, 1, 0);
            /*
            Vector3Int tileMin = playerTilePos - epic;
            Vector3Int tileMax = playerTilePos + epic + epic;
            List<Vector3Int> tilePosList = new List<Vector3Int>();
            */
            //public BoundsInt(int xMin, int yMin, int zMin, int sizeX, int sizeY, int sizeZ)

            /*
            BoundsInt bounds = new BoundsInt();
            bounds.SetMinMax(tileMin, tileMax);
            Debug.Log("Current Bounds Size" + bounds.size.x + " " + bounds.size.y);
            */

            //TileBase[] tileArray = map.GetTilesBlock(bounds);
            int bestX = 0;
            int bestY = 0;
            bool isFirst = true;
            Vector3 bestPos = Vector3.zero;
            for (int x = -1 * xRange; x <= xRange; x++)
            {
                for(int y = -1 * yRange; y <= yRange; y++)
                {
                    Vector3Int posOffset = new Vector3Int(x, y, 0);
                    //if (tileArray[i] != null)
                    if (map.GetTile(playerTilePos + posOffset) != null)
                    {
                        Vector3 currentWorldPos = map.CellToWorld(playerTilePos + posOffset);
                        if (!isPlayerHanging || (isPlayerHanging && hangingTile != currentWorldPos))
                        {
                            if (isFirst)
                            {
                                bestX = x;
                                bestY = y;
                                bestPos = currentWorldPos;
                                isFirst = false;
                            }
                            else if (Vector3.Distance(bestPos, playerPos) <= Vector3.Distance(currentWorldPos, playerPos))
                            {
                                bestX = x;
                                bestY = y;
                                bestPos = currentWorldPos;
                            }
                        }
                        else
                        {
                            Debug.Log("Repeated Tile");
                        }
                    }
                }
            }
            if(!isFirst)
            {
                isPlayerHanging = true;
                hangingTile = map.WorldToCell(bestPos);
                Debug.Log("Got Tile At X: " + bestX + " Y: " + bestY);
                charController.StartHang(hangingTile + playerOffsetVect, ringScript);
            }
            else
            {
                Debug.Log("No Tiles In Range");
            }
            
            /*
            for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    Vector3Int checkVec = new Vector3Int(playerTilePos.x + x, playerTilePos.y + y, playerTilePos.z);
                    if (map.GetTile(map.WorldToCell(checkVec)) != null)
                    {

                    }
                }
            }
            */
        }
    }

    public void UnhangPlayer(Vector3 playerPos)
    {
        Debug.Log("UnhangPlayer Called");
        if(isPlayerHanging && hangingTile + playerOffsetVect == playerPos)
        {
            isPlayerHanging = false;
            Debug.Log("Successful Unhanging");
        }
    }
}
