using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class ItemManager : MonoBehaviour
    {
        
        private void GetKey()
        {
            PlayerController.Keys++;
        }

        public void GetMushRoomRed()
        {
            
        }
        
        public void GetMushRoomPink()
        {
            
        }

        public void AddEvent(UnityEvent uevent, ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Key:
                    uevent.AddListener(GetKey);
                    break;
                case ItemType.Sword:
                    break;
                case ItemType.Torch:
                    break;
                case ItemType.MushroomRed:
                    break;
                case ItemType.MushRoomPink:
                    break;
                case ItemType.Snake:
                    break;
                case ItemType.Skull:
                    break;
                case ItemType.Spider:
                    break;
                case ItemType.Lava:
                    break;
                case ItemType.FireChainWall:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null);
            }   
        }
        
    }

    public enum ItemType
    {
        Key, Sword, Torch, MushroomRed, MushRoomPink, Snake, Skull, Spider, Lava, FireChainWall
    }
}
