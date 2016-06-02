using UnityEngine;
using System.Collections;
using System;

public class LaserPuzzleManager : MonoBehaviour {

    public ParticleSystem laserParticules;
    public Camera mainCamera;
    public GameObject gridPlane;

    private enum NodeType {
        START,
        RELAY,
        END
    }

    private class LaserNode {
        public bool activated;
        public NodeType type;
        public LaserRay[] laserRays;
        public GameObject gameObj;

        public LaserNode(NodeType type, GameObject gameObj, LaserRay[] lasersRays, bool activated)
        {
            this.type = type;
            this.laserRays = lasersRays;
            this.activated = activated;
            this.gameObj = gameObj;
        }
    }
    
    private class LaserRay {
        public ParticleSystem particules;
        public Vector3 dir;
        public Vector3 pos;
        public Transform parentNodeTransform;
        public LaserNode pointedNode = null;

        public LaserRay(Transform nodeTransform, Transform laserTransform, ParticleSystem originalParticules)
        {
            parentNodeTransform = nodeTransform;
            particules = Instantiate(originalParticules); // clone

            Quaternion laserRotation = laserTransform.rotation;
            Vector3 laserDir = laserTransform.position - nodeTransform.position;
            this.dir = laserDir;

            particules.transform.rotation = laserRotation;
            UpdatePosition();
        }
       
        public void UpdatePosition()
        {
            this.pos = parentNodeTransform.position + dir * 0.51f;
            particules.transform.position = this.pos;
        }
    }
    
    private LaserNode[] nodes;
    private Transform manipulatedNode = null;
    private bool win = false;

    void Start () {
        InitGameState();
	}

    void Update()
    {
        ListenInput();
        RefreshNodesStates();
        UpdateRender();
        CheckWin();
    }

    void InitGameState()
    {
        int countNodes = transform.childCount;
        int lastNodeIndex = countNodes - 1;
        nodes = new LaserNode[countNodes];

        for(int i=0; i<countNodes; i++) {

            Transform target = transform.GetChild(i);
            bool activated = false;

            NodeType type;
            if (i == 0) {
                type = NodeType.START;
                activated = true;
            }
            else if (i == lastNodeIndex) {
                type = NodeType.END;
            } 
            else {
                type = NodeType.RELAY;
            }
                

            int nbLasers = target.childCount;
            LaserRay[] rays = new LaserRay[nbLasers];
            for(int j=0; j< nbLasers; j++)
            {
                Transform laserTransform = target.transform.GetChild(j);
                LaserRay ray = new LaserRay(target.transform, laserTransform, laserParticules);
                rays[j] = ray;
            }

            LaserNode node = new LaserNode(type, target.gameObject, rays, activated);
            nodes[i] = node;
        }
        Destroy(laserParticules);
    }
	
	void ListenInput()
    {
        if(!win)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (manipulatedNode != null)
                    manipulatedNode = null;
                else
                {
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        Transform objectHit = hit.transform;
                        if (objectHit.IsChildOf(transform))
                        {
                            LaserNode clickedNode = null;
                            for (int i = 0; i < nodes.Length; i++)
                            {
                                if (objectHit.gameObject == nodes[i].gameObj)
                                {
                                    clickedNode = nodes[i];
                                    break;
                                }
                            }
                            if (clickedNode != null && clickedNode.type != NodeType.START && clickedNode.type != NodeType.END)
                                manipulatedNode = objectHit;
                        }
                    }
                }
            }

            if (manipulatedNode != null)
            {
                Collider nodeCollider = manipulatedNode.gameObject.GetComponent<Collider>();
                nodeCollider.enabled = false;
                RaycastHit planeHit;
                if (Physics.Raycast(ray, out planeHit))
                {
                    if (planeHit.transform.gameObject == gridPlane)
                        manipulatedNode.transform.position = planeHit.point + Vector3.up * 0.5f;
                }
                nodeCollider.enabled = true;
            }
        }
        
    }

    void CheckWin()
    {
        if (nodes[nodes.Length - 1].activated)
        {
            EndGame();
            win = true;
        }
    }

    void EndGame()
    {
        PlayerProgress progressComp = GameObject.Find("PlayerProgress").GetComponent<PlayerProgress>();
        progressComp.WinPuzzleLaser();
    }

    void RefreshNodesStates(bool disableNecessary = true)
    {
        foreach (LaserNode node in nodes)
        {
            if(node.activated)
            {
                foreach(LaserRay ray in node.laserRays)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(new Ray(ray.pos, ray.dir), out hit))
                    {
                        Transform objectHit = hit.transform;
                        if(objectHit.IsChildOf(transform))
                        {
                            GameObject gameObj = objectHit.gameObject;
                            foreach(LaserNode n in nodes)
                            {
                                if(n.gameObj == gameObj)
                                {
                                    // TODO : update distance
                                    n.activated = true;
                                    ray.pointedNode = n;
                                    //break; // pb ici ?
                                }
                            }
                        }
                        else if(disableNecessary)
                        {
                            // TODO : update distance
                            DisableTargetedNodes(node, 0);
                        }
                    }
                    else if(disableNecessary)
                    {
                        // TODO : update distance
                        DisableTargetedNodes(node, 0);
                    }
                        
                }
            }
            else if(disableNecessary)
            {
                DisableTargetedNodes(node, 0);
            }
        }
    }

    void DisableTargetedNodes(LaserNode sourceNode, int recur)
    {
        recur++;
        foreach (LaserRay ray in sourceNode.laserRays)
        {
            LaserNode pointedNode = ray.pointedNode;
            if (pointedNode != null)
            {
                if(pointedNode.activated && pointedNode.type != NodeType.START)
                {
                    if(recur < nodes.Length) // triche contre bug stack overflow
                    {
                        DisableTargetedNodes(pointedNode, recur);
                        pointedNode.activated = false;
                        RefreshNodesStates(false);
                    }
                }
                pointedNode = null;
            }
        }
    }

    void UpdateRender()
    {
        foreach(LaserNode node in nodes)
        {
            Material nodeMaterial = node.gameObj.GetComponent<Renderer>().material;
            if (node.activated)
            {
                nodeMaterial.color = new Color(0f, 1f, 0f);
                foreach(LaserRay ray in node.laserRays)
                {
                    ray.UpdatePosition();
                    ParticleSystem particules = ray.particules;
                    if(particules.isStopped)
                        particules.Play();
                }
            }
            else
            {
                nodeMaterial.color = new Color(1f, 0f, 0f);
                foreach (LaserRay ray in node.laserRays)
                {
                    ray.UpdatePosition();
                    ParticleSystem particules = ray.particules;
                    if(particules.isPlaying)
                        particules.Stop();
                }
            }
        }
    }
}
