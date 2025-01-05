using UnityEngine;

namespace OurAssets.Scripts
{
    public class AnimGif : MonoBehaviour
    {
        [SerializeField] private Texture2D[] frames;
        [SerializeField] private  float fps = 10.0f;

        private Material mat;

        private void Start () {
            mat = GetComponent<Renderer> ().material;
        }

        private void Update () {
            var index = (int)(Time.time * fps);
            index = index % frames.Length;
            mat.mainTexture = frames[index]; // usar en planeObjects
            //GetComponent<RawImage> ().texture = frames [index];
        }
    }
}
