using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header("Order of sprite is important! First is up then changes clockwise")]
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private Sprite _danceSprite;
    
    private SpriteRenderer _sprite;
    private Vector2 _direction = Vector2.zero;

    private Player playerState = new ();
    
    private int _currentSprite = 0;
    
    //          0,1
    //     -1,1  |  1,1
    // -1,0  <---|--->  1,0
    //    -1,-1  |  1,-1
    //         0,-1
    private Vector2[] _directionToAnimation = new Vector2[8]
    {
        new (0, 1),
        new (1, 1),
        new (1, 0),
        new (1, -1),
        new (0, -1),
        new (-1, -1),
        new (-1, 0),
        new (-1, 1)
    };
    
    void Start()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }
    
    void Update()
    {
        if (Player.state == Player.State.Walking)
        {
            _direction = Player.direction;
            _directionConverter();
            _sprite.sprite = _sprites[_currentSprite];
        }
        else
        {
            _sprite.sprite = _danceSprite;
        }
    }

    private void _directionConverter()
    {
        for (int i = 0; i < _directionToAnimation.Length; i++)
        {
            if (_direction == _directionToAnimation[i])
            {
                _currentSprite = i;
            }
        }
    }
}
