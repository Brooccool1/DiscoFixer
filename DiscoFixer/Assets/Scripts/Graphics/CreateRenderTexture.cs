using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CreateRenderTexture : MonoBehaviour
{
    private Camera _camera;
    private RawImage _image;
    private int _scale = 8;
    
    void Start()
    {
        RenderTexture texture = new RenderTexture(Screen.width / _scale, Screen.height / _scale, 32);
        texture.name = "test";
        texture.dimension = TextureDimension.Tex2D;
        texture.filterMode = FilterMode.Point;
        texture.format = RenderTextureFormat.ARGBFloat;
        texture.antiAliasing = 1;
        
        _camera = GetComponent<Camera>();
        _image = GetComponent<RawImage>();
        if (_camera)
        {
            if (_camera.targetTexture != null)
            {
                _camera.targetTexture.Release();
            }
    
            _camera.targetTexture = texture;
        }
    
        if (_image)
        {
            Camera camera = GetComponentInParent<Camera>();
            _image.rectTransform.sizeDelta = new Vector2(1, 1);
            //_image.rectTransform.sizeDelta = camera.sensorSize /*(new Vector3(Screen.width, Screen.height))*/;
            _image.texture = camera.targetTexture;
        }
    }
}
