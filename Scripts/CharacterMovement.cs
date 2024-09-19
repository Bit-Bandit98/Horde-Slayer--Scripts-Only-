using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    Rigidbody Rigi;
    [SerializeField]
    Texture2D Reticle;
    float hori, veri;
   

    [SerializeField]
    Animator PlayerAnime;

    [SerializeField]
    public AudioLowPassFilter MuffleFilter;


    public static CharacterMovement Instance;

    [SerializeField]
    GameObject CameraScript, Character;


    Vector3 CurrentVelocity;

    Camera MainCamera;
    // Start is called before the first frame update
    
        //Character looks in the direction of the mouse
    void lookAtMouse()
    {
        Ray cameraRay = MainCamera.ScreenPointToRay(Input.mousePosition);
        //Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        //Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
       
        RaycastHit hit;
        //Check if a raycast hits a piece of geometry or an enemy.
       if( Physics.Raycast(cameraRay, out hit, Mathf.Infinity, ~(1 << 10)))
        {

            Vector3 PointToLook = cameraRay.GetPoint(Vector3.Distance(cameraRay.origin, hit.point));
           
          //  Debug.DrawRay(cameraRay.origin, PointToLook, Color.blue);
            
            if (Input.GetKeyDown(KeyCode.Space)) { print(PointToLook); }
            Character.transform.LookAt(new Vector3(PointToLook.x, transform.position.y, PointToLook.z));
        }
        // If the previous raycast doesnt hit anything, say at the edge of the world, then it still behaves normally.
      //    else if (groundPlane.Raycast(cameraRay, out rayLength))
        //{
          //  Vector3 PointToLook = cameraRay.GetPoint(rayLength);
            // Debug.DrawRay(cameraRay.origin, PointToLook, Color.red);
            // Character.transform.LookAt(new Vector3(PointToLook.x, transform.position.y, PointToLook.z));
        //}
    }
    
    void Awake()
    {
        Instance = this;
        Cursor.SetCursor(Reticle, Vector2.zero, CursorMode.Auto);


        // CameraScript.GetComponent<CameraSystem>();
        MainCamera = Camera.main;
        MuffleFilter = GameObject.Find("Music Player").GetComponent<AudioLowPassFilter>();
        MuffleFilter.enabled = false;
            Time.timeScale = 1;
        //speed = BaseSpeed;
        //transform.position = Camera.main.ScreenToWorldPoint(new Vector3((Input.mousePosition.x/Screen.width) / 2, (Input.mousePosition.y / Screen.height) / 2, Camera.main.nearClipPlane));
    }

    private void Update()
    {
        PauseGame();

     //   PlayerAnime.SetFloat("Speed", Rigi.velocity.magnitude);


    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (PlayerChar.Instance.IsAlive && !GM.GlobalGameManager.GameIsPaused) {

            if (CameraSystem.Instance.PosX <= 2.5f && CameraSystem.Instance.PosX >= -2.5f && CameraSystem.Instance.Posz >= -2.5f && CameraSystem.Instance.Posz <= 2.5f) { lookAtMouse(); }
        MovementFunction();
         //   Cursor.SetCursor(Reticle, Vector2.zero, CursorMode.Auto);

        }

    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            uiManager.PlayerUIState.PauseTextDisplay(true);
            GM.GlobalGameManager.GameIsPaused = true;
            uiManager.PlayerUIState.ShowPauseMenu(true);

            //Adds a muffle filter to the music.
            MuffleFilter.enabled = GM.GlobalGameManager.GameIsPaused;
            Time.timeScale = 0;
            return;
        }
    }
    void PauseGame()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape) && GM.GlobalGameManager.GameIsPaused == false)
        {
            uiManager.PlayerUIState.PauseTextDisplay(true);
            GM.GlobalGameManager.GameIsPaused = true;
            uiManager.PlayerUIState.ShowPauseMenu(true);
            
            //Adds a muffle filter to the music.
            MuffleFilter.enabled = GM.GlobalGameManager.GameIsPaused;
            Time.timeScale = 0;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && GM.GlobalGameManager.GameIsPaused == true)
        {
            uiManager.PlayerUIState.PauseTextDisplay(false);
            GM.GlobalGameManager.GameIsPaused = false;
            uiManager.PlayerUIState.ShowPauseMenu(false);
            MuffleFilter.enabled = GM.GlobalGameManager.GameIsPaused;
            Time.timeScale = 1;
            return;
        }
    }

    float CharacterAngle;
    float AngleDotProductY;
    float AngleDotProductX;
    bool[] MovingDirection = new bool[8];

    private void MovementFunction()
    {
        hori = Input.GetAxis("Horizontal") * PlayerChar.Instance.BaseSpeed;
        veri = Input.GetAxis("Vertical") * PlayerChar.Instance.BaseSpeed;

        bool isMoving = (hori > 0 || veri > 0 || hori < 0 || veri < 0);

        CurrentVelocity = new Vector3(hori, Rigi.velocity.y, veri);
        Rigi.velocity = transform.TransformDirection(CurrentVelocity);

        if (isMoving)
        {
            PlayerAnime.SetBool("IsMoving", true);
        } else { PlayerAnime.SetBool("IsMoving", false); }

        //Returns the angle between the Character's forward vector, and the character's velocity
         CharacterAngle = Vector3.Angle(Rigi.velocity, Character.transform.forward);
        //Grabs the dot product between the player's velocity, Character's right vector and forward vector.
         AngleDotProductY = (Vector3.Dot(Vector3.Normalize(Character.transform.forward), Vector3.Normalize(Rigi.velocity)));
         AngleDotProductX =  (Vector3.Dot(Vector3.Normalize(Character.transform.right), Vector3.Normalize(Rigi.velocity)));

        //These angles are gathered and are constantly compared in order to dictate which direction the character should move in 
        //regards to animation. 
       
        //Forward
        MovingDirection[0] = (CharacterAngle > 0 && CharacterAngle < 22.5f && AngleDotProductY > 0 && AngleDotProductY < 1);
        //Forward Right
        MovingDirection[1] = (CharacterAngle > 22.5f && CharacterAngle < 67.5f && AngleDotProductX > 0 && AngleDotProductX < 1);
        //Forward Left
        MovingDirection[2] = (CharacterAngle > 22.5f && CharacterAngle < 67.5f && AngleDotProductX < 0 && AngleDotProductX > -1);
        //Right
        MovingDirection[3] = (CharacterAngle > 67.5f && CharacterAngle < 112.5f && AngleDotProductX < 1 && 0.7f < AngleDotProductX);
        //Left
        MovingDirection[4] = (CharacterAngle > 67.5f && CharacterAngle < 112.5f && AngleDotProductX > -1 && -0.7f > AngleDotProductX);
        //Back Right
        MovingDirection[5] = (CharacterAngle > 112.5f && CharacterAngle < 157.5f && AngleDotProductX > 0 && AngleDotProductX < 1);
        //Back Left
        MovingDirection[6] = (CharacterAngle > 112.5f && CharacterAngle < 157.5f && AngleDotProductX < 0 && AngleDotProductX > -1);
        //Back
        MovingDirection[7] = (CharacterAngle < 180 && CharacterAngle > 157.5f && AngleDotProductY > -1 && -0.7f > AngleDotProductY);

      
        //Sets the animation states to have the character animate properly.

        PlayerAnime.SetBool("RunningForward", MovingDirection[0]);
        PlayerAnime.SetBool("RunningForwardRight", MovingDirection[1]);
        PlayerAnime.SetBool("RunningForwardLeft", MovingDirection[2]);
        PlayerAnime.SetBool("RunningRight", MovingDirection[3]);
        PlayerAnime.SetBool("RunningLeft", MovingDirection[4]);
        PlayerAnime.SetBool("RunningBackRight", MovingDirection[5]);
        PlayerAnime.SetBool("RunningBackLeft", MovingDirection[6]);
        PlayerAnime.SetBool("RunningBack", MovingDirection[7]);

      
        
        

    }
}
