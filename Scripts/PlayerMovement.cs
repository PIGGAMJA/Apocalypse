using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour {

    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도
    private bool isGroundOnGrass = false; // (풀 위에 있는지)구분을 위한 변수

    public AudioClip[] walkFootStepClips; // 걷는 소리
    public AudioClip[] runFootStepClips; // 뛰는 소리
    public AudioClip[] grassWalkFootStepClips; // 풀 위에서 걷는 소리
    public AudioClip[] grassRunFootStepClips; // 풀 위에서 뛰는 소리

    private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

    private void Start()
    {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();
    }

    // FixedUpdate는 물리 갱신 주기(기본값 0.02초)에 맞춰 실행됨
    private void FixedUpdate() 
    {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행

        Move();

        Rotate();

        // 입력값에 따라 애니메이터의 Move 파라미터값 변경
        playerAnimator.SetFloat("Move", playerInput.move);
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move() 
    {
        // 상대적으로 이동할 거리 계산
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;

        // 리지드바디를 이용해 게임오브젝트 위치 변경
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);

        // 물리처리를 무시하는 사고를 방지하기 위해 Rigidbody.position에 할당
    }

    // 입력값에 따라 캐릭터를 좌우로 회전
    private void Rotate() 
    {
        // 사용자 입력에 따라 한 프레임 동안 회전할 각도를 저장하는 변수 turn
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;

        // 리지드바디를 이용해 게임 오브젝트 회전 변경
        playerRigidbody.rotation =
            playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);

        // 물리처리를 무시하는 사고를 방지하기 위해 Rigidbody.rotation에 할당
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Grass"))
        {
            isGroundOnGrass = true;
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Grass"))
        {
            isGroundOnGrass = true;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (isGroundOnGrass && col.gameObject.layer == LayerMask.NameToLayer("Grass"))
        {
            isGroundOnGrass = false;
        }
    }

    private void WalkFootStep()
    {
        if (isGroundOnGrass)
        {
            playerAudioPlayer.PlayOneShot(grassWalkFootStepClips[Random.Range(0, grassWalkFootStepClips.Length)]);
        }
        else
        {
            playerAudioPlayer.PlayOneShot(walkFootStepClips[Random.Range(0, walkFootStepClips.Length)]);
        }
    }

    private void RunFootStep()
    {
        if (isGroundOnGrass)
        {
            playerAudioPlayer.PlayOneShot(grassRunFootStepClips[Random.Range(0, grassRunFootStepClips.Length)]);
        }
        else
        {
            playerAudioPlayer.PlayOneShot(runFootStepClips[Random.Range(0, runFootStepClips.Length)]);
        }
    }
}