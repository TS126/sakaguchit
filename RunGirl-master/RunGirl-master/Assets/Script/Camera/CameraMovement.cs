using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraMovement : Navigation
{
    [SerializeField]
    private Transform PlayerCenter;
    private List<GameObject> CameraPositions;
    private int CameraPlace;
    [SerializeField]
    private float CameraDistance;
    private float NextCameraDistance;
    private float defaultCameraDistance;
    private Vector3 Offset;
    public Vector3 FirstPosition;
    public Vector3 SecondPosition;
    private PlayerMovement playerMovement;
    private bool isTurning = false;

    private bool OutCameraDistanceInLiner()
    {
        Vector3 pos1 = FirstPosition;
        Vector3 pos2 = SecondPosition;
        Vector3 arrow = (pos2 - pos1).normalized;
        float DistancefromPlayer = Vector3.Dot(PlayerCenter.transform.position - pos1, arrow);
        float Distance = (pos2 - pos1).magnitude;
        return DistancefromPlayer - Distance > CameraDistance;
    }

    private Vector3 GetCameraPositioninLine()
    {
        Vector3 pos1 = FirstPosition;
        Vector3 pos2 = SecondPosition;
        Vector3 arrow = (pos2 - pos1).normalized;
        float DistancefromPlayer = Vector3.Dot(PlayerCenter.transform.position - pos1, arrow);
        return (DistancefromPlayer - CameraDistance) * arrow + pos1;
    }
    private Vector3 GetCameraPosition()
    {
        /*
        if (CameraPositions.Count <= 2)
        {
            CameraPositions.AddRange(GetNavigationPoints());
        }
        while (OutCameraDistanceInLiner())
        {
            CameraPositions.RemoveAt(0);
        }
        return GetCameraPositioninLine();
        */
        Vector3 pos1 = FirstPosition;
        Vector3 pos2 = SecondPosition;
        Vector3 arrow = (pos2 - pos1).normalized;
        float DistancefromPlayer = Vector3.Dot(PlayerCenter.transform.position - pos1, arrow);
        return (DistancefromPlayer - CameraDistance) * arrow + pos1;
    }
    private float GetDistance(GameObject a)
    {
        return (this.transform.position - a.transform.position).magnitude;
    }

    void Start()
    {
        //CameraPositions = new List<GameObject>();
        //CameraPositions.AddRange(GetNavigationPoints());
        //CameraPlace = 0;
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>().GetComponent<PlayerMovement>();
        Offset = Vector3.zero;
        FirstPosition = this.transform.position;
        SecondPosition = this.transform.position + this.transform.forward;
        NextCameraDistance = CameraDistance;
        wasOnConvex = false;
        defaultCameraDistance = CameraDistance;
        this.transform.position = PlayerCenter.transform.position - this.transform.forward * CameraDistance;
    }
    private Vector3 GetCameraforward()
    {

        Vector3 arrow1 = (PlayerCenter.transform.position - this.transform.position).normalized;
        Vector3 arrow2 = (SecondPosition - FirstPosition).normalized;
        Vector3 res = Vector3.Lerp(arrow1, arrow2, 0.5f);
        return res;
    }
    private bool wasOnConvex;
    public void OnConvexUpdate(Quaternion forward, Vector3 Point1, Vector3 Point2)
    {
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, forward, 0.1f);
        SecondPosition = Point2;
        FirstPosition = Point1;
        wasOnConvex = true;
    }
    private void Update()
    {
        updateRotation();
        updatePosition();
        UpdateCameraDistance();
        updateNextCameraDistance();
    }
    private void updateNextCameraDistance()
    {
        Vector3 forward = (SecondPosition - FirstPosition).normalized;
        Vector3 Distance = PlayerCenter.transform.position - this.transform.position;
        NextCameraDistance = CameraDistance * defaultCameraDistance / (Distance.magnitude * Vector3.Dot(Distance.normalized, forward));
    }
    private void updateRotation()
    {
        if (wasOnConvex)
        {
            wasOnConvex = false;
            return;
        }
        Vector3 CameraPlayerPosition = GetCameraforward();
        Quaternion forward;
        Vector3 rotateTarget;
        if (!playerMovement.isOnPlane || isTurning)
        {
            rotateTarget = this.transform.right;
        }
        else
        {
            if (Vector3.Dot(PlayerCenter.up, this.transform.up) > 0f)
                rotateTarget = PlayerCenter.right;
            else
                rotateTarget = -PlayerCenter.right;
        }
        forward = Quaternion.LookRotation(CameraPlayerPosition, Vector3.Cross(CameraPlayerPosition, rotateTarget));
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, forward, 0.1f);
    }

    private void updatePosition()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, GetCameraPosition(), 0.1f) + Offset;
    }
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        var pos = transform.localPosition;

        var elapsed = 0f;

        while (elapsed < duration)
        {
            var x = Random.Range(-1f, 1f) * magnitude;
            var y = Random.Range(-1f, 1f) * magnitude;
            var z = Random.Range(-1f, 1f) * magnitude;
            Offset = new Vector3(x, y, z);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
        Offset = Vector3.zero;
    }

    public void AddNavigationPoint(GameObject Navigation)
    {
        FirstPosition = SecondPosition;
        SecondPosition = Navigation.transform.position;
    }
    public void AddNavigationPoint(Vector3 Point)
    {
        FirstPosition = SecondPosition;
        SecondPosition = Point;
    }
    private void UpdateCameraDistance()
    {
        CameraDistance = Mathf.Lerp(CameraDistance, NextCameraDistance, 0.2f);
    }

    public void TurnCamera()
    {
        isTurning = true;
        StartCoroutine(turnCamera());
    }

    private IEnumerator turnCamera()
    {
        float bnum = 0f;
        while (true)
        {
            float nextnum = Mathf.Lerp(bnum, 180f, 0.2f);
            this.transform.Rotate(0f, 0f, nextnum - bnum, Space.Self);
            bnum = nextnum;
            if (180f - nextnum < 0.1f)
            {
                isTurning = false;
                break;
            }
            else
            {
                yield return null;
            }
        }
    }

    public void ChangeCameraDistance(float nextDistance)
    {
        NextCameraDistance = nextDistance;
    }
}