using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour
{
    [Header("Movements")]
    [SerializeField] private float movementVelocity = 115 / 60f * 50;
    [SerializeField] private float speed = 115 / 60f * 4; //BPS

    private Vector3 _startPos;
    public GameObject _removePos;
    public float _beatOfThisMark;

    void Start()
    {
        _startPos = transform.position;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(_startPos, _removePos.transform.position, (Conductor._i.beatsShownInAdvance - (_beatOfThisMark - Conductor._i.songPositionInBeats)) / Conductor._i.beatsShownInAdvance);
        if (transform.position.z <= 0)
            Destroy(this.gameObject);
    }
}
