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
            GameManager.instance.Score += 500;
        }

        private void GetMushRoomRed()
        {
            GameManager.instance.Score += 2000;
        }

        private void GetMushRoomPink()
        {
            GameManager.instance.Score += 1000;
        }

        private void FireChainWall()
        {
            print("Burn!@");
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
                    uevent.AddListener(GetMushRoomRed);
                    break;
                case ItemType.MushRoomPink:
                    uevent.AddListener(GetMushRoomPink);
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
                    uevent.AddListener(FireChainWall);
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
