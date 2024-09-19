using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{

    public float PosX, Posz;
    public float range;
    [SerializeField]
    int HeightLimit;
  public float ShakeTime;
    Vector3 OriginalPosition;
    Vector3 velocity = Vector3.zero;
    Vector3 newLocal = new Vector3();

    public static CameraSystem Instance;
    
   
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;   
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.LeftShift)) range *= 4;
        //if (Input.GetKeyUp(KeyCode.LeftShift)) range /= 4;
       
    }

   public IEnumerator ScreenShake(float Timer)
    {
        OriginalPosition = Camera.main.gameObject.transform.localPosition;
        while (Timer > 0)
        {
            Camera.main.gameObject.transform.localPosition = OriginalPosition + (Random.insideUnitSphere * ShakeTime);
            Timer -= Time.deltaTime;
            yield return null;
        }
        Camera.main.gameObject.transform.localPosition = OriginalPosition;
        
    }

    void FixedUpdate()
    {
    //Normalises the screen width and height so that 
    // the position of the mouse cursor is consistent
    // regardless of the screen size.
        PosX = (Input.mousePosition.x / Screen.width / 2 - 0.25f) * range;
        Posz = (Input.mousePosition.y / Screen.height / 2 - 0.25f) * range;
       
        if(PosX <= 2.5f && PosX >= -2.5f  && Posz >= -2.5f && Posz <=2.5f) transform.localPosition = new Vector3(PosX, HeightLimit, Posz);
        //print(PosX);
    }
}
