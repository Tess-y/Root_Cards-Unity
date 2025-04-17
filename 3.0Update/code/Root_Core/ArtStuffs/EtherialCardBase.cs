using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

namespace RootCore.ArtStuffs {
    public class EtherialCardBase:CardAnimation {
        public float amount;

        public float speed = 1f;

        private float startSeed;

        private Vector3 startPos;

        Dictionary<Transform, Vector3> startPoses = new Dictionary<Transform, Vector3>();
        Dictionary<Transform, float> startSeeds = new Dictionary<Transform, float>();

        private void Awake() {

            foreach(var obj in this.transform.GetComponentsInChildren<Transform>()) {
                if(obj.name.ContainsOR("Background", "Art")) continue;
                startPoses.Add(obj,obj.localPosition);
                startSeeds.Add(obj, Random.Range(0f, 100000f));
            }
        }

        private void Update() {
            foreach(var obj in startPoses.Keys) {
                Vector2 vector = new Vector2(Mathf.PerlinNoise(startSeeds[obj] + Time.unscaledTime * speed, startSeeds[obj] + Time.unscaledTime * speed - 0.5f), Mathf.PerlinNoise(startSeeds[obj] + Time.unscaledTime * speed, startSeeds[obj] + Time.unscaledTime * speed) - 0.5f);
                obj.localPosition = startPoses[obj] + (Vector3)vector * amount;
                if(obj.GetComponent<Image>() is Image img)
                    img.color = Colour.New(img.color.r, img.color.g, img.color.b, vector.y * vector.x);
                if(obj.GetComponent<ProceduralImage>() is ProceduralImage procImg)
                    procImg.color = Colour.New(procImg.color.r, procImg.color.g, procImg.color.b, vector.y * vector.x);
                if(obj.GetComponent<TextMeshProUGUI>() is TextMeshProUGUI text)
                    text.color = Colour.New(text.color.r, text.color.g, text.color.b, vector.x);
            }
        }
    }
}