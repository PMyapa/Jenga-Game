using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class JengaGame : MonoBehaviour
{

    public Dictionary<Block, GameObject> blockToGameObject;
    public Dictionary<GameObject, Block> gameObjectToBlock;

    public GameObject BlockPrefab;

    public Material[] mats;

    public GameObject TowerSix;
    public GameObject TowerSev;
    public GameObject TowerEig;

    private HashSet<Block> sixBlocks;
    private HashSet<Block> sevBlocks;
    private HashSet<Block> eigBlocks;

    public Block[] SixBlocks
    {
        get { return sixBlocks.ToArray(); }
    }
    public Block[] SevBlocks
    {
        get { return sevBlocks.ToArray(); }
    }
    public Block[] EigBlocks
    {
        get { return eigBlocks.ToArray(); }
    }

    [System.Serializable]
    public class StandardsObject
    {
        //public List<Block> standards { get; set; }
        public Block.BlockData[] standards = new Block.BlockData[362];
    }

    public StandardsObject SO;

    // Start is called before the first frame update
    void Start()
    {
        GetStandardsFromJson();
        GenerateTowers(TowerSix, SixBlocks);
        GenerateTowers(TowerSev, SevBlocks);
        GenerateTowers(TowerEig, EigBlocks);
    }

    public void GetStandardsFromJson()
    {
        blockToGameObject = new Dictionary<Block, GameObject>();
        gameObjectToBlock = new Dictionary<GameObject, Block>();

        sixBlocks = new HashSet<Block>();
        sevBlocks = new HashSet<Block>();
        eigBlocks = new HashSet<Block>();

        string json = File.ReadAllText(Application.dataPath + "/stack.json");

        StandardsObject rt = JsonUtility.FromJson<StandardsObject>(json);

      /*  Block[] mathStandards = new Block[JsonUtility.FromJson<RootObject>(json).standards.Length];
        mathStandards = JsonUtility.FromJson<RootObject>(json).standards;
*/
        Debug.LogError("len" + rt.standards.Length.ToString());

        for (int i = 0; i < rt.standards.Length; i++)
        {
            SO.standards[i] = rt.standards[i];
            // Access the properties of each MathStandard object
            /*int id = mathStandard.id;
            string subject = mathStandard.subject;
            string grade = mathStandard.grade;
            int mastery = mathStandard.mastery;
            string domainid = mathStandard.domainid;
            string domain = mathStandard.domain;
            string cluster = mathStandard.cluster;
            string standardid = mathStandard.standardid;
            string standarddescription = mathStandard.standarddescription;*/


            /* Vector3 pos = new Vector3(0, 0.5f * i, 0);
             GameObject blockGO = (GameObject)Instantiate(BlockPrefab, pos, Quaternion.identity, this.transform);

             blockToGameObject[mathStandards[i]] = blockGO;
             gameObjectToBlock[blockGO] = mathStandards[i];*/


            if (SO.standards[i].grade == "6th Grade")
            {
                Debug.LogError("New Six Block");
                Block b = new Block();
                b.bd = SO.standards[i];
                sixBlocks.Add(b);
            }
            if (SO.standards[i].grade == "7th Grade")
            {
                Debug.LogError("New Six Block");
                Block b = new Block();
                b.bd = SO.standards[i];
                sevBlocks.Add(b);
            }
            if (SO.standards[i].grade == "8th Grade")
            {
                Debug.LogError("New Six Block");
                Block b = new Block();
                b.bd = SO.standards[i];
                eigBlocks.Add(b);
            }
        }

    }

    public void GenerateTowers(GameObject go , Block[] grade)
    {
        for(int i = 0; i < grade.Length; i++)
        {
            //Vector3 pos = new Vector3(0, 0.5f * i, 0);
            GameObject blockGO = (GameObject)Instantiate(BlockPrefab, Tower(go).Positions[i], Tower(go).Rotations[i], go.transform);

            blockToGameObject[grade[i]] = blockGO;
            gameObjectToBlock[blockGO] = grade[i];

            float rand = Random.Range(0, 1f);
            if (rand <= 0.1f)
            {
                grade[i].Mastery = 0;
            }
            else if (rand <= 0.45)
            {
                grade[i].Mastery = 1;
            }
            else
            {
                grade[i].Mastery = 2;
            }

            blockGO.transform.GetChild(0).GetComponent<MeshRenderer>().material = mats[grade[i].Mastery];

            /*blockToGameObject[grade[i]].transform.position = Tower(go).Positions[i];
            blockToGameObject[grade[i]].transform.rotation = Tower(go).Rotations[i];
            */
        }
    }

    public class blockTransforms
    {
        public Vector3[] Positions = new Vector3[300];
        public Quaternion[] Rotations = new Quaternion[300];
    }

    public blockTransforms Tower(GameObject Base)
    {
        Vector3[] poss = new Vector3[300];
        Quaternion[] rots = new Quaternion[300];
        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                int p = 6 * i + j;
                poss[p] = Base.transform.position;
                rots[p] =Quaternion.Euler(0, 0, 0);
                if (j == 0)
                {
                    poss[p].x = poss[p].x ;
                    poss[p].y = poss[p].y + 2*i;
                    poss[p].z = poss[p].z -3 ;

                    rots[p].y = 0;
                }
                else if (j == 1)
                {
                    poss[p].x = poss[p].x;
                    poss[p].y = poss[p].y + 2 * i;
                    poss[p].z = poss[p].z;

                    rots[p].y = 0;
                }
                else if (j == 2)
                {
                    poss[p].x = poss[p].x;
                    poss[p].y = poss[p].y + 2 * i;
                    poss[p].z = poss[p].z + 3;

                    rots[p].y = 0;
                }
                else if (j == 3)
                {
                    poss[p].x = poss[p].x + 3;
                    poss[p].y = poss[p].y + (2 * i )+ 1;
                    poss[p].z = poss[p].z;

                    //rots[p].y = -90;
                    rots[p] = Quaternion.Euler(0, -90, 0);
                }
                else if (j == 4)
                {
                    poss[p].x = poss[p].x;
                    poss[p].y = poss[p].y + (2 * i) + 1;
                    poss[p].z = poss[p].z;

                    //rots[p].y = -90;
                    rots[p] = Quaternion.Euler(0, -90, 0);
                }
                else if (j == 5)
                {
                    poss[p].x = poss[p].x - 3;
                    poss[p].y = poss[p].y +( 2 * i) + 1;
                    poss[p].z = poss[p].z;

                    //rots[p].y = -90;
                    rots[p] = Quaternion.Euler(0, -90, 0);
                }
                
                
                
            }
        }

        blockTransforms bts = new blockTransforms();
        bts.Positions = poss;
        bts.Rotations = rots;
        return bts;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Block GetBlockFromGameObject(GameObject blockGO)
    {
        if (gameObjectToBlock.ContainsKey(blockGO))
        {
            return gameObjectToBlock[blockGO];
        }

        return null;
    }

    public GameObject GetGameObjectFromBlock(Block block)
    {
        if (blockToGameObject.ContainsKey(block))
        {
            return blockToGameObject[block];
        }

        return null;
    }
}
