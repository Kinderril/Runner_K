using System;
using UnityEngine;
using System.Collections;
using System.Threading;
using UnityStandardAssets._2D;
using Random = System.Random;

public class LevelController : Singleton<LevelController>
{
    private Block _lastBlock;
    public Block BlockPrefab;
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
        //
    }

    

    public void Init()
    {
        //PUT first block
        _lastBlock = Instantiate(BlockPrefab.gameObject).GetComponent<Block>();
        _lastBlock.transform.SetParent(transform,false);
        _lastBlock.transform.position = Vector3.zero;
    }

    public void Reset()
    {
        var v = hero.transform.position;
        hero.transform.position = new Vector3(v.x + 1,maxY);
        hero.m_Rigidbody2D.velocity = Vector2.zero;
    }
    private void RemoveBlock(Block _lastBlock)
    {

    }

    public void StartRotate()
    {
        if (!isRotating)
        {
            var block2rotate = Instantiate(BlockPrefab.gameObject).GetComponent<Block>();
            block2rotate.transform.localRotation = Quaternion.Euler(new Vector3(0,90,0));
            block2rotate.transform.localPosition = new Vector3(_lastBlock.transform.localPosition.x + _lastBlock.width*1.5f,0,_lastBlock.width);
            block1 = _lastBlock.transform;
            block2 = block2rotate.transform;
            isRotating = true;
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
                Debug.Log(rotate);
            }
            block2.RotateAround(pointOfRotate, Vector3.up, rotate);
            block1.RotateAround(pointOfRotate, Vector3.up, rotate);
        }
    }

    public void PutBlock()
    {
        Vector2 oldPos = new Vector2(_lastBlock.transform.position.x + _lastBlock.width / 2, _lastBlock.transform.position.y);
        
        var block = Instantiate(BlockPrefab.gameObject).GetComponent<Block>();
        block.transform.SetParent(transform,true);

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
        block.transform.position = oldPos;
        LastBlock = block;
        Debug.Log(oldPos);

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
	    currentBlockPercent = (heorX - _lastBlock.transform.position.x + _lastBlock.width/2)/_lastBlock.width;
	    if (currentBlockPercent > 0.2f)
	    {
	        PutBlock();
	    }

	}
}
