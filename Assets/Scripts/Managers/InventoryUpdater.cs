using System;
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
                foreach (var (key, value) in manager[i])
                {
                    var obj = Instantiate(value.UIPrefab, gridContents[i]);
                    obj.name = $"Inv_Item_{i}_{key.x}_{key.y}";
                    var t = obj.GetComponent<RectTransform>();
                    t.anchorMax = Vector2.zero;
                    t.anchorMin = Vector2.zero;
                    t.anchoredPosition = key * cell.rect.size + t.rect.size / 2;
                }
            }
        }

        private void OnDisable()
        {
            foreach (var t in gridContents)
                for (var j = 0; j < t.childCount; j++)
                    Destroy(t.GetChild(0).gameObject);
        }
    }
}