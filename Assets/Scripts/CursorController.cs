using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public int scaleFactor = 10;
    public int stepsPerSecond = 5;
    public int movementSpeed = 10;
    public GameObject pixelPrefab;
    public GameObject cursorPositionDisplay;
    private Vector3 begin, end;

    private bool isDrawing = false;
    private string toPositionString(Vector3 point)
    {
        return $"[{point.x.ToString("0.0")}, {point.y.ToString("0.0")}, {point.z.ToString("0.0")}]";
    }
    private int abs(int x)
    {
        return x >= 0 ? x : -x;
    }
    private int scale(float x)
    {
        return (int)(x * scaleFactor);
    }

    private void drawPoint(int x, int y, int z)
    {
        float s = 1 / (float)scaleFactor;
        var pixel = Instantiate(pixelPrefab, new Vector3(x * s, y * s, z * s), Quaternion.identity);
        pixel.transform.localScale = new Vector3(s, s, s);
    }
    // create 2 arrays, (x, y) and (x, z) and merge them

    private IEnumerator drawBresenhamLine(Vector3 begin, Vector3 end) {
        float coroutineInterval = 1f / stepsPerSecond;
        int x = scale(begin.x), y = scale(begin.y), z = scale(begin.z);
        var dv = end - begin;
        var dx = scale(dv.x);
        var dy = scale(dv.y);
        var dz = scale(dv.z);
        var incX = (dx < 0) ? -1 : 1;
        var incY = (dy < 0) ? -1 : 1;
        var incZ = (dz < 0) ? -1 : 1;
        var l = abs(dx);
        var m = abs(dy);
        var n = abs(dz);
        var twoL = l * 2;
        var twoM = m * 2;
        var twoN = n * 2;
        int e1, e2;
        if ((l >= m) && (l >= n))
        {
            e1 = twoM - l;
            e2 = twoN - l;
            for (int i = 0; i < l; i++)
            {
                drawPoint(x, y, z);
                yield return new WaitForSeconds(coroutineInterval);
                if (e1 > 0)
                {
                    y += incY;
                    e1 -= twoL;
                }
                if (e2 > 0)
                {
                    z += incZ;
                    e2 -= twoL;
                }
                e1 += twoM;
                e2 += twoN;
                x += incX;
            }
        }
        else if ((m >= l) && (m >= n))
        {
            e1 = twoL - m;
            e2 = twoN - m;
            for (int i = 0; i < m; i++)
            {
                drawPoint(x, y, z);
                yield return new WaitForSeconds(coroutineInterval);
                if (e1 > 0)
                {
                    x += incX;
                    e1 -= twoM;
                }
                if (e2 > 0)
                {
                    z += incZ;
                    e2 -= twoM;
                }
                e1 += twoL;
                e2 += twoN;
                y += incY;
            }
        }
        else
        {
            e1 = twoM - n;
            e2 = twoL - n;
            for (int i = 0; i < n; i++)
            {
                drawPoint(x, y, z);
                yield return new WaitForSeconds(coroutineInterval);
                if (e1 > 0)
                {
                    y += incY;
                    e1 -= twoN;
                }
                if (e2 > 0)
                {
                    x += incX;
                    e2 -= twoN;
                }
                e1 += twoM;
                e2 += twoL;
                z += incZ;
            }
        }
        drawPoint(x, y, z);
        yield return null;
    }

    private void onEndPointSelected(Vector3 point) {
        if (isDrawing) {
            isDrawing = false;
            end = point;
            Debug.Log(toPositionString(begin) + " -> " + toPositionString(end));
            StartCoroutine(drawBresenhamLine(begin, end));
        } else {
            isDrawing = true;
            begin = point;
        }
    }
    void Start() {
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            onEndPointSelected(transform.position);
            return;
        }
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            return;
        }
        float horizontal = Input.GetAxis("Horizontal");
        float verticle = Input.GetAxis("Vertical");
        float zDirection = 0;
        if (Input.GetKey(KeyCode.I))
        {
            zDirection = Input.GetKey(KeyCode.LeftShift) ? -1 : 1;
        }
        float factor = Time.deltaTime * movementSpeed;
        transform.position += new Vector3(horizontal * factor, verticle * factor, zDirection * factor);
        cursorPositionDisplay.GetComponent<TextMeshPro>().text = toPositionString(transform.position);
    }
}
