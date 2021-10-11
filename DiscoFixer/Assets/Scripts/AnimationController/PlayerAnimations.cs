using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites;
    private SpriteRenderer _sprite;
    
    void Start()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    
    void Update()
    {
        
        //Player.direction
    }
}
