using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;

public class SpawnUI : MonoBehaviour
{
    [Serializable]
    public class SpriteSelection
    {
        public UnitType type;
        public Sprite sprite;
    }

    public List<SpriteSelection> spriteSelections;
    public float positionSpread;

    private Dictionary<UnitType, Sprite> spriteMappings;
    private List<GameObject> spriteDisplays = new List<GameObject>();


    void Awake()
    {
        InvokeRepeating("UpdateDisplay", 0.25f, 0.25f);
        spriteMappings = spriteSelections.ToDictionary(s => s.type, s => s.sprite);
    }

    void UpdateDisplay()
    {
        if (WaveManager.S == null || WaveManager.S.IsWaveRunning)
            return;

        List<Sprite> sprites = Spawn.S.ToSpawn.Select(t => spriteMappings[t]).ToList();
        int count = sprites.Count;

        TrimToSize(count);

        for (int i = 0; i < sprites.Count; i++)
        {
            if (i >= spriteDisplays.Count)
            {
                CreateDisplay(sprites[i]);
            }
            else
            {
                SetSprite(sprites[i], i);
            }
        }
    }

    private void TrimToSize(int size)
    {
        if (spriteDisplays.Count <= size)
            return;

        for (int i = size; i < spriteDisplays.Count; i++)
        {
            Destroy(spriteDisplays[i]);
        }

        spriteDisplays.RemoveRange(size, spriteDisplays.Count - size);
    }

    private void CreateDisplay(Sprite spr)
    {
        float xpos = spriteDisplays.Count * positionSpread;
        GameObject display = new GameObject("SpawnDisplay", typeof(Image));
        display.GetComponent<Image>().sprite = spr;

        display.transform.SetParent(this.transform);
        display.transform.localPosition = new Vector3(xpos, 0);
        display.transform.localScale = Vector3.one;

        this.spriteDisplays.Add(display);
    }

    private void SetSprite(Sprite spr, int index)
    {
        GameObject display = this.spriteDisplays[index];
        display.GetComponent<Image>().sprite = spr;
    }
}
