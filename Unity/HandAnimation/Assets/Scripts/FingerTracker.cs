using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FingerTracker : MonoBehaviour
{
    Transform FingerTipTransform;

    Vector3 Tip;

    GameObject Sphere;

    public LineRenderer Line;
    private bool LineStarted;
    private KeyCode startDrawingKey = KeyCode.Space;


    // Start is called before the first frame update
    void Start()
    {
        // Get finger Tip transform
        int first=0, second=1;
        Transform ModelTransform = gameObject.transform;
        Transform WorldTransform = ModelTransform.GetChild(second);
        Transform HandTransform = WorldTransform.GetChild(first);
        FingerTipTransform = HandTransform.GetChild(2).GetChild(first).GetChild(first).GetChild(first);

        // Create Sphere
        float small = 0.0033f;
        Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Sphere.transform.position = FingerTipTransform.position;
        Sphere.transform.localScale = new Vector3(small, small, small);

        // Change Sphere color
        var SphereRenderer = Sphere.GetComponent<Renderer>();
        SphereRenderer.material.SetColor("_Color", Color.blue);

        // Create Line rendered
        // Make it half as big as finger-tip ball
        // Line code based on http://gyanendushekhar.com/2020/04/05/draw-line-at-run-time-unity-3d/
        Line = GetComponent<LineRenderer>();
        Line.startColor = Color.red;
        Line.endColor = Color.red;
        Line.startWidth = small / 2.0f;
        Line.endWidth = small / 2.0f;
        Line.positionCount = 0;
        LineStarted = false;
    }

    void initializeLine(Vector3 tip)
    {
      Line.positionCount = 2;
      Line.SetPosition(0, Tip);
      Line.SetPosition(1, Tip);
    }

    void addLinePoint(Vector3 tip)
    {
      Line.positionCount++;
      Line.SetPosition(Line.positionCount-1, tip);
    }


    // Update is called once per frame
    void LateUpdate()
    {
      // Move Sphere to finger Tip
      Tip = FingerTipTransform.position;
      Sphere.transform.position = Tip;

      if (LineStarted)
      {
        addLinePoint(Tip);
      }
      else if(Input.GetKey(startDrawingKey))
      {
        initializeLine(Tip);
        LineStarted = true;
      }
      else
      {
        // Do nothing
      }

    }
}
