
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets._2D;

[RequireComponent(typeof (PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour
{
    private PlatformerCharacter2D m_Character;
    private bool m_Jump;


    private void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
    }
        
    public void OnMouseUp(BaseEventData bed)
    {
        m_Jump = false;
    }

    public void OnMouseDown(BaseEventData bed)
    {
        var ped = bed as PointerEventData;
        if (ped.position.x > Screen.width/2)
        {
            m_Jump = true;
        }
        else
        {
            m_Jump = true;
            LevelController.Instance.StartRotate();
        }
    }

    private void FixedUpdate()
    {
        m_Character.Move(0.3f,  false,m_Jump);
    }
}

