using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Coder : MonoBehaviour
{
    // paper
    [SerializeField] private Vector3 posFirstLetter;
    [SerializeField] private Vector3 offsetLetter;

    [SerializeField] private GameObject[] NumPrefs;
    [SerializeField] private Transform paper;
    private string code;

    // pc
    private int currentScreen;
    private int[] screenNums = new int[4]{0, 0, 0, 0};
    [SerializeField] private GameObject[] screenNums_obj = new GameObject[4];
    [SerializeField] private Vector3[] screenPoses;
    [SerializeField] private List<GameObject> NumPrefsPC;
    [SerializeField] private Transform arrow;
    [SerializeField] private Vector3[] arrowPoints;
    [SerializeField] private Animator enterAnim;
    [SerializeField] private Animator[] updownAnims;
    [SerializeField] private GameObject targetFound;
    private bool codeEntered;


    void Start(){
        generateCode();
    }
    void generateCode(){
        code = "";
        for(int i = 0; i < 4; i++){
            int num = Random.Range(0, 10);
            code += num;

            if (NumPrefs[num].activeSelf){
                GameObject numcopy = Instantiate(NumPrefs[num], paper);
                numcopy.transform.localPosition = posFirstLetter + offsetLetter * i;
            }
            else{
                NumPrefs[num].SetActive(true);
                NumPrefs[num].transform.localPosition = posFirstLetter + offsetLetter * i;
            }
        }
    }

    void Update(){
        if (codeEntered) return;

        if (Input.GetKeyUp(KeyCode.W)) buttonUD(true);
        else if (Input.GetKeyUp(KeyCode.S)) buttonUD(false);

        if (Input.GetKeyUp(KeyCode.D)) moveArrow(true);
        else if (Input.GetKeyUp(KeyCode.A)) moveArrow(false);

        arrow.localPosition = Vector3.MoveTowards(arrow.localPosition, arrowPoints[currentScreen], 10 * Time.deltaTime);
    }

    void moveArrow(bool toRight){
        if (toRight) currentScreen++;
        else currentScreen--;

        if (currentScreen >= 4) currentScreen = 0;
        else if (currentScreen < 0) currentScreen = 3;

        if (!enterAnim.enabled) enterAnim.enabled = true;
        else enterAnim.CrossFade("PressUDButtonPC", 0.1f, -1, 0);
        
    }

    void buttonUD(bool isUp){
        if (!updownAnims[isUp ? 0 : 1].enabled) updownAnims[isUp ? 0 : 1].enabled = true;
        else updownAnims[isUp ? 0 : 1].CrossFade("PressUDButtonPC", 0.1f, -1, 0);

        if (isUp){
            screenNums[currentScreen]++;
            if (screenNums[currentScreen] >= 10) screenNums[currentScreen] = 0;
        }
        else{
            screenNums[currentScreen]--;
            if (screenNums[currentScreen] < 0) screenNums[currentScreen] = 9;
        }

        bool ok = true;
        for(int i = 0; i < 4; i++){
            if (int.Parse(code[i].ToString()) != screenNums[i]){
                ok = false;
                break;
            }
        }
        if (ok) correctCode();

        GameObject obj = NumPrefsPC.Where(x => x.name.Contains(screenNums[currentScreen].ToString()) && !x.activeSelf).FirstOrDefault();
        if (obj == null){
            GameObject reference = NumPrefsPC.Where(x => x.name.Contains(screenNums[currentScreen].ToString())).First();
            GameObject newnum = Instantiate(reference, reference.transform.parent);
            newnum.SetActive(false);
            NumPrefsPC.Add(newnum);
            obj = NumPrefsPC.Where(x => x.name.Contains(screenNums[currentScreen].ToString()) && !x.activeSelf).FirstOrDefault();
        }

        if (screenNums_obj[currentScreen] != null) screenNums_obj[currentScreen].SetActive(false);
        screenNums_obj[currentScreen] = obj;
        obj.SetActive(true);
        obj.transform.localPosition = screenPoses[currentScreen];
    }

    void correctCode(){
        if (codeEntered) return;
        codeEntered = true;
        targetFound.SetActive(true);

        // NEXT STAGE
    }
}
