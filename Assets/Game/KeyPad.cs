using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public class KeyPad : MonoBehaviour
    {
        private bool _run;

        private Tilemap _tilemap;
        private Vector3Int _tileMain;
        private Vector3Int[] _adjacentTiles;

        private void Start()
        {
            _tilemap = transform.parent.GetComponent<Tilemap>();
            
            _tileMain = _tilemap.WorldToCell(transform.position);
         
            _adjacentTiles = new []
            {
                new Vector3Int(_tileMain.x,_tileMain.y+1,_tileMain.z),
                new Vector3Int(_tileMain.x,_tileMain.y+2,_tileMain.z),
                new Vector3Int(_tileMain.x,_tileMain.y-1,_tileMain.z),
                _tileMain
            };
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            if (PlayerController.Keys > 0 && _run == false)
            {
                PlayerController.Keys--;
                _run = true;
                Invoke(nameof(DestroyTiles),.1f);
                print("opa: "+ other.tag+" keys: "+PlayerController.Keys);
            }

            if (_run)
            {
                gameObject.SetActive(false);
            }
        }

        private void DestroyTiles()
        {
            _tilemap.SetTiles(_adjacentTiles, new TileBase[_adjacentTiles.Length]);
        }
    }
}
