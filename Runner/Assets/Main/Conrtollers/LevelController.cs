using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Threading;
using UnityStandardAssets._2D;
using Random = System.Random;

public class LevelController : Singleton<LevelController>
{
    public Block _lastBlock;
    public List<Block> BlockPrefabs = new List<Block>();
    public PlatformerCharacter2D hero;
    public float currentBlockPercent = 0;
    public Transform block1;
    public Transform block2;
    public bool isRotating = false;
    public float curRotateAngel = 0;
    public Random rnd = new Random(8765);
    public float maxY;
    public float minY;
    public float JumpHeight;
    public float JumpWidth;
    private const float rndConst = 1000;
    public Block currentBlock;


    public Block LastBlock
    {
        get { return _lastBlock; }
        set
        {
            if (_lastBlock != null)
                RemoveBlock(_lastBlock);
            _lastBlock = value;
        }
    }

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        //PUT first block
        _lastBlock = Instantiate(BlockPrefabs[0].gameObject).GetComponent<Block>();
        _lastBlock.transform.SetParent(transform,false);
        _lastBlock.transform.position = Vector3.zero;
    }

    public void Reset()
    {
        var v = hero.transform.position;
        hero.transform.position = new Vector3(v.x + 1,maxY);
        hero.m_Rigidbody2D.velocity = Vector2.zero;
    }
    private void RemoveBlock(Block b)
    {
        //Destroy(b.gameObject);
    }

    private Block GetRandomBlockPrefab()
    {
        return BlockPrefabs[rnd.Next(0, BlockPrefabs.Count)];
    }

    public void StartRotate()
    {
        if (!isRotating)
        {
            Block blockP = GetRandomBlockPrefab();
            var block2rotate = Instantiate(blockP.gameObject).GetComponent<Block>();
            block2rotate.transform.SetParent(transform);
            block2rotate.transform.localRotation = Quaternion.Euler(new Vector3(0,90,0));
            block2rotate.transform.localPosition = new Vector3(currentBlock.transform.localPosition.x + JumpWidth * 1.5f, 0, currentBlock.width);
            block1 = currentBlock.transform;
            block2 = block2rotate.transform;
            isRotating = true;
            curRotateAngel = 0;
            Destroy(_lastBlock.gameObject);
            LastBlock = block2rotate;
            //Debug.Log("rotate " + block1 + "   " + block2);
        }
    }

    private void Update()
    {
        if (isRotating)
        {
            Vector3 pointOfRotate = new Vector3(block2.localPosition.x, 0, 0);
            float rotate = Time.deltaTime*222f;
            curRotateAngel += rotate;
            Debug.Log(curRotateAngel + "   " + rotate);
            if (curRotateAngel > 90)
            {
                float ang = curRotateAngel - rotate;
                rotate = 90-ang;
                curRotateAngel = 90;
                isRotating = false;
                var p = block2.transform.localPosition;
                block2.transform.localPosition = new Vector3(p.x,p.y,0);
                Debug.Log(rotate);
            }
            block2.RotateAround(pointOfRotate, Vector3.up, rotate);
            block1.RotateAround(pointOfRotate, Vector3.up, rotate);
        }
    }

    public void PutBlock()
    {
        //Vector2 oldPos = new Vector2(_lastBlock.transform.position.x + _lastBlock.width / 2, _lastBlock.transform.position.y);

        Block blockP = GetRandomBlockPrefab();
        var block = Instantiate(blockP.gameObject).GetComponent<Block>();
        block.transform.SetParent(transform,true);
        /*
        var newY = GetRandom(Mathf.Clamp(oldPos.y + JumpHeight, minY, maxY), Mathf.Clamp(oldPos.y - JumpHeight, minY, maxY));
        float deltaX;
        if (newY > oldPos.y)
        {
            deltaX = JumpWidth + (JumpWidth-(newY-oldPos.y))*JumpHeight/JumpWidth;
        }
        else
        {
            deltaX = JumpWidth*2 + (oldPos.y - newY)*JumpWidth/oldPos.y;
        }
        oldPos.x = deltaX + oldPos.x + block.width/2;
        oldPos.y = newY;
        block.transform.position = oldPos;*/
        CalcPosForBlock(block, _lastBlock);
        currentBlock = _lastBlock;
        LastBlock = block;

    }

    private void CalcPosForBlock(Block block, Block oldBlock)
    {
        Vector2 start = new Vector2(oldBlock.transform.position.x + oldBlock.width, oldBlock.transform.position.y + oldBlock.heightR);
        var newY = GetRandom(Mathf.Clamp(start.x + JumpHeight, minY, maxY), Mathf.Clamp(start.y - JumpHeight, minY + oldBlock.heightR, maxY));
        float deltaX;
        bool isHeigher = (newY > start.y);
        if (isHeigher)
        {
            deltaX = JumpWidth + (JumpWidth - (newY - start.y)) * JumpHeight / JumpWidth;
        }
        else
        {
            deltaX = JumpWidth * 2 + (start.y - newY) * JumpWidth / start.y;
        }
        newY -= block.heightL;
        block.transform.position = new Vector3(start.x + deltaX, newY);

        Debug.Log("oldblock:" + oldBlock.transform.position+ "  start:" + start + "  " + oldBlock.width + " >> " + block.transform.position + " " + block.width + "    JumpWidth:" + JumpWidth + "   isHeigher:" + isHeigher);
    }

    public float GetRandom(float a, float b)
    {
        return UnityEngine.Random.Range(a, b);
    }

    /*static float NextFloat(Random random,float a,float b)
    {
        double mantissa = (random.NextDouble() * 2.0) - 1.0;
        double exponent = Math.Pow(2.0, random.Next(a, b));
        return (float)(mantissa * exponent);
    }*/

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    var heorX = hero.transform.position.x;
	    if (_lastBlock != null)
	    {
	        currentBlockPercent = (heorX - _lastBlock.transform.position.x + _lastBlock.width/2)/_lastBlock.width;
	        if (currentBlockPercent > 0.2f)
	        {
	            PutBlock();
	        }
	    }
	}
}
