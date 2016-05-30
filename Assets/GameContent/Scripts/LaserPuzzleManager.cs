using UnityEngine;
using System.Collections;
using System;

public class LaserPuzzleManager : MonoBehaviour {

    public ParticleSystem laserParticules;

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
        public LaserNode pointedNode = null;

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

        public LaserRay(Vector3 pos, Quaternion rotation, Vector3 dir, ParticleSystem originalParticules)
        {
            particules = Instantiate(originalParticules); // clone
            particules.transform.position = pos;
            particules.transform.rotation = rotation;
            this.dir = dir;
            this.pos = pos + dir * 0.51f;
        }

    }
    
    private bool win = false;
    private LaserNode[] nodes;

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
                Transform laserTransform = target.GetChild(j);
                Quaternion laserRotation = laserTransform.rotation;
                Vector3 laserDir = laserTransform.position - target.position;
                LaserRay ray = new LaserRay(target.position, laserRotation, laserDir, laserParticules);
                rays[j] = ray;
            }

            LaserNode node = new LaserNode(type, target.gameObject, rays, activated);
            nodes[i] = node;
        }
        Destroy(laserParticules);
    }
	
	void ListenInput()
    {

    }

    void CheckWin()
    {
        
    }

    void EndGame()
    {
        Destroy(gameObject);
    }

    void RefreshNodesStates()
    {
        foreach(LaserNode node in nodes)
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
                                    node.pointedNode = n;
                                    break; // pb ici ?
                                }
                            }
                        }
                        else
                        {
                            // TODO : update distance
                            if (node.pointedNode != null)
                            {
                                node.pointedNode.activated = false;
                                node.pointedNode = null;
                            }
                        }
                    }
                    else
                    {
                        // TODO : update distance
                        if (node.pointedNode!=null)
                        {
                            node.pointedNode.activated = false;
                            node.pointedNode = null;
                        }
                    }
                        
                }
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
                    ParticleSystem particules = ray.particules;
                    if(particules.isPlaying)
                        particules.Stop();
                }
            }
        }
    }
}
