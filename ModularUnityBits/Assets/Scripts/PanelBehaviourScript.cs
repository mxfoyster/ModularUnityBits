using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBehaviourScript : MonoBehaviour
{
    /// <summary>
    /// We want the panels to rotate around a centre. They will form baiscally a polygon when viewed from 
    /// above that can be inscribed within a circle (with some room at each side)
    /// in order to set the depth we need to be away from the pivot point so the panels don't intersect,
    /// We calculate the Apothem (distance between centre of our panel and pivot point)
    /// 
    /// We will use panels of the same size, so it's a regular polygon
    /// S/((2 * tan)pi/n) where n = number of sides and s = width 
    /// Therefore apothem is
    /// https://artofproblemsolving.com/wiki/index.php/Apothem#:~:text=The%20apothem%20of%20a%20regular,inscribed%20circle%20of%20the%20polygon.
    /// 
    /// </summary>
    [SerializeField] private GameObject[] carouselPanels;
    [SerializeField] private GameObject pivotPoint;
    [SerializeField] private int sideSpace; //our buffer at the side of each panel
    private int currentPanelID;
    private int numPanels;
    private float panelAngle, currentAngle, rotateAmount;
    private float panelWidth;
    private Vector3 pivotPointPosition;

    private enum rotateOptions
    {
        left,
        right,
        stop
    }

    private rotateOptions rotateStatus;
    
    // Start is called before the first frame update
    void Start()
    {
        rotateStatus = rotateOptions.stop;
        currentAngle = 0;
        rotateAmount = 1;

        pivotPointPosition = pivotPoint.transform.position;
        numPanels = carouselPanels.Length;
        panelAngle = 360 / numPanels; //calculate angle between panel planes
        RectTransform rt = (RectTransform)carouselPanels[0].transform; //rect of first panel
        panelWidth = rt.rect.width;//get width from that

        float panelDepthOffset = CalculateOffset();
        numPanels = carouselPanels.Length;
   
        int thisPanel =  0; //panel counter within loop
        //move through all panels and position them as necessary
        foreach (var panel in carouselPanels)
        {
            thisPanel++;
            panel.transform.position = pivotPointPosition + new Vector3(0, 0, -panelDepthOffset);
            panel.transform.RotateAround(pivotPointPosition, Vector3.up, (panelAngle * thisPanel));//RotateAround(Vector3 point, Vector3 axis, float angle);
        }
        currentPanelID = 0;
        Camera.main.transform.position += Vector3.back * panelDepthOffset; //move camera back enough to see repositioned panels
    }


    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) ) rotateStatus = rotateOptions.left;
        if (Input.GetKey(KeyCode.RightArrow)) rotateStatus = rotateOptions.right;
    }
    private void FixedUpdate()
    {
        Rotate();
        //pivotPoint.transform.Rotate(0.0f, 0.5f, 0.0f, Space.World);
    }
    
    
    /// <summary>
    /// calculate apothem
    /// S/((2 * tan(pi/n)) where n = number of sides and s = width 
    /// </summary>
    /// <returns>apothem</returns>
    private float CalculateOffset()
    {
        const float pi = 3.142f;
        return (panelWidth + sideSpace) / (2f * Mathf.Tan(pi / numPanels));
    }

    private void Rotate()
    {
        switch (rotateStatus)
        {
            case rotateOptions.left:
                pivotPoint.transform.Rotate(0.0f, rotateAmount, 0.0f, Space.World);
                currentAngle += rotateAmount;
                break;
                   
            case rotateOptions.right:
                pivotPoint.transform.Rotate(0.0f, -rotateAmount, 0.0f, Space.World);
                currentAngle -= rotateAmount;
                break;
        }
        if (currentAngle % panelAngle == 0) rotateStatus = rotateOptions.stop;//if we're on a panel plane, stop
    }

}
