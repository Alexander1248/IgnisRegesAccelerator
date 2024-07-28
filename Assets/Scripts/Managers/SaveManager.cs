using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Items;
using Player;
using UnityEditor;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.Localization;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        private static GameState _state;
        [SerializeField] private string path;
        [SerializeField] private string[] supportedVersions;
        [SerializeField] private InventoryManager manager;
        
        private void Start()
        {
            GameState state;
            if (_state != null) state = _state;
            else
            {
                if (!File.Exists(path)) return;
                state = JsonUtility.FromJson<GameState>(File.ReadAllText(path));
                if (state.supportedVersions.All(version => Application.version != version)) return;
            }
            manager.Initialize(state.inventories.Select(inventory => inventory.items.ToDictionary(
                locations => new Vector2Int(locations.x, locations.y),
                locations =>
                {
                    var o = ScriptableObject.CreateInstance(Type.GetType(locations.type));
                    JsonUtility.FromJsonOverwrite(locations.data, o);
                    return o as Item;
                }
                )).ToArray());
        }

        public void Save()
        {
            var inventories = new Inventory[manager.Count];
            for (var i = 0; i < inventories.Length; i++)
            {
                inventories[i] = new Inventory
                {
                    items = new List<ItemLocations>()
                };
                foreach (var (key, value) in manager[i])
                    inventories[i].items.Add(new ItemLocations
                    {
                        data = JsonUtility.ToJson(value)
                    });
            }

            _state = new GameState
            {
                supportedVersions = supportedVersions.Append(Application.version).ToArray(),
                inventories = inventories
            };
            File.WriteAllText(path, JsonUtility.ToJson(_state));
        }

        public void Clear()
        {
            _state = null;
            File.Delete(path);
        }
        
        
        [Serializable]
        private class GameState
        {
            public string[] supportedVersions;
            public Inventory[] inventories;
        }
        
        [Serializable]
        private class Inventory
        {
            public List<ItemLocations> items;
        }
        [Serializable]
        private class ItemLocations
        {
            public int x;
            public int y;
            public string type;
            public string data;
        }
        [Serializable]
        private class Item
        { 
            public Vector2Int[] shape;
            public GameObject uiPrefab;
            public LocalizedString itemName;
            public LocalizedString itemDescription;
            public bool secured;
        }
    }
}