using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Game
{
    public class Item : MonoBehaviour
    {

        private bool _run;
        private Tilemap _tilemap;
        private Vector3Int _tileMain;
        public Vector3Int[] adjacentTiles;
        private Vector3Int[] _adjacentTiles;

        public ItemType itemType = ItemType.Key;
        
        public UnityEvent onPlayerTouch;

        private void Start()
        {
            _tilemap = transform.parent.GetComponent<Tilemap>();
            _tileMain = _tilemap.WorldToCell(transform.position);

            _adjacentTiles = new Vector3Int[adjacentTiles.Length];

            for (var i = 0; i < adjacentTiles.Length; i++)
            {
                _adjacentTiles[i] = new Vector3Int(
                    _tileMain.x + adjacentTiles[i].x,
                    _tileMain.y + adjacentTiles[i].y,
                    _tileMain.z + adjacentTiles[i].z);
                
            }
            
            GameManager.gm.itemManager.AddEvent(onPlayerTouch,itemType);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            if (_run) return;
            _run = true;
                
            onPlayerTouch.Invoke();
            
            Invoke(nameof(DestroyTiles),0.1f);
            // gameObject.SetActive(false);
        }
        
        private void DestroyTiles()
        {
            _tilemap.SetTiles(_adjacentTiles, new TileBase[_adjacentTiles.Length]);
        }
    }
}