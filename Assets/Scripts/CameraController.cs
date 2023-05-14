using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _mouseSensitivity = 3.0f;

    private float _rotationY;
    private float _rotationX;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _distanceFromTarget = 3.0f;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    [SerializeField]
    private float _smoothTime = 0.2f;

    [SerializeField]
    private Vector2 _rotationXMinMax = new Vector2(-40, 40);


    delegate void UpdateFunc();
    UpdateFunc Update_CurrentFunc;

    Block blockUnderMouse;

    JengaGame jengaGame;

    public LayerMask LayerIDForBlocks;

    // Unit selection
    Block __selectedBlock = null;
    public Block SelectedBlcok
    {
        get { return __selectedBlock; }
        set
        {
            __selectedBlock = null;

            __selectedBlock = value;
            //UnitSelectionPanel.SetActive(__selectedBlock != null);
            //UpdateSelectionIndicator();
        }
    }

    void Start()
    {
        Update_CurrentFunc = Update_DetectModeStart;

        jengaGame = GameObject.FindObjectOfType<JengaGame>();
    }

    void Update()
    {
        blockUnderMouse = MouseToBlock();
        Update_CurrentFunc();
    }


    void Update_DetectModeStart()
    {
        // Check here(?) to see if we are over a UI element,
        // if so -- ignore mouse clicks and such.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // TODO: Do we want to ignore ALL GUI objects?  Consider
            // things like unit health bars, resource icons, etc...
            // Although, if those are set to NotInteractive or Not Block
            // Raycasts, maybe this will return false for them anyway.
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Left mouse button just went down.
            // This doesn't do anything by itself, really.
            Debug.Log("MOUSE DOWN");
            SelectedBlcok = blockUnderMouse;
            if (SelectedBlcok != null)
            {
                Update_CurrentFunc = BlockMovement;
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("MOUSE UP -- click!");




        }
        else if (Input.GetMouseButton(0))
        {
            // Left button is being held down AND the mouse moved? That's a camera drag!
            if (SelectedBlcok != null)
            {
                Update_CurrentFunc = BlockMovement;
            }
            else
            {

                Update_CurrentFunc = CameraDrag;
            }

            Update_CurrentFunc();
        }

    }


    public void CameraDrag()
    {


        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Cancelling camera drag.");
            CancelUpdateFunc();
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        _rotationY -= mouseX;
        _rotationX += mouseY;

        // Apply clamping for x rotation 
        _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

        // Apply damping between rotation changes
        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        transform.localEulerAngles = _currentRotation;

        // Substract forward vector of the GameObject to point its forward vector to the target
        transform.position = _target.position - transform.forward * _distanceFromTarget;
    }

    public void BlockMovement()
    {
        Rigidbody rb = jengaGame.blockToGameObject[SelectedBlcok].transform.GetComponent<Rigidbody>();

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Cancelling camera drag.");
            CancelUpdateFunc();
            rb.useGravity = true;
            //rb.isKinematic = false;
            rb.freezeRotation = false;
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        /*jengaGame.blockToGameObject[SelectedBlcok].transform.GetComponent<Rigidbody>().position.x -= mouseX;
        _rotationX += mouseY;
*/

        

        Vector3 forwardMove = transform.right * 5 * mouseX;
        Vector3 horizontalMove = transform.up * 5 * mouseY;
        rb.MovePosition(rb.position + forwardMove + horizontalMove);
        rb.useGravity = false;
        //rb.isKinematic = true;

        rb.freezeRotation = true;
    }

    Block MouseToBlock()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        int layerMask = LayerIDForBlocks.value;

        if (Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, layerMask))
        {
            // Something got hit
            //Debug.Log( hitInfo.collider.name );

            // The collider is a child of the "correct" game object that we want.
            GameObject blockGO = hitInfo.rigidbody.gameObject;

            return jengaGame.GetBlockFromGameObject(blockGO);
        }

        //Debug.Log("Found nothing.");
        return null;
    }

    public void CancelUpdateFunc()
    {
        Update_CurrentFunc = Update_DetectModeStart;

        
    }
}
