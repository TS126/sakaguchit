using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : Navigation
{
    public enum IsGround
    {
        Water,
        Sand,
        Bridge  
    }
    public float InputSum;
    private IsGround isGround;
    public float MaxRaycastRange = 30f;
    public ParticleSystem stepRpar;
    public ParticleSystem stepLpar;
    public ParticleSystem SandCloudpar;
    public ParticleSystem BubblePar;
    public AudioSource NormalBGM;
    public AudioSource SurfBGM;
    private PlayerSE playerSE;
    public float explosionRadius;
    public float expPower;
    private DisplayGravity displayGravity;
    private Animator animator;
    private CameraMovement cameraMovement;
    List<GameObject> capEnemy = new List<GameObject>();
    [SerializeField]
    private GameObject shockwave;
    [SerializeField]
    private Transform PlayerCenter;
    [SerializeField]
    private GameObject SurfBoard;
    [SerializeField]
    private List<GameObject> NavigationPoints = new List<GameObject>();
    private float altitude;
    private bool jumping;
    private bool floating = false;
    private bool isDead = false;
    [SerializeField]
    private LayerMask hitroad;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float yokoSpeed;
    [SerializeField]
    public float upspeed;
    [SerializeField]
    public float downspeed;
    [SerializeField]
    public float glidspeed;
    [SerializeField]
    public float highest;

    public Vector3 FirstPosition;
    public Vector3 SecondPosition;
    public int JumpNum;

    public float BoostItemTime;
    public float BoostPerSecond;
    public float DecelerationPerSecond;
    public float StartSpeedValue;
    public float DecelerationPerShockWave;
    private float NowSpeedValue;
    public float MaxForwardAtHorizontalInput;

    private float VerticalSpeed;
    private bool turned;
    private float StartTime;
    private float nofloortime;
    public float StandGirlHeight;
    public float TurnGirlHeight;
    private float GetDistance(GameObject a)
    {
        return (this.transform.position - a.transform.position).magnitude;
    }

    private Vector3 GetForward()
    {
        return (SecondPosition - FirstPosition).normalized;
        /*
        if (GetDistance(NavigationPoints[0]) > GetDistance(NavigationPoints[1]))
        {
            NavigationPoints.RemoveAt(0);
        }
        return (NavigationPoints[1].transform.position - NavigationPoints[0].transform.position).normalized;
        */
    }

    // Use this for initialization
    void Start()
    {
        InputSum = 0f;
        JumpNum = 0;
        animator = GetComponent<Animator>();
        altitude = 0f;
        jumping = false;
        turned = false;
        cameraMovement = Camera.main.GetComponent<CameraMovement>();
        FirstPosition = this.transform.position;
        SecondPosition = this.transform.position + this.transform.forward;
        playerSE = GetComponent<PlayerSE>();
        isFinishInthisFrame = false;
        StartTime = Time.time;
        nofloortime = -0.1f;
        displayGravity = FindObjectOfType<DisplayGravity>();
        isUnderConvex = false;
        GameManager.CollectNum = 1;
        NowSpeedValue = StartSpeedValue;
        isOnPlane = false;
        isGround = IsGround.Water;
        stepRpar.Stop();
        stepLpar.Stop();
        GetBGMs();
        BubblePar.Stop();
        BubblePar.gameObject.SetActive(false);
        SandCloudpar.gameObject.SetActive(false);
        //Application.targetFrameRate = 40;
    }
    private GameObject BObject;
    private Vector3 Bpos;
    private bool isUnderConvex;
    public bool isOnPlane;
    private void isOnConvexfunc(RaycastHit hit)
    {
        if (hit.collider.tag == "Convex")
        {
            isUnderConvex = true;
            Vector3 vec = this.transform.position + this.transform.up * 5f;
            //cameraMovement.AddNavigationPoint(vec);
            cameraMovement.OnConvexUpdate(this.transform.rotation, vec, vec + this.transform.forward);
        }
        else
        {
            isUnderConvex = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        //デバグ用
        makeGameOverfromPlayerInput();
        if (Input.GetKeyDown(KeyCode.N))
        {
            ClearedChallenge();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.CollectNum |= 2;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameObject d = GameObject.FindObjectOfType<GoalObject>().gameObject;
            this.transform.position = d.transform.position;
            return;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            NowSpeedValue *= 2;
        }
        float nowSpeedValue = GetForwardValue() * speed;
        this.transform.position += this.transform.forward * Time.deltaTime * nowSpeedValue;
        if (!floating)
        {
            float HorizontalInput = GetHorizontalInput();
            this.transform.position += transform.right * Time.deltaTime * HorizontalInput * speed * (isOnPlane ? 0.66f : yokoSpeed);
        }
        RaycastHit hit;
        // Transformの真下の地形の法線を調べる
        if (Physics.Raycast(
           this.transform.position + this.transform.up * 3f,
                -transform.up,
                out hit,
            MaxRaycastRange, hitroad))
        {
            updateIsGround(hit);
            SandCloud();
            isOnPlane = hit.collider.GetComponent<BoxCollider>() != null;
            isOnConvexfunc(hit);
            if (nofloortime > 0f)
            {
                altitude = (this.transform.position - hit.point).magnitude;
            }
            isUnderConvex = hit.collider.tag == "Convex";
            nofloortime = -0.1f;
            // 傾きの差を求める
            //if (!floating)
            //{
            Quaternion q = Quaternion.FromToRotation(
                               transform.up,
                               hit.normal);
            // 自分を回転させる
            transform.rotation *= q;
            //}
            if (isJump())
            {
                if (altitude < 0.01f)
                {
                    JumpNum += (jumping == false) ? 1 : 0;
                    VerticalSpeed = upspeed;
                    jumping = true;
                }
                else if (!jumping)
                {
                    VerticalSpeed = Mathf.Lerp(VerticalSpeed, -glidspeed, 0.1f);
                }
            }
            else
            {
                jumping = false;
                VerticalSpeed -= downspeed * Time.deltaTime * 5f;
            }
            if (altitude < 0.01f)
            {
                VerticalSpeed = Mathf.Max(0f, VerticalSpeed);
            }
            else
            {
                VerticalSpeed = Mathf.Max(-downspeed * 2f, VerticalSpeed);
            }
            altitude = Mathf.Max(VerticalSpeed * Time.deltaTime + altitude, 0f);
            if (isJump() && altitude > highest && jumping)
            {
                if (iscanturning())
                {
                    hit = turning();
                }
                else
                {
                    jumping = false;
                    turned = true;
                }
            }
            if (floating && altitude < 0.01f && turned)
            {
                turned = false;
                makeShockwave();
                playShockwaveSound();
                DecelerationInShockWave();
            }
            floating = altitude > 0.01f;
            this.transform.position = hit.point + this.transform.up * altitude;
            //if (floating == false)
            ForwardUpdate(hit);
            if (hit.collider.gameObject == BObject)
            {
                this.transform.position += hit.collider.transform.position - Bpos;
            }
            Bpos = hit.collider.transform.position;
            BObject = hit.collider.gameObject;
        }
        else
        {
            BObject = null;
            if (!jumping)
            {
                VerticalSpeed -= downspeed * Time.deltaTime * 5f;
                VerticalSpeed = Mathf.Max(-downspeed * 2f, VerticalSpeed);
            }
            altitude += VerticalSpeed;
            if (isJump() && altitude > highest && jumping)
            {
                if (iscanturning())
                {
                    hit = turning();
                }
                else
                {
                    jumping = false;
                    turned = true;
                }
            }
            nofloortime += Time.deltaTime;
            this.transform.position += this.transform.up * VerticalSpeed * Time.deltaTime;
            if (nofloortime > 2f)
            {
                StartCoroutine(Gameover());
            }
        }
        UpdateRotateAnimation();
    }
    public GameObject TurnObject;
    private Quaternion startRotation;
    private float nowRotate;
    private bool isTurnMotion = false;
    private void UpdateRotateAnimation()
    {
        if (altitude > 2f)
        {
            if (!isTurnMotion)
            {
                animator.SetBool("Sit", true);
                isTurnMotion = true;
                startRotation = TurnObject.transform.localRotation;
            }
            TurnObject.transform.Rotate(0f, (nowRotate = nowRotate - 1440f * Time.deltaTime) * Time.timeScale, 0f, Space.Self);
            this.GetComponent<CapsuleCollider>().height = TurnGirlHeight;
        }
        else
        {
            if (isTurnMotion)
            {
                animator.SetBool("Sit", false);
                isTurnMotion = false;
                TurnObject.transform.localRotation = startRotation;
            }
            this.GetComponent<CapsuleCollider>().height = StandGirlHeight;
        }
    }
    private void updateIsGround(RaycastHit hit)
    {
        if(hit.collider.GetComponent<BoxCollider>()!=null){
            isGround = IsGround.Bridge;
            return;
        }
        if (hit.collider.tag == "Sand")
        {
            isGround = IsGround.Sand;
            return;
        }
        isGround = IsGround.Water;
        return;
    }
    private float GetForwardValue()
    {
        float res = 1f;
        if (isGround == IsGround.Sand)
        {
            res = 0.5f;
        }

        if (NowSpeedValue == 1f)
        {
            return 1f * res;
        }
        else if (NowSpeedValue < 1f)
        {
            NowSpeedValue = Mathf.Min(1f, NowSpeedValue + BoostPerSecond * Time.deltaTime);
        }
        else
        {
            NowSpeedValue = Mathf.Max(1f, NowSpeedValue - DecelerationPerSecond * Time.deltaTime);
        }
        return NowSpeedValue * res;
    }
    private void DecelerationInShockWave()
    {
        NowSpeedValue = Mathf.Max(1f / 3f, NowSpeedValue - DecelerationPerShockWave);
    }
    private Coroutine boostCoroutine;
    public void GetBoostItem(float num = 1f / 3f)
    {
        //NowSpeedValue += num;
        if (boostCoroutine != null)
        {
            StopCoroutine(boostCoroutine);
            boostCoroutine = null;
        }
        boostCoroutine = StartCoroutine(OnBoostFloor());
    }

    public IEnumerator OnBoostFloor()
    {
        float startTime = Time.time;
        StartCoroutine(ChangeFieldOfViewByBoost(75f, 0.2f));
        animator.SetBool("InBoost", true);
        SurfBoard.SetActive(true);
        NormalBGM.Pause();
        SurfBGM.UnPause();
        while (true)
        {
            if (Time.time - startTime > BoostItemTime)
            {
                break;
            }
            NowSpeedValue = 1.5f;
            yield return null;
        }
		StartCoroutine(ChangeFieldOfViewByBoost(60f, 0.2f));
        animator.SetBool("InBoost", false);
        SurfBoard.SetActive(false);
        NormalBGM.UnPause();
        SurfBGM.time = 0f;
        SurfBGM.Pause();
        boostCoroutine = null;
    }
    public IEnumerator ChangeFieldOfViewByBoost(float needValue, float time)
    {
        float startTime = Time.time;
        float startView = Camera.main.fieldOfView;
        while (true)
        {
            if (Time.time - startTime > time)
            {
                break;
            }
            Camera.main.fieldOfView = Mathf.Lerp(startView, needValue, (Time.time - startTime) / time);
            yield return null;
        }
        Camera.main.fieldOfView = needValue;
    }
    private bool isJump()
    {
        if (isDead)
        {
            return false;
        }
#if UNITY_EDITOR
        return isBothinput();
#elif UNITY_IOS
        return isSingleTupping();
#elif UNITY_ANDROID
        return isSingleTupping();
#endif
        return false;
    }
    private bool isBothinput()
    {
        if (jumping)
        {
            return true;
        }
        int num = 0;
        num |= Input.GetKey(KeyCode.A) ? 1 : 0;
        num |= Input.GetKey(KeyCode.LeftArrow) ? 1 : 0;
        num |= Input.GetKey(KeyCode.D) ? 2 : 0;
        num |= Input.GetKey(KeyCode.RightArrow) ? 2 : 0;
        Touch[] myTouches = Input.touches;
        foreach (var i in myTouches)
        {
            if (i.position.x - Screen.width / 2f > 0f)
            {
                num |= 1;
            }
            else
            {
                num |= 2;
            }
        }
        return num == 3;
    }

    private bool isSingleTupping()
    {
        return Input.touches.Length > 0 || jumping;
    }
    private float GetHorizontalInput()
    {
        float Horizontal;
        //return GetAccSpeed ();
#if UNITY_EDITOR
        Horizontal = GetHorizontal();
#elif UNITY_IOS
		Horizontal = GetAccSpeed();
#elif UNITY_ANDROID
        Horizontal = GetAccSpeed();
#endif
        InputSum += Horizontal * Time.deltaTime;
        return Horizontal;
    }
    private float GetHorizontal()
    {
        float res = 0;
        res = Input.GetAxis("Horizontal");
        Touch[] myTouches = Input.touches;
        foreach (var i in myTouches)
        {
            if (i.position.x - Screen.width / 2f > 0f)
            {
                res += 1f;
            }
            else
            {
                res -= 1f;
            }
        }
        return res;
    }
    private float bspeed = 0f;
    private bool isturning = false;
    private float GetAccSpeed()
    {
        Quaternion res = new Quaternion();
        Vector2 acc = new Vector2();
        Vector2 pcc = new Vector2();
        acc = new Vector2(Input.acceleration.x, Input.acceleration.y).normalized;
        //acc = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")).normalized;
        if (isUnderConvex)
        {
            pcc = -Vector2.up;
        }
        else
        {
            pcc = new Vector2(Vector3.Dot(this.transform.up, -Camera.main.transform.right), Vector3.Dot(this.transform.up, Camera.main.transform.up)).normalized;
        }
        Vector2 re = new Vector2();
        re.x = acc.x * pcc.x - acc.y * pcc.y;
        re.y = acc.x * pcc.y + acc.y * pcc.x;
        /*
        res.x = -acc.x;
        res.y = -acc.y;
        Quaternion d = this.transform.rotation;
        res = d * res;
        */
        float resspeed = Mathf.Clamp(Mathf.Atan2(re.y, re.x), -1, 1);
        resspeed = Mathf.Lerp(bspeed, resspeed, 0.4f);
        bspeed = resspeed;
        return resspeed;
    }
    private void makeGameOverfromPlayerInput()
    {
        bool flag = false;
#if UNITY_EDITOR
        flag = Input.GetKeyDown(KeyCode.G);
#elif UNITY_IOS
        flag = isWantGameOverInIOS();
#endif
        if (flag)
        {
            StartCoroutine(Gameover());
        }
    }
    private bool isWantGameOverInIOS()
    {
        return false;
        //return Input.touches.Length >= 4;
    }
    private bool iscanturning()
    {
        return Physics.Raycast(PlayerCenter.transform.position, this.transform.up, MaxRaycastRange, hitroad);
    }
    private RaycastHit turning()
    {
        Camera.main.GetComponent<CameraMovement>().TurnCamera();
        isturning = !isturning;
        VerticalSpeed = -downspeed;
        this.transform.Rotate(0f, 0f, 180f, Space.Self);
        jumping = false;
        RaycastHit nhit;
        Physics.Raycast(this.transform.position, -transform.up, out nhit, MaxRaycastRange, hitroad);
        altitude = (this.transform.position - nhit.point).magnitude;
        turned = true;
        if (displayGravity != null)
        {
            displayGravity.TurnGravity();
        }
        return nhit;
    }

    private void makeShockwave()
    {

        var d = Instantiate(shockwave, this.transform.position, this.transform.rotation) as GameObject;
        d.transform.position += this.transform.forward * NowSpeedValue * speed * 0.3f;
        cameraMovement.Shake(0.25f, 1f);
        /*
        Collider[] targets = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider obj in targets)
        {
            Obstacle obstacle = obj.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                obj.GetComponent<Rigidbody>().AddExplosionForce(expPower, transform.position, explosionRadius);
                obstacle.GetExplosion();
            }
        }
        */
    }
    private void playShockwaveSound()
    {
        playerSE.PlayExplosion();
    }

    private void playDeathSound()
    {
        playerSE.PlayDeath();
    }

    private void playCollectItemSound()
    {
        playerSE.PlayCollectItem();
    }

    private void playBoostSound()
    {
        playerSE.PlayBoost();
    }

    private void ForwardUpdate(RaycastHit hit)
    {
        Physics.Raycast(this.transform.position + this.transform.up * 1f, -transform.up, out hit, MaxRaycastRange, hitroad);
        Vector3 right = Vector3.Cross(hit.normal, GetForward());
        Vector3 myForward = Vector3.Cross(right, hit.normal).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(myForward, hit.normal);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, 0.9f);
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Garter")
        {
            this.transform.position += other.transform.up * 0.5f;
        }
        passCameraDistance(other.gameObject);
        if (other.tag == "CollectItem")
        {
            GameManager.CollectNum |= 2;
            Destroy(other.gameObject);
            playCollectItemSound();
            return;
        }
        if (other.tag == "BoostItem")
        {
            GetBoostItem();
            Destroy(other.gameObject);
            playBoostSound();
            return;
        }
        if (other.tag == "Turn")
        {
            this.transform.position += this.transform.up * 10f;
            turning();
            other.GetComponent<Collider>().enabled = false;
        }
        if (other.tag == "Obstacle")
        {
            if (isActiveObstacle(other))
            {
                StartCoroutine(Gameover());
            }
        }
        GameManager gameManager = GameManager.First; ;
        if (other.tag == "Goal" && !isFinishInthisFrame)
        {
            //gameManager.ClickMask.GetComponent<Animator>().SetBool("Whiteout", true);
            GameManager.First.IsClear = true;
            StartCoroutine(CollisionGoal());
            isFinishInthisFrame = true;
            passClearTimeToGameManager();
        }
        if (other.tag == "MovePoint")
        {

            if (!isInList(getList, other.gameObject) && checkNextMovePoint(other.gameObject))
            {
                getList.Add(other.gameObject);
                if (getList.Count >= 10)
                {
                    getList[0].gameObject.SetActive(true);
                    getList.RemoveAt(0);
                }

                cameraMovement.AddNavigationPoint(other.gameObject);
                FirstPosition = SecondPosition;
                SecondPosition = other.transform.position;
                other.gameObject.SetActive(false);
            }
        }
    }

    private bool checkNextMovePoint(GameObject target)
    {
        Vector3 v1 = (SecondPosition - FirstPosition).normalized;
        Vector3 v2 = (target.transform.position - SecondPosition).normalized;
        return Vector3.Dot(v1, v2) > 0f;
    }
    private void passCameraDistance(GameObject other)
    {
        ChangeCameraDistance changeCameraDistance = other.GetComponent<ChangeCameraDistance>();
        if (changeCameraDistance == null)
        {
            return;
        }
        Camera.main.GetComponent<CameraMovement>().ChangeCameraDistance(changeCameraDistance.CameraDistance);
    }
    private bool isFinishInthisFrame;
    private void passClearTimeToGameManager()
    {
        GameManager.ClearTime = Time.time - StartTime;
    }
    private bool isInList(List<GameObject> list, GameObject gameObject)
    {
        foreach (var i in list)
        {
            if (i == gameObject)
            {
                return true;
            }
        }
        return false;
    }
    private List<GameObject> getList = new List<GameObject>();
    private bool isActiveObstacle(Collider other)
    {
        Obstacle obstacle = other.GetComponent<Obstacle>();
        if (obstacle == null)
        {
            return true;
        }
        else
        {
            return !obstacle.isbreak;
        }
    }

    private IEnumerator CollisionGoal()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Result");
    }

    private IEnumerator Gameover()
    {
        if (!isDead && !isFinishInthisFrame)
        {
            playDeathSound();
            BubblePar.gameObject.SetActive(true);
            BubblePar.Play();
            ClearMesh(this.gameObject);
            Camera.main.GetComponent<CameraMovement>().enabled = false;
            //this.transform.position += -transform.forward;
            isDead = true;
            animator.SetTrigger("Taore");
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    private void ClearMesh(GameObject target)
    {
        foreach (Transform d in target.transform)
        {
        if (d.GetComponent<SkinnedMeshRenderer>() != null)
            {
                d.GetComponent<SkinnedMeshRenderer>().enabled = false;
            }
            ClearMesh(d.gameObject);
        }
    }
    void StepR()
    {
        if (isGround != IsGround.Water)
        {
            return;
        }
        SandCloudpar.gameObject.SetActive(false);
        stepRpar.Play();
        stepLpar.Stop();
    }

    void StepL()
    {
        if (isGround != IsGround.Water)
        {
            return;
        }
        SandCloudpar.gameObject.SetActive(false);
        stepLpar.Play();
        stepRpar.Stop();
    }
    public void ClearedChallenge()
    {
        GameManager.CollectNum |= 4;
    }
    void SandCloud()
    {
        if (isGround != IsGround.Sand)
        {
            SandCloudpar.gameObject.SetActive(false);
            return;
        }
        if (stepLpar.isPlaying)
        {
            stepLpar.Stop();
        }
        if (stepRpar.isPlaying)
        {
            stepRpar.Stop();
        }
        if (!SandCloudpar.isPlaying)
        {
            SandCloudpar.Play();
        }
        SandCloudpar.gameObject.SetActive(true);
    }

    private void GetBGMs()
    {
        NormalBGM = ObjectManager.NormalBGM;
        SurfBGM = ObjectManager.SurfBGM;
        NormalBGM.Play();
        SurfBGM.Play();
        SurfBGM.Pause();
    }
}
