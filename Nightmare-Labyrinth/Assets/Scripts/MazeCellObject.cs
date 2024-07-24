using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCellObject : MonoBehaviour
{
    [SerializeField] GameObject topWall;
    [SerializeField] GameObject bottomWall;
    [SerializeField] GameObject rightWall;
    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject roof;

    public void Init (bool top, bool bottom, bool right, bool left, bool hasRoof = true)
    {
        topWall.SetActive (top);
        bottomWall.SetActive (bottom);
        rightWall.SetActive (right);
        leftWall.SetActive (left);
        roof.SetActive(hasRoof);
    }
}
