using UnityEngine;
using System.Collections;
using System.Threading;
using UnityStandardAssets._2D;

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
        int deltaX = 1;
        oldPos.x = deltaX + oldPos.x + block.width/2;
        block.transform.position = oldPos;
        LastBlock = block;

    }

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
