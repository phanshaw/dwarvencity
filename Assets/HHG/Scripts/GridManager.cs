using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int sizeX = 10;
    public int sizeY = 10;

    public GameObject gridPiece;
    private GameObject[] _gamePieceInstances;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeMap()
    {
    }

    void ClearMap()
    {
        if(_gamePieceInstances == null)
            return;

        for (int i = 0; i < sizeX * sizeY; i++)
        {
        }
    }
}
