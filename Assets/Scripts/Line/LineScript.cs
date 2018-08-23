using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour {

    public float scrollSpeed;
    private Material mat;

    private Vector2 offset = Vector2.zero;

    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Use this for initialization
    void Start()
    {
        offset = mat.GetTextureOffset("_MainTex");
    }

    // Update is called once per frame
    void Update()
    {
        offset.x += scrollSpeed * Time.deltaTime;
        mat.SetTextureOffset("_MainTex", offset);
    }
}
