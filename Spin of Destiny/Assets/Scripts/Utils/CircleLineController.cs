
using UnityEngine;
public class CircleLineController : MonoBehaviour
{
    public int vertices=1; 
    public float width;

    [Header("radius")]
    public float radius;
    public float growingFactor;
    public float growingTimesPerSeconds;
    private float timer;

    [Header("rotation")]
    public float rotationSpeed;
    private float theta = 0;

    public Transform center;

    private LineRenderer lineRenderer;
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();   
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        if(timer>= 1f/growingTimesPerSeconds){
            timer = 0;
            radius+=radius * growingFactor;
            //width+=width * growingFactor/2;
        }

        majCircle();
    }

    void majCircle(){
        lineRenderer.positionCount = vertices;
        lineRenderer.widthMultiplier = width;
        float dtheta = 2f*Mathf.PI/vertices;
        theta += Time.deltaTime * rotationSpeed;
        float tmp = theta;
        for(int i = 0;  i < vertices; i++){
            Vector2 position = new Vector2(center.position.x+radius*Mathf.Cos(theta), center.position.y+radius*Mathf.Sin(theta));
            lineRenderer.SetPosition(i, position);
            theta += dtheta;
        }
        theta = tmp;
    }
}
