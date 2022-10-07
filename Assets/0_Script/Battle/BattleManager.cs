using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 카메라 이동
유저의 마법 활성화


몬스터, 소환사 프리펩 개발
활동(이동, 공격, 마법, 소환) 애니메이션 적용

 */
public enum eBattleType
{
    Normal=8,
    //Elite=10,
    Boss=10//14
}

public enum eBattleState
{
    FindUnitTurn,
    HighLightUnit,
    SelectMove,
    MoveAni,

    SelectActRotate,
    ReadyWait,
    ReadyAttack,
    ReadyMagic,
    MagicAttack,
    MagicAssist,
    SelectSummon,
    ActAni,

    BattleEnd
}


public class BattleManager : Singleton<BattleManager>
{
    //public static BattleManager _instance;

    public int _stage = 1;  // 맵 씬에서 진행중인 스테이지 변수 얻기

    [SerializeField] GameObject _menuAct, _directionWay;

    [SerializeField] Canvas ca;

    [SerializeField] Transform _camPos, _camRot;

    eBattleState _currentState;
    eBattleType _battleType;
    EnumList.eMagicList _readyMagic;
    EnumList.eStateUnit _readyState;

    Vector3 defCamPos = new Vector3(15, 0, -15);

    Unit _playUnit;   // 플레이할 유닛 
    Tile unitTile;

    bool isOpenWnd = false;
    bool isBuff = false;
    float delayTime = 0f;

    public void SetIsBuff(bool buff) => isBuff = buff;
    public bool GetIsBuff() => isBuff;


    //private void Awake()
    //{
    //    //_instance = this;
    //}

    private void Start()
    {
        GenerateBattle(MapManager.readyBattleType);
    }

    private void Update()
    {
        if (GUIScript._instance.ExistPauseWnd())
            return;

        switch (_currentState)
        {
            case eBattleState.FindUnitTurn:
                FindUnitTurn();
                break;
            case eBattleState.HighLightUnit:
                HighlightUnit();
                break;
            case eBattleState.SelectMove:
                SelectMove();
                break;

            case eBattleState.MoveAni:
                MoveAni();
                break;

            case eBattleState.SelectActRotate:
                UnitRotate();
                break;
            case eBattleState.ReadyWait:
                ReadyWait();
                break;
            case eBattleState.ReadyAttack:
                ReadyAttack();
                break;
            case eBattleState.MagicAttack:
                MagicAttack();
                break;
            case eBattleState.MagicAssist:
                MagicAssist();
                break;
            case eBattleState.SelectSummon:
                SelectSummon();
                break;
            case eBattleState.ActAni:
                ActAni();
                break;

            case eBattleState.BattleEnd:
                BattleEnd();
                break;

        }

        if(_playUnit._PlayerTeam)
            MoveCam();

        ZoomCam();
    }

    void ZoomCam()
    {
        Camera.main.fieldOfView += Input.mouseScrollDelta.y;
        if (Camera.main.fieldOfView < 40)
            Camera.main.fieldOfView = 40;

        if (Camera.main.fieldOfView > 80)
            Camera.main.fieldOfView = 80;
    }

    private void ActAni()
    {
        
    }

    private void BattleEnd()
    {
        GUIScript._instance.OpenWnd(EnumList.eUIWnd.ResultWnd);
    }

    private void MagicAssist()
    {
        if (_playUnit._PlayerTeam)
        {

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) 
                {
                    Tile tempTile = hit.transform.GetComponentInParent<Tile>();

                    if(tempTile.TileStatus == EnumList.eTileHighlightStatus.Magic)
                    {
                        BoardManager._instance.ResetListTile();
                        _playUnit.GetComponent<Summoner>().UseMagic(tempTile.GetUnitObject());
                        tempTile.GetUnitObject().GetComponent<Unit>().AddState(_readyState);
                        _currentState = eBattleState.ActAni;

                        GUI_ActLog._instance.MagicAssistLog(_playUnit, tempTile.GetUnitObject().GetComponent<Unit>());
                    }
                }
            }

            if (Input.GetMouseButtonDown(1) && !IsPointerOverUIObject())
            {
                BoardManager._instance.ResetListTile();
                GUIScript._instance.OpenWnd(EnumList.eUIWnd.MagicWnd);
            }
        }
        
        
    }

    private void MagicAttack()
    {

        if (_playUnit._PlayerTeam)
        {

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Tile tempTile = hit.transform.GetComponentInParent<Tile>();

                    if (tempTile.TileStatus == EnumList.eTileHighlightStatus.Magic)
                    {
                        BoardManager._instance.ResetListTile();
                        _playUnit.GetComponent<Monster>().UseMagic(tempTile.GetUnitObject());
                        _currentState = eBattleState.ActAni;

                        GUI_ActLog._instance.MagicAttackLog(_playUnit, tempTile.GetUnitObject().GetComponent<Unit>());
                    }
                }
            }

            if (Input.GetMouseButtonDown(1) && !IsPointerOverUIObject())
            {
                BoardManager._instance.ResetListTile();
                _directionWay.SetActive(true);
                GUIScript._instance.OpenWnd(EnumList.eUIWnd.MenuActBtns);
                _currentState = eBattleState.SelectActRotate;
            }

        }
        else
        {
            
            Tile tempTile = AutoSelect.SelectTileAttack(_playUnit);
            
            if (tempTile == null )
            {
                _currentState = eBattleState.ReadyWait;
                AutoSelect.ResetTargetTile();
                return;
            }

            GameObject tempObj = tempTile.transform.GetChild(1).gameObject;
            _playUnit.GetComponent<Monster>().UseMagic(tempObj);
            _currentState = eBattleState.ActAni;

            GUI_ActLog._instance.MagicAttackLog(_playUnit, tempTile.GetUnitObject().GetComponent<Unit>());
            AutoSelect.ResetTargetTile();
        }
    }

    // 유닛의 차례 찾기
    void FindUnitTurn()
    {
        if(SpawnUnitManager._instance.EnemyMonsterCount == 0)
        {
            _currentState = eBattleState.BattleEnd;
            return;
        }

        _playUnit = SpawnUnitManager._instance.TurnUnit();
        unitTile = _playUnit.transform.parent.GetComponent<Tile>();
        _currentState = eBattleState.HighLightUnit;
        
    }

    // 플레이할 유닛 포커스
    void HighlightUnit()
    {

        _directionWay.SetActive(false);
        BoardManager._instance.MoveTile(_playUnit, _battleType);
        
        _camPos.eulerAngles = _playUnit._PlayerTeam ? Vector3.zero : new Vector3(0,180,0);
        _camPos.position =
            new Vector3(_playUnit.transform.position.x, 20, _playUnit.transform.position.z) + (_playUnit._PlayerTeam ? defCamPos : -defCamPos);

        _currentState = eBattleState.SelectMove;
    }

    // 이동할 위치 선정
    void SelectMove()
    {
        

        if (_playUnit.GetComponent<Unit>()._PlayerTeam)
        {
            

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Tile"))
                {
                    Tile tempTile = hit.transform.parent.GetComponent<Tile>();

                    if (tempTile.TileStatus == EnumList.eTileHighlightStatus.Move)
                    {
                        _playUnit.transform.SetParent(tempTile.transform);
                        _playUnit.transform.LookAt(_playUnit.transform.parent);
                        _playUnit.GetComponent<Unit>().Move();
                        
                        BoardManager._instance.ResetListTile();

                        _camPos.SetParent( _playUnit.transform);
                        

                        _currentState = eBattleState.MoveAni;
                    }
                }

                _camPos.position =
                    new Vector3(_playUnit.transform.position.x, 20, _playUnit.transform.position.z) + (_playUnit._PlayerTeam ? defCamPos : -defCamPos);
            }
        }
        else
        {
            delayTime += Time.deltaTime;

            if (delayTime > 2)
            {
                delayTime = 0f;

                if(_playUnit.CompareTag("Monster") && AutoSelect.CheckTileAttackUnit(_playUnit))
                {
                    _currentState = eBattleState.ReadyAttack;
                    return;
                }

                Tile tempTile = AutoSelect.SelectTileMove(_playUnit);

                _playUnit.transform.SetParent( tempTile != null ? tempTile.transform : _playUnit.transform.parent);
                _playUnit.transform.LookAt(_playUnit.transform.parent);
                _playUnit.GetComponent<Unit>().Move();
                _camPos.SetParent( _playUnit.transform);
                
                _currentState = eBattleState.MoveAni;

            }
        }
    }

    // 유닛의 이동 애니매이션 작동
    void MoveAni()
    {
        if (Vector3.Distance(_playUnit.transform.localPosition, Vector3.zero) > 0.5f)
        {
            _playUnit.transform.localPosition = Vector3.MoveTowards(_playUnit.transform.localPosition, Vector3.zero, Time.deltaTime*10);
        }
        else
        {
            _camPos.SetParent(null);

            _playUnit.transform.localPosition = Vector3.zero;
            _playUnit.GetComponent<Unit>().Idle();

            _directionWay.SetActive(true);
            _directionWay.transform.position = _playUnit.transform.position;

            // 방향 자동 설정            
            SetRotation();

            if(_playUnit._PlayerTeam)
                GUIScript._instance.OpenWnd(EnumList.eUIWnd.MenuActBtns);
            _currentState = eBattleState.SelectActRotate;
        }
    }


    // 이동 후 회전
    void UnitRotate()
    {
        if (_playUnit._PlayerTeam)
        {

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("DirectionWay"))
                    {
                        string tempWay = hit.transform.parent.name;

                        switch (tempWay[tempWay.Length - 1])
                        {
                            case 'N':
                                _playUnit.GetComponent<Unit>().SetWay(EnumList.eUnitWay.N);
                                break;
                            case 'E':
                                _playUnit.GetComponent<Unit>().SetWay(EnumList.eUnitWay.E);
                                break;
                            case 'S':
                                _playUnit.GetComponent<Unit>().SetWay(EnumList.eUnitWay.S);
                                break;
                            case 'W':
                                _playUnit.GetComponent<Unit>().SetWay(EnumList.eUnitWay.W);
                                break;
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(1) && !IsPointerOverUIObject())
            {
                if (isOpenWnd)
                {
                    isOpenWnd = false;
                    return;
                }
                _playUnit.transform.SetParent( unitTile.transform);
                _playUnit.transform.localPosition = Vector3.zero;

                GUIScript._instance.CloseWnd(EnumList.eUIWnd.MenuActBtns);
                _currentState = eBattleState.HighLightUnit;
            }
        }
        else
        {
            _directionWay.SetActive(false);

            int tempInt = UnityEngine.Random.Range(0, 100);
            
            if (_playUnit.CompareTag("Summoner"))
            {
                
                if (tempInt >= 35)
                    _currentState = eBattleState.SelectSummon;
                else
                    _currentState = eBattleState.ReadyWait;


            }
            else
            {

                if (tempInt < 90)
                {
                    if (IsUnitHasMagic() && AutoSelect.CheckTileMagicAttackUnit(_playUnit.GetComponent<Monster>()))
                        _currentState = eBattleState.MagicAttack;
                    else
                    {
                        if (AutoSelect.CheckTileAttackUnit(_playUnit))
                            _currentState = eBattleState.ReadyAttack;
                        else
                            _currentState = eBattleState.ReadyWait;
                    }
                }
                else
                    _currentState = eBattleState.ReadyWait;


            }

            
        }
    }
    private void MoveCam()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _camPos.Translate(Vector3.forward*Time.deltaTime*50);
            _camPos.Translate(Vector3.left * Time.deltaTime*50);
        }

        if (Input.GetKey(KeyCode.S))
        {
            _camPos.Translate(Vector3.back * Time.deltaTime * 50);
            _camPos.Translate(Vector3.right * Time.deltaTime * 50);
        }

        if (Input.GetKey(KeyCode.A))
        {
            _camPos.Translate(Vector3.back * Time.deltaTime * 50);
            _camPos.Translate(Vector3.left * Time.deltaTime * 50);
        }

        if (Input.GetKey(KeyCode.D))
        {
            _camPos.Translate(Vector3.forward * Time.deltaTime * 50);
            _camPos.Translate(Vector3.right * Time.deltaTime * 50);
        }
        
    }

    // 몬스터 소환
    private void SelectSummon()
    {

        if (_playUnit._PlayerTeam)
        {

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Tile tile = hit.transform.parent.GetComponent<Tile>();

                    if (tile.TileStatus == EnumList.eTileHighlightStatus.Magic)
                    {
                        BoardManager._instance.ResetListTile();
                        _playUnit.GetComponent<Unit>().SetWay(tile);
                        _playUnit.GetComponent<Summoner>().Summon(tile);
                        _currentState = eBattleState.ActAni;
                    }

                }
            }

            if (Input.GetMouseButtonDown(1) && !IsPointerOverUIObject())
            {
                BoardManager._instance.ResetListTile();
                GUIScript._instance.OpenWnd(EnumList.eUIWnd.SummonWnd);
                _currentState = eBattleState.SelectActRotate;
            }
        }
        else
        {
            Tile tempTile = AutoSelect.SelectTileSummon(_playUnit);

            if(tempTile==null)
            {
                _currentState = eBattleState.ReadyWait;
                return;
            }

            
            SpawnUnitManager._instance.SetWaitMonster((EnumList.eKindMonster)UnityEngine.Random.Range(0, (int)EnumList.eKindMonster.COUNT));
            _playUnit.GetComponent<Summoner>().Summon(tempTile);
            _currentState = eBattleState.ActAni;
        }
    }

    // 공격할 타일 선택
    private void ReadyAttack()
    {
        _directionWay.SetActive(false);

        if (_playUnit._PlayerTeam)
        {

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Tile tempTile = hit.transform.GetComponentInParent<Tile>();

                    if(tempTile.TileStatus == EnumList.eTileHighlightStatus.Enemy)
                    {
                        _playUnit.transform.LookAt(tempTile.transform.position);
                        SetRotation();
                        _playUnit.GetComponent<Monster>().Attack(tempTile.GetUnitObject());
                        _currentState = eBattleState.ActAni;

                        GUI_ActLog._instance.AttackLog(_playUnit, tempTile.GetUnitObject().GetComponent<Unit>());
                    }

                }
            }

            if (Input.GetMouseButtonDown(1) && !IsPointerOverUIObject())
            {
                BoardManager._instance.ResetListTile();
                GUIScript._instance.OpenWnd(EnumList.eUIWnd.MenuActBtns);
                _directionWay.SetActive(true);
                _currentState = eBattleState.SelectActRotate;

            }
        }
        else
        {

            Tile tempTile = AutoSelect.SelectTileAttack(_playUnit);
            
            if(tempTile == null )
            {
                _currentState = eBattleState.ReadyWait;
                AutoSelect.ResetTargetTile();
                return;
            }
            
            _playUnit.transform.LookAt(tempTile.transform);
            SetRotation();
            _playUnit.GetComponent<Monster>().Attack(tempTile.GetUnitObject());

            
            BoardManager._instance.ResetListTile();
            _currentState = eBattleState.ActAni;

            GUI_ActLog._instance.AttackLog(_playUnit, tempTile.GetUnitObject().GetComponent<Unit>());
            AutoSelect.ResetTargetTile();
        }

    }


    // 배틀 맵 생성
    public void GenerateBattle(eBattleType battleType)
    {
        _battleType = battleType;
        BoardManager._instance.GenerateBoard((int)battleType);
        SpawnUnitManager._instance.SpawnSummoner(battleType);
        _currentState = eBattleState.FindUnitTurn;
    }

    void SetRotation()
    {
        float angle = _playUnit.transform.eulerAngles.y;

        if ((angle >= 315 && angle <= 360) || (angle >= 0 && angle < 45))//N
            _playUnit.GetComponent<Unit>().SetWay(EnumList.eUnitWay.N);

        if (angle >= 45 && angle < 135)//E
            _playUnit.GetComponent<Unit>().SetWay(EnumList.eUnitWay.E);

        if (angle >= 225 && angle < 315)//W
            _playUnit.GetComponent<Unit>().SetWay(EnumList.eUnitWay.W);

        if ((angle >= 135 && angle < 225) || (angle >= -225 && angle < -135))//S
            _playUnit.GetComponent<Unit>().SetWay(EnumList.eUnitWay.S);
    }

    void ReadyWait()
    {
        GUI_ActLog._instance.WaitLog(_playUnit);
        _currentState = eBattleState.FindUnitTurn;
    }

    public void ChangeState(eBattleState state)
    {
        _currentState = state;
    }

    

    public bool IsPlaySummoner()
    {
        return _playUnit._Grade == 5;
    }

    public bool IsUnitHasMagic()
    {
        Monster temp = _playUnit.gameObject.GetComponent<Monster>();

        return temp.Magic != EnumList.eMagicList.None;
            
    }

    public void SetOpenWnd(bool check) => isOpenWnd = check;

    public void SetUnitState(EnumList.eStateUnit state) => _readyState = state;
    public bool GetOpenWnd() => isOpenWnd;

    public Unit PlayUnit => _playUnit;

    public eBattleType BattleType => _battleType;

    public void SetDirectionWay(bool isActive) => _directionWay.SetActive(isActive);
    // 배틀 종료 
    public void EndBattle()
    {
        GUIScript._instance.OpenWnd(EnumList.eUIWnd.ResultWnd);
    }

    bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

}
