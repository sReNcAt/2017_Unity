using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakePyramid : MonoBehaviour {

    const float CUBE_DIST = 0.1f ;
    const int MIN_NUM = 5 ;
    const int MAX_NUM = 10 ;

	public int position = 0; // 가운데 정렬을위한 변수

    public InputField inputNum;
    public InputField inputChar;
    public Text textLog;
    public Text textResult;

    public GameObject oneCube;

    private GameObject[,] cubeArray = new GameObject[(MAX_NUM + 1) * 2, MAX_NUM + 1];

    private void addLogData (string strLog)
    {
        textLog.text += strLog + "\n";
    }

    private int getInputedNum ()
    {
        string strInputedData = inputNum.text;
        int iRet = -1;

        strInputedData = strInputedData.Trim();

        try
        {
            iRet = int.Parse(strInputedData);
        }
        catch (Exception e)
        {
            addLogData(e.Message);
            addLogData("반드시 " + MIN_NUM.ToString () + " ~ " + MAX_NUM.ToString () + " 사이의 숫자를 입력해 주세요.");
            inputNum.text = "";

            return -1;
        }

        if (iRet < 5 || iRet > 10)
        {
            addLogData("입력하신 데이타가 " + MIN_NUM.ToString() + " ~ " + MAX_NUM.ToString() + " 사이의 숫자가 아닙니다.\n다시 입력해 주세요.");
            inputNum.text = "";

            return -1;
        }
        else
        {
            addLogData("입력받은 숫자는 '" + iRet.ToString() + "' 입니다.");

            if (iRet % 2 == 0)
            {
                addLogData("입력하신 숫자가 짝수이기 때문에 +1을 합니다.");
                iRet++;
            }
        }
		position = iRet/2; //가운데 정렬을위해 입력받은 숫자값/2를 변수에 저장
        return iRet;
    }

    private char getInputedChar()
    {
        string strInputedData = inputChar.text;

        strInputedData = strInputedData.Trim();

        if (strInputedData.Length == 1)
        {
            addLogData("입력받은 문자는 '" + strInputedData + "' 입니다.");
            return strInputedData[0];
        }
        else
        {
            addLogData("입력하신 문자는 공백이 없이 하나의 문자여야 합니다.\n다시 입력해 주세요.");
            inputChar.text = "";

            return ' ';
        }
    }

    private void initCubeArray ()
    {
        for (int i = 0; i < (MAX_NUM + 1) * 2; i++)
        {
            for (int j = 0; j < MAX_NUM + 1; j++)
            {
                if (cubeArray[i, j] != null)
                {
                    Destroy(cubeArray[i, j]);
                    cubeArray[i, j] = null;
                }
            }
        }
    }

    private void showCubePyramid ()
    {
        //for (int i = 0; i < (MAX_NUM + 1) * 2; i++)
		for (int i = 0; i < (MAX_NUM + 1); i++)

		{
            for (int j = 0; j < MAX_NUM + 1; j++)
            {
                if (cubeArray[i, j] != null)
                {
					//Vector3 cubePos = new Vector3(j * 1.0f-position, i * 1.0f-position, 0);
					Vector3 cubePos = new Vector3(j-position * 1.0f, i-position * 1.0f, 0);
                    //가운데 정렬을 위하여 벡터값을 변경 -position을 안넣을경우 시점중간부터 생성됨
                    cubeArray[i, j].transform.position = cubePos;

                    cubeArray[i, j].SetActive(true);
                }
            }
        }
    }

    private void drawPyramid (int iCol, char ch)
    {
        int iOneLoopCount = (iCol + 1) / 2;

        for (int i=0; i<iOneLoopCount*2; i++)
        {
            int iCharCount = 0;
            int iPreSpaceCount = 0;

            if (i < iOneLoopCount)
            {
                iPreSpaceCount = iCol - i - (iCol / 2) - 1;
            }
            else
            {
                iPreSpaceCount = i + (iCol / 2) - iCol;
            }

            for (int j=0; j<iCol; j++)
            {
                if (i < iOneLoopCount)
                {
                    if (iCharCount >= i * 2 + 1)
                        break;
                }
                else
                {
                    if (iCharCount >= iCol - (i - iOneLoopCount) * 2)
                        break;
                }

                if (j < iPreSpaceCount)
                {
                    textResult.text += "  ";
                }
                else
                {
                    textResult.text += ch;
                    iCharCount++;
                    
                    cubeArray[i, j] = Instantiate(oneCube);
                }
            }

            textResult.text += "\n";
        }
    }

    public void onBtnRun ()
    {
        textLog.text = textResult.text = "";

        int iInputedNum = getInputedNum();
        char chInputedData;

        if (iInputedNum >= MIN_NUM && iInputedNum <= MAX_NUM+1)
        {
            chInputedData = getInputedChar();

            if (chInputedData != ' ')
            {
                initCubeArray();
                //drawPyramid(iInputedNum, chInputedData);
				drawPyramid2(iInputedNum, chInputedData);

				showCubePyramid();
            }
        }
    }


	private void drawPyramid2 (int iCol, char ch)
	{
		/*
        *  x는 0부터 시작
        *  y는 입력받은 숫자-1 (배열크기)
        * x와 y가 같을경우 X자의 중심이 되므로 같을경우 same 논리값을 True로 변경
        * same 논리값이 false일 경우 x는 1씩증가 y는 1씩 감소
        * same 논리값이 true일 경우 x는 다시 1씩감소 y는 1씩증가
        *
        */
		int x = 0; //X모양 출력을 위한 변수
		int y = iCol-1; //X모양 출력을 위한 변수
		Boolean same = false; //X모양 출력을 위한 변수
		for (int i=0; i<iCol; i++){
			for (int j = 0; j < iCol; j++) {
				if (j == x || j==y||j==0||i==0||i==iCol-1||j==iCol-1) {
                    //x는 0부터 증가했다 감소하는 변수
                    //y는 입력값부터 감소했다 증가하는 변수
                    //j==0은 네모 모양의 좌측
                    //i==0은 네모 모양의 상단
                    //i==icol-1은 네모 모양의 하단
                    //j==icol-1은 네모 모양의 우측
					textResult.text += ch;
					cubeArray[i, j] = Instantiate(oneCube);
				} else {
					textResult.text += "  ";
				}
			}
			if (same == false) {
				x++;
				y--;
                //X모양의 위쪽부분을 생성하기위해 변수값을 조절
			} else {
				x--;
				y++;
                //X모양의 아랫쪽 부분을 생성하기위해 변수값을 조절
			}
			if (x == y) {
				same = true;
                //증가,감소를 나누기위한 논리값을 변경
			}
			textResult.text += "\n";
		}
        
	}
    /*
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    */
}
