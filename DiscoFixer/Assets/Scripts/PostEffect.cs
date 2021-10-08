using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostEffect : MonoBehaviour
{
    [SerializeField] private float _beatSync = 0.1f;
    [Header("X = Minimum value, Y = Maximum value")]
    [SerializeField] private Vector2 _bloomMinMax = new Vector2(0, 0);
    [SerializeField, Tooltip("Max can't be higher than 1")] private Vector2 _chromaticMinMax = new Vector2(0, 0);
    [SerializeField, Tooltip("Max can't be higher than 1")] private Vector2 _lenDistMinMax = new Vector2(0, 0);

    private float[] _targetFloat = new float[3];
    
    public Volume volume;
    ChromaticAberration chromatic;
    Bloom bloom;
    LensDistortion distortion;
    private bool allowUpdate = false;
    
    // Skips beats
    private int _offBeat = 2;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.beat.onBeat += PostFX;
        volume = GetComponent<Volume>();

        volume.profile.TryGet<Bloom>(out bloom);
        volume.profile.TryGet<ChromaticAberration>(out chromatic);
        volume.profile.TryGet<LensDistortion>(out distortion);
    }
    
    private void PostFX()
    {
        if (_offBeat >= 2)
        {
            if (chromatic.intensity.value <= _chromaticMinMax.x + 0.1f)
            {
                _targetFloat[0] = _bloomMinMax.y;
                _targetFloat[1] = _chromaticMinMax.y;
                _targetFloat[2] = _lenDistMinMax.y;
            }

            _offBeat = 0;
        }

        _offBeat++;
    }
    
    void Update()
    {
       bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, _targetFloat[0], _beatSync * Time.deltaTime);
       chromatic.intensity.value = Mathf.Lerp(chromatic.intensity.value, _targetFloat[1], _beatSync * Time.deltaTime);
       distortion.intensity.value = Mathf.Lerp(distortion.intensity.value, _targetFloat[2], _beatSync * Time.deltaTime);

       
       if (chromatic.intensity.value >= _chromaticMinMax.y - 0.2f)
       {
           _targetFloat[0] = _bloomMinMax.x;
           _targetFloat[1] = _chromaticMinMax.x;
           _targetFloat[2] = _lenDistMinMax.x;
       }
        
    }
}
