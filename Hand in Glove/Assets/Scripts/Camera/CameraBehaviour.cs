using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class CameraBehaviour : MonoBehaviour {
    public List<Transform> objsToFollow;
    private Camera myCam;
    [SerializeField]
    [Tooltip("how much distance there is between player and camera border")]
    private float cameraOffset;
    [SerializeField]
    private float minCameraSize = 10f;
    [SerializeField]
    private bool doSmoothFollow = true;
    [SerializeField]
    [Range(0f,1f)]
    private float smoothSpeed;
    private float usedSmoothSpeed;
    private EndDoor endDoor;
    private List<Transform> slideObjsToFollow;
    [Header("Camera Slide")]
    [SerializeField]
    private bool doCameraSlide = false;
    [HideInInspector]
    public bool finishedCameraSlide;
    [SerializeField]
    private float levelViewDuration;
    [SerializeField]
    private float startSize;
    [SerializeField]
    [Tooltip("Time to scroll from one collectable to the next one.")]
    private float slideDuration = 2f;
    [SerializeField]
    [Tooltip("Time the camera needs to get to the normal camera size.")]
    private float startToSmallSizeDuration = 1f;
    [SerializeField]
    private float lookAtCollectableDuration = 1f;
    [SerializeField]
    private float cameraPlanSize = 35f;
    private int slideObjCount;
    private bool[] wantToSkip;
    public bool inPlanningPhase;
    public bool xCompensation;
    public bool yCompensation;

    // Use this for initialization
    void Awake () {
        myCam = GetComponent<Camera>();
        objsToFollow = new List<Transform>();
        slideObjsToFollow = new List<Transform>();
        wantToSkip = new bool[4] { false, false, false, false };
        inPlanningPhase = false;
        usedSmoothSpeed = smoothSpeed;
    }
    private void Start()
    {
        endDoor = FindObjectOfType<EndDoor>();
        if (doCameraSlide)
        {
            if (endDoor != null)
                StartCoroutine(CameraLevelView());
        }
        else FinishCameraSlide();
    }

    IEnumerator CameraLevelView()
    {
        foreach (Collectable c in endDoor.collectables)
            slideObjsToFollow.Add(c.transform);
        slideObjCount = 0;
        transform.position = new Vector3(0f, 0f, -10f);
        myCam.orthographicSize = startSize;
        yield return new WaitForSeconds(levelViewDuration);
        StartCoroutine(CameraSlide());
        StartCoroutine(ChangeCameraSize());
    }

    IEnumerator ChangeCameraSize()
    {
        float startTime = Time.time;
        float endTime = Time.time + startToSmallSizeDuration;
        while (Time.time <= endTime)
        {
            myCam.orthographicSize = Mathf.Lerp(startSize, minCameraSize, (Time.time - startTime) / (endTime - startTime));
            yield return null;
        }
    }
    IEnumerator CameraSlide()
    {
        Vector2 dir = (Vector2)slideObjsToFollow[slideObjCount].position - (Vector2)transform.position;
        //slideObjCount++;
        while (true)
        {
            if(((Vector2)transform.position - (Vector2)slideObjsToFollow[slideObjCount].position).magnitude <= dir.magnitude * Time.deltaTime / slideDuration)
            {
                transform.position = slideObjsToFollow[slideObjCount].position + new Vector3(0f, 0f, -10);
                slideObjCount++;
                yield return new WaitForSeconds(lookAtCollectableDuration);
                if (slideObjCount >= slideObjsToFollow.Count) break;
                dir = (Vector2)slideObjsToFollow[slideObjCount].position - (Vector2)transform.position;
            }
            transform.Translate(dir * Time.deltaTime / slideDuration);
            yield return null;
        }
        FinishCameraSlide();
    }

    private void PlanningPhase()
    {
        Time.timeScale = 0f;
        GameManager.paused = true;
        myCam.orthographicSize = cameraPlanSize;
        transform.position = new Vector3(0f, 0f, -10f);
        inPlanningPhase = true;
    }

    private void FinishCameraSlide()
    {
        finishedCameraSlide = true;
        StopAllCoroutines();
        PlayerSpawner pSpawner = FindObjectOfType<PlayerSpawner>();
        if (pSpawner != null)
        {
            if (pSpawner.spawnOnStart)
                pSpawner.Spawn();
        }
        wantToSkip = new bool[4] { false, false, false, false };
        PlanningPhase();
    }
    public void AddObjToFollow(Transform objToFollow)
    {
        objsToFollow.Add(objToFollow);
    }
    public void RemoveObjToFollow(Transform objToFollow)
    {
        objsToFollow.Remove(objToFollow);
    }
    // Update is called once per frame
    void LateUpdate () {
        if (!finishedCameraSlide || inPlanningPhase)
        {
            int count = 0;
            foreach(InputInformation i in GameManager.inputInformation)
            {
                int keyBoardInputNr = 0;
                if (GameManager.useKeyboard)
                {
                    if (i.inputNr > 4)
                        keyBoardInputNr = i.inputNr - GameManager.keyBoardPlayersAmount - 4 + GameManager.playerAmount;
                    else keyBoardInputNr = i.inputNr;
                }
                if (Input.GetButtonDown("Fire1_P" + i.inputNr) && !wantToSkip[keyBoardInputNr - 1])
                {
                    wantToSkip[keyBoardInputNr - 1] = true;
                    GameObject.Find("P" + keyBoardInputNr).GetComponentInChildren<Text>().color = Color.green;
                }
            }
            foreach (bool b in wantToSkip)
                if (b) count++;
            if (Input.GetKeyDown(KeyCode.Escape) || count == GameManager.inputInformation.Count)
            {
                if (inPlanningPhase)
                {
                    Time.timeScale = 1f;
                    GameManager.paused = false;
                    inPlanningPhase = false;
                    Destroy(GameObject.Find("ReadyCheck"));
                    FindObjectOfType<Timer>().active = true;
                }
                else
                    FinishCameraSlide();
            }
        }

        if (objsToFollow.Count <= 0 || GameManager.paused) return;
        
        Vector3 pos = new Vector2();
        int counter = 0;
        float oSize = cameraPlanSize;
        if (Input.GetButton("ZoomOut"))
        {
            pos = Vector3.down;
        }
        else
        {
            
            for (int i = objsToFollow.Count - 1; i>= 0; i--)  //Get the average position of all obj that have to be in the camera view
            {
                if (objsToFollow[i] == null)
                {
                    objsToFollow.RemoveAt(i);
                    continue;
                }

                pos += objsToFollow[i].position;
                counter++;
            }
            pos /= (float)counter;
            Vector2 deltaPos = new Vector2();
            foreach (Transform objToFollow in objsToFollow) //Get the biggest x and y distance to the current pos of the camera to calculate the orthographic size
            {
                if (Mathf.Abs(deltaPos.x) < Mathf.Abs(objToFollow.position.x - pos.x)) deltaPos.x = (objToFollow.position.x - pos.x);
                if (Mathf.Abs(deltaPos.y) < Mathf.Abs(objToFollow.position.y - pos.y)) deltaPos.y = (objToFollow.position.y - pos.y);
            }
            //Decide which directions has to be take into account
            //orthographic size sets the half-size of the y axis of the camera
            //therefore x has to be converted in the respecting y value
            oSize = Mathf.Abs(deltaPos.x * Screen.height / Screen.width) > Mathf.Abs(deltaPos.y) ?
                                                             Mathf.Abs(deltaPos.x * Screen.height / Screen.width) :
                                                             Mathf.Abs(deltaPos.y);
            oSize += cameraOffset;
            if (yCompensation)
            {
                if (pos.y >= 0f)
                {
                    if (pos.y + oSize > cameraPlanSize)
                    {
                        float difference = ((pos.y + oSize) - cameraPlanSize);
                        pos.y -= difference / 2f;
                        oSize -= difference / 2f;
                    }
                }
                else
                {
                    if (pos.y - oSize < -cameraPlanSize)
                    {
                        float difference = ((pos.y - oSize) + cameraPlanSize);
                        pos.y -= difference / 2f;
                        oSize += difference / 2f;
                    }
                }
            }

            if (xCompensation)
            {
                float t = (oSize - minCameraSize)  / (cameraPlanSize - minCameraSize);
                pos.x -= Mathf.Lerp(0f, pos.x, t);
            }
        }
        if (doSmoothFollow)
        {
            float tmp = Mathf.Lerp(myCam.orthographicSize, oSize, smoothSpeed * 10f * Time.deltaTime);
            myCam.orthographicSize = tmp;
        }
        else 
            myCam.orthographicSize = oSize;
        if (myCam.orthographicSize < minCameraSize) myCam.orthographicSize = minCameraSize;
        Vector3 desiredPos = new Vector3(pos.x, pos.y + 0.5f, -10f);
        if(doSmoothFollow)
        {
            desiredPos = Vector3.Lerp(transform.position, desiredPos, usedSmoothSpeed * 10f * Time.deltaTime);
            desiredPos = desiredPos.RoundToNearestPixel();
        }

        transform.position = desiredPos;
	}
    public void SetSmoothSpeed(float _smoothSpeed)
    {
        usedSmoothSpeed = _smoothSpeed;
    }
    public void ResetSmoothSpeed()
    {
        usedSmoothSpeed = smoothSpeed;
    }
}
