using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block 
{
    [System.Serializable]
    public class BlockData
    {

        public int id;
        public string subject;
        public string grade;
        public int mastery;
        public string domainid;
        public string domain;
        public string cluster;
        public string standardid;
        public string standarddescription;
    }

    public int Mastery;

    public BlockData bd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
