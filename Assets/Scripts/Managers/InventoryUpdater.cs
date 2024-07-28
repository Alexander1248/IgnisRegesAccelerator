﻿using System;
using Player;
using UnityEngine;

namespace Managers
{
    public class InventoryUpdater : MonoBehaviour
    {
        [HideInInspector] public InventoryManager manager;
        [HideInInspector] public RectTransform[] gridContents;
        [HideInInspector] public RectTransform cell;

        private void OnEnable()
        {
            for (var i = 0; i < manager.Count; i++)
            {
                for (var j = 0; j < gridContents[i].childCount; j++)
                {
                    var o = gridContents[i].GetChild(j);
                    var parts = o.name.Replace($"Inv_Item_{i}_", "").Split("_");
                    if (manager.GetItem(i, int.Parse(parts[0]), int.Parse(parts[1])) == null)
                        Destroy(o.gameObject);
                }

                foreach (var (key, value) in manager[i])
                {
                    var objName = $"Inv_Item_{i}_{key.x}_{key.y}";
                    if (gridContents[i].Find(objName) != null) continue;
                    var obj = Instantiate(value.UIPrefab, gridContents[i]);
                    obj.name = objName;
                    var t = obj.GetComponent<RectTransform>();
                    t.anchorMax = Vector2.zero;
                    t.anchorMin = Vector2.zero;
                    t.anchoredPosition = key * cell.rect.size + t.rect.size / 2;
                    value.Draw(t);
                }
            }
        }
    }
}