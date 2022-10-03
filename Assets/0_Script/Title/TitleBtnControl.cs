using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum eTitleState
{
    None,
    SelectCharacter,
    Option,
    Explain
}

public class TitleBtnControl : MonoBehaviour
{
    [SerializeField] GameObject SelectCharObj, OptionObj, ExplainObj;
    [SerializeField] GameObject ExplainsPanel, ExplainPrev, ExplainNext;

    int explainIdx = 0;
    eTitleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = eTitleState.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (OptionObj == null && state == eTitleState.Option)
            state = eTitleState.None;
    }

    public void OnClickStartBtn()
    {
        if (state != eTitleState.None)
            return;

        SelectCharObj.SetActive(true);
        state = eTitleState.SelectCharacter;

    }

    public void OnClickOptionBtn()
    {
        if (state != eTitleState.None)
            return;

        OptionObj = Instantiate(Resources.Load("OptionWnd") as GameObject, transform.parent);
        state = eTitleState.Option;

    }

    public void OnClickExplainBtn()
    {
        if (state != eTitleState.None)
            return;

        ExplainObj.SetActive(true);
        

        state = eTitleState.Explain;
    }

    public void OnClickExplainPrevArrowBtn()
    {
        ExplainsPanel.transform.GetChild(explainIdx).gameObject.SetActive(false);
        ExplainNext.SetActive(true);
        if (--explainIdx == 0)
            ExplainPrev.SetActive(false);
        else
            ExplainPrev.SetActive(true);

        ExplainsPanel.transform.GetChild(explainIdx).gameObject.SetActive(true);
    }

    public void OnClickExplainNextArrowBtn()
    {
        ExplainsPanel.transform.GetChild(explainIdx).gameObject.SetActive(false);

        ExplainPrev.SetActive(true);

        if (++explainIdx == ExplainsPanel.transform.childCount-1)
            ExplainNext.SetActive(false);
        else
            ExplainNext.SetActive(true);

        ExplainsPanel.transform.GetChild(explainIdx).gameObject.SetActive(true);
    }

    public void OnClickExplainExitBtn()
    {
        ExplainsPanel.transform.GetChild(0).gameObject.SetActive(true);
        ExplainsPanel.transform.GetChild(1).gameObject.SetActive(false);
        ExplainsPanel.transform.GetChild(2).gameObject.SetActive(false);

        ExplainPrev.SetActive(false);
        ExplainNext.SetActive(true);
        explainIdx = 0;

        state = eTitleState.None;
        ExplainObj.SetActive(false);
    }


    public void OnCharacterClickBackBtn()
    {
        SelectCharObj.SetActive(false);
        state = eTitleState.None;
    }

    public void OnOptionClickConfirmBtn()
    {
        OptionObj.SetActive(false);
        state = eTitleState.None;
    }

    public void OnClickExitBtn()
    {
        if (state != eTitleState.None)
            return;

        Application.Quit();
    }
}
