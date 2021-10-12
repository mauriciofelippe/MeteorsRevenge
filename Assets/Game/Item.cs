using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Game
{
    public class Item : MonoBehaviour
    {

        private bool _run;
        private Tilemap tilemap;
        private Vector3Int tileMain;
        public Vector3Int[] adjacentTiles;
        private Vector3Int[] _adjacentTiles;

        public ItemType itemType = ItemType.Key;
        
        public UnityEvent onPlayerTouch;

        public bool destroyTiles = true;
            
            
            private void Start()
        {
            tilemap = transform.parent.GetComponent<Tilemap>();
            tileMain = tilemap.WorldToCell(transform.position);

            _adjacentTiles = new Vector3Int[adjacentTiles.Length];

            for (var i = 0; i < adjacentTiles.Length; i++)
            {
                _adjacentTiles[i] = new Vector3Int(
                    tileMain.x + adjacentTiles[i].x,
                    tileMain.y + adjacentTiles[i].y,
                    tileMain.z + adjacentTiles[i].z);
                
            }
            
            GameManager.instance.itemManager.AddEvent(onPlayerTouch,itemType);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
         
            
            if (_run) return;
            _run = true;
            
            onPlayerTouch.Invoke();
            Invoke(nameof(DestroyTiles),0.1f);
        }
        
        private void DestroyTiles()
        {
            if(destroyTiles)
                tilemap.SetTiles(_adjacentTiles, new TileBase[_adjacentTiles.Length]);
        }

        private void OnDisable()
        {
            _run = false;
        }
    }
}