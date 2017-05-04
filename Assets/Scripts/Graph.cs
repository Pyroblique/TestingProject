using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public float nodeRadius = 1f;
    public LayerMask unwalkableMask;

    public Node[,] nodes;

    private float nodeDiameter;
    private int gridSizeX, gridSizeZ;
    private Vector3 scale;
    private Vector3 halfScale;

    // Use this for initialization
    void Start()
    {
        CreateGrid();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(transform.position, transform.localScale);
        if(nodes != null)
        {
            for(int x = 0; x < nodes.GetLength(0); x++)
            {
                for (int z = 0; z < nodes.GetLength(0); z++)
                {
                    // Get the node and store it in variable
                    Node node = nodes[x, z];

                    Gizmos.color = node.walkable ? new Color(0, 0, 1, 0.5f) : new Color(1, 0, 0, 0.5f);

                    // Draw a sphere to represent the node
                    Gizmos.DrawSphere(node.position, nodeRadius);
                }
            }
        }
    }


    // Generates a 2D grid on the X and Y axis
    public void CreateGrid()
    {
        // Calculate the node diameter
        nodeDiameter = nodeRadius * 2f;

        // Get transform's scale
        scale = transform.localScale;

        // Half the scale
        halfScale = scale / 2f;

        // Calculate grid size in (int) form
        gridSizeX = Mathf.RoundToInt(scale.x / nodeDiameter); 
        gridSizeZ = Mathf.RoundToInt(scale.z / nodeDiameter);

        // Create a grid of that size
        nodes = new Node[gridSizeX, gridSizeZ];

        // Get the bottom left point of the position
        Vector3 bottomLeft = transform.position - Vector3.right * halfScale.x - Vector3.forward * halfScale.z;
        
        // Loop through all nodes in grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for(int z = 0; z < gridSizeZ; z++)
            {
                // Calculate offset for x and z
                float xOffset = x * nodeDiameter + nodeRadius;
                float zOffset = z * nodeDiameter + nodeRadius;
                // Create position using offsets
                Vector3 nodePoint = bottomLeft + Vector3.right * xOffset + Vector3.forward * zOffset;
                // Use physics to check if node collided with non-walkable object
                bool walkable = !Physics.CheckSphere(nodePoint, nodeRadius, unwalkableMask);
                // Create the node and put it in the 2D array
                nodes[x, z] = new Node(walkable, nodePoint);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
