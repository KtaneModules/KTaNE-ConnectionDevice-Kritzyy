using UnityEngine;
using KModkit;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class KritConnectionDev : MonoBehaviour
{
    public KMSelectable BootupBtn, SubmitBtn;
    public KMSelectable WhatsappSelect, DiscordSelect, SkypeSelect;
    public KMSelectable Number1Btn, Number2Btn, Number3Btn, Number4Btn, Number5Btn, Number6Btn, Number7Btn;
    public KMSelectable Letter1Btn, Letter2Btn, Letter3Btn, Letter4Btn, Letter5Btn, Letter6Btn, Letter7Btn;

    public Renderer ScreenMat, TextBlockMat;

    public Texture ScreenOff, ScreenOn, WhatsappBack, DiscordBack, SkypeBack;
    public Texture WhatsappOpen, DiscordOpen, SkypeOpen;
    public Texture WhatsappReceived, DiscordReceived, SkypeReceived;
    public GameObject WhatsappIcon, DiscordIcon, SkypeIcon, BootupBtnObj;
    public KMSelectable ModuleSelectable;

    public TextMesh Number1BtnText, Number2BtnText, Number3BtnText, Number4BtnText, Number5BtnText, Number6BtnText, Number7BtnText;
    public TextMesh Letter1BtnText, Letter2BtnText, Letter3BtnText, Letter4BtnText, Letter5BtnText, Letter6BtnText, Letter7BtnText;
    public TextMesh PCSerialNumber;

    public TextMesh TextMessage;
    public GameObject TextBlock;
    public GameObject ResponseCode;
    public TextMesh MessageReceivedText1;
    public TextMesh MessageReceivedText2;
    public TextMesh ErrorMsgColor;

    public TextMesh WhatsappCode;
    public TextMesh DiscordCode;
    public TextMesh SkypeCode;

    public KMAudio LaptopSFX;


    List<string> NumberSerial = new List<string>
    {
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
    };
    List<string> LetterSerial = new List<string>
    {
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "W"
    };

    private readonly string TwitchHelpMessage = "Type '!{0} boot' to boot up the device. Type '!{0} press letter 1' to press the first letter key (Or '!{0} press number 1 for the first number'), from left to right. Type '!{0} submit' to send the message.";
    public KMSelectable[] ProcessTwitchCommand(string Command)
    {
        Command = Command.ToLowerInvariant().Trim();

        if (Command.Equals("boot"))
        {
            return new[] { BootupBtn };
        }
        else if (Command.Equals("press letter 1"))
        {
            return new[] { Letter1Btn };
        }
        else if (Command.Equals("press letter 2"))
        {
            return new[] { Letter2Btn };
        }
        else if (Command.Equals("press letter 3"))
        {
            return new[] { Letter3Btn };
        }
        else if (Command.Equals("press letter 4"))
        {
            return new[] { Letter4Btn };
        }
        else if (Command.Equals("press letter 5"))
        {
            return new[] { Letter5Btn };
        }
        else if (Command.Equals("press number 1"))
        {
            return new[] { Number1Btn };
        }
        else if (Command.Equals("press number 2"))
        {
            return new[] { Number2Btn };
        }
        else if (Command.Equals("press number 3"))
        {
            return new[] { Number3Btn };
        }
        else if (Command.Equals("press number 4"))
        {
            return new[] { Number4Btn };
        }
        else if (Command.Equals("press number 5"))
        {
            return new[] { Number5Btn };
        }
        else if (Command.Equals("submit"))
        {
            return new[] { SubmitBtn };
        }
        return null;
    }
    public KMBombInfo BombInfo;

    static int moduleIdCounter = 1;
    int moduleId;

    int SerialNum1Gen;
    int SerialNum2Gen;

    int Characters;

    string Number1value;
    string Number2value;
    string Number3value;
    string Number4value;
    string Number5value;
    string Number6value;
    string Number7value;

    string Letter1value;
    string Letter2value;
    string Letter3value;
    string Letter4value;
    string Letter5value;
    string Letter6value;
    string Letter7value;


    List<string> NumberList = new List<string>
    {
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
    };

    List<string> LetterList = new List<string>
    {
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "W", "X", "Y", "Z"
    };

    List<string> EvenLetterList = new List<string>
    {
        "D", "F", "H", "J", "L", "N", "P", "R", "T", "V", "X", "Z"
    };

    List<string> SetButtonLetters = new List<string>();

    List<string> SetButtonNumbers = new List<string>();

    string FirstLetterSerial;

    string SerialNr;

    string Program;

    string KeyPressed;

    public string Message;
    string ErrorMessage;

    string CodeShouldContain;
    string ItemPresent;

    bool TypingActive = false;
    bool TypeCharacter = false;
    bool FirstCharacter = true;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        BootupBtn.OnInteract += Bootup;

        Number1Btn.OnInteract += Number1;
        Number2Btn.OnInteract += Number2;
        Number3Btn.OnInteract += Number3;
        Number4Btn.OnInteract += Number4;
        Number5Btn.OnInteract += Number5;
        Number6Btn.OnInteract += Number6;
        Number7Btn.OnInteract += Number7;

        Letter1Btn.OnInteract += Letter1;
        Letter2Btn.OnInteract += Letter2;
        Letter3Btn.OnInteract += Letter3;
        Letter4Btn.OnInteract += Letter4;
        Letter5Btn.OnInteract += Letter5;
        Letter6Btn.OnInteract += Letter6;
        Letter7Btn.OnInteract += Letter7;
        SubmitBtn.OnInteract += Submit;
        WhatsappSelect.OnInteract += Whatsapp;
        DiscordSelect.OnInteract += Discord;
        SkypeSelect.OnInteract += Skype;
        BootupBtn.ForceSelectionHighlight = true;
    }

    void Start()
    {
        WhatsappIcon.SetActive(false);
        DiscordIcon.SetActive(false);
        SkypeIcon.SetActive(false);
        Init();
    }

    void Init()
    {
        //PC Serial Generator...
        int SerialLtr1Gen = Random.Range(0, 22);
        int SerialLtr2Gen = Random.Range(0, 22);
        int SerialLtr3Gen = Random.Range(0, 22);
        SerialNum1Gen = Random.Range(0, 10);
        SerialNum2Gen = Random.Range(0, 10);

        string serialChar1 = LetterSerial[SerialLtr1Gen];
        string serialChar2 = LetterSerial[SerialLtr2Gen];
        string serialChar3 = LetterSerial[SerialLtr3Gen];
        string serialChar4 = NumberSerial[SerialNum1Gen];
        string serialChar5 = NumberSerial[SerialNum2Gen];

        //...And applying said serial
        SerialNr = serialChar1 + serialChar2 + serialChar3 + serialChar4 + serialChar5;
        PCSerialNumber.text = "KT-A-NE: " + SerialNr;
        CheckWhatsapp();
        Code();
    }

    void CheckWhatsapp() //Here come those rules boys. Whatsapp rules:
    {

        if (SerialNum1Gen + SerialNum2Gen > 8)
        {
            if (SerialNr.Contains("W") || SerialNr.Contains("H") || SerialNr.Contains("A") || SerialNr.Contains("T") || SerialNr.Contains("S") || SerialNr.Contains("A") || SerialNr.Contains("P"))
            {
                Program = "Whatsapp";
                ApplicationAnswer();
            }
            else
            {
                CheckDiscord();
            }
        }
        else
        {
            CheckDiscord();
        }
    }

    void CheckDiscord() //Now Discord:
    {
        if (SerialNr.Contains("D") || SerialNr.Contains("I") || SerialNr.Contains("S") || SerialNr.Contains("C") || SerialNr.Contains("O") || SerialNr.Contains("R"))//Discord rules
        {
            if (BombInfo.GetSerialNumberLetters().Any("DISCORD".Contains))
            {
                Program = "Discord";
                ApplicationAnswer();
            }
            else
            {
                CheckSkype();
            }
        }
        else
        {
            CheckSkype();
        }
    }

    void CheckSkype() //Lastly Skype
    {
        Program = "Skype";
        ApplicationAnswer();
    }

    void ApplicationAnswer()
    {
        Debug.LogFormat("[Connection Device #{0}] The Connection device's Serial Number is {1}.", moduleId, SerialNr);
        Debug.LogFormat("[Connection Device #{0}] The desired application is {1}.", moduleId, Program);
    }

    void ActivateScreen()
    {
        Debug.LogFormat("[Connection Device #{0}] Connection device activated.", moduleId);
        ScreenMat.material.mainTexture = ScreenOn;
        //Enable the icons
        WhatsappIcon.SetActive(true);
        DiscordIcon.SetActive(true);
        SkypeIcon.SetActive(true);
    }


    IEnumerator Transition()
    {
        for (int i = 0; i < 2; i++)
        {
            int TransitTime = i;
            if (TransitTime == 0)
            {
                WhatsappIcon.SetActive(false);
                DiscordIcon.SetActive(false);
                SkypeIcon.SetActive(false);
            }
            else if (TransitTime == 1)
            {
                TypingActive = true;
                TextBlock.SetActive(true);
                if (Program == "Whatsapp")
                {
                    WhatsappApp();
                }
                else if (Program == "Discord")
                {
                    DiscordApp();
                }
                else if (Program == "Skype")
                {
                    SkypeApp();
                }
            }
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    void WhatsappApp() //Opening the program
    {
        TextBlock.transform.Translate(0.01f, 0, -0.0025f);
        TextMessage.transform.Translate(0.01f, -0.0025f, 0);

        TextMessage.color = Color.black;
        TextBlockMat.material.color = Color.black;
        MessageReceivedText1.color = Color.black;
        MessageReceivedText2.color = Color.black;
        ErrorMsgColor.color = Color.black;

        ScreenMat.material.mainTexture = WhatsappOpen;
    }

    void DiscordApp()
    {
        TextBlock.transform.Translate(0.003f, 0.0002f, 0.0005f);
        TextMessage.transform.Translate(0.003f, 0.00075f, -0.0005f);
        TextMessage.color = Color.white;
        TextBlockMat.material.color = Color.white;
        ScreenMat.material.mainTexture = DiscordOpen;
    }

    void SkypeApp()
    {
        TextBlock.transform.Translate(0.006f, 0f, -0.002f);
        TextMessage.transform.Translate(0.006f, -0.0015f, -0.0005f);
        TextMessage.color = Color.white;
        TextBlockMat.material.color = Color.white;
        ScreenMat.material.mainTexture = SkypeOpen;
    }

    void Typing()
    {
        if (Characters <= 5)
        {
            TypeCharacter = true;
            Characters++;
            TextBlock.transform.Translate(0.0026f, 0, 0);
            if (FirstCharacter == true)
            {
                TextBlock.transform.Translate(-0.002f, 0, 0);
            }
            if (KeyPressed == "Num1")
            {
                TextMessage.text += Number1value;
                Message += Number1value;
                Debug.LogFormat("[Connection Device #{0}] Character {1} is a {2}", moduleId, Characters, Number1value);
                FirstCharacter = false;
                TypeCharacter = false;
            }
            else if (KeyPressed == "Num2")
            {
                TextMessage.text += Number2value;
                Message += Number2value;
                Debug.LogFormat("[Connection Device #{0}] Character {1} is a {2}", moduleId, Characters, Number2value);
                FirstCharacter = false;
                TypeCharacter = false;
            }
            else if (KeyPressed == "Num3")
            {
                TextMessage.text += Number3value;
                Message += Number3value;
                Debug.LogFormat("[Connection Device #{0}] Character {1} is a {2}", moduleId, Characters, Number3value);
                TypeCharacter = false;
                FirstCharacter = false;
            }
            else if (KeyPressed == "Num4")
            {
                TextMessage.text += Number4value;
                Message += Number4value;
                Debug.LogFormat("[Connection Device #{0}] Character {1} is a {2}", moduleId, Characters, Number4value);
                TypeCharacter = false;
                FirstCharacter = false;
            }
            else if (KeyPressed == "Num5")
            {
                TextMessage.text += Number5value;
                Message += Number5value;
                Debug.LogFormat("[Connection Device #{0}] Character {1} is a {2}", moduleId, Characters, Number5value);
                TypeCharacter = false;
                FirstCharacter = false;
            }
            else if (KeyPressed == "Num6")
            {
                TextMessage.text += Number6value;
                Message += Number6value;
                Debug.LogFormat("[Connection Device #{0}] Character {1} is a {2}", moduleId, Characters, Number6value);
                TypeCharacter = false;
                FirstCharacter = false;
            }
            else if (KeyPressed == "Num7")
            {
                TextMessage.text += Number7value;
                Message += Number7value;
                Debug.LogFormat("[Connection Device #{0}] Character {1} is a {2}", moduleId, Characters, Number7value);
                TypeCharacter = false;
                FirstCharacter = false;
            }


            else if (KeyPressed == "Ltr1")
            {
                TextMessage.text += Letter1value;
                Message += Letter1value;
                Debug.LogFormat("[Connection Device #{0}] Character {1} is a {2}", moduleId, Characters, Letter1value);
                TypeCharacter = false;
                FirstCharacter = false;
            }
            else if (KeyPressed == "Ltr2")
            {
                TextMessage.text += Letter2value;
                Message += Letter2value;
                Debug.LogFormat("[Connection Device #{0}] Character {1} is a {2}", moduleId, Characters, Letter2value);
                TypeCharacter = false;
                FirstCharacter = false;
            }
            else if (KeyPressed == "Ltr3")
            {
                TextMessage.text += Letter3value;
                Message += Letter3value;
                Debug.LogFormat("[Connection Device #{0}] Character {1} is a {2}", moduleId, Characters, Letter3value);
                TypeCharacter = false;
                FirstCharacter = false;
            }
            else if (KeyPressed == "Ltr4")
            {
                TextMessage.text += Letter4value;
                Message += Letter4value;
                Debug.LogFormat("[Connection Device #{0}] Character {1} is a {2}", moduleId, Characters, Letter4value);
                TypeCharacter = false;
                FirstCharacter = false;
            }
            else if (KeyPressed == "Ltr5")
            {
                TextMessage.text += Letter5value;
                Message += Letter5value;
                Debug.LogFormat("[Connection Device #{0}] Character {1} is a {2}", moduleId, Characters, Letter5value);
                TypeCharacter = false;
                FirstCharacter = false;
            }
            else if (KeyPressed == "Ltr6")
            {
                TextMessage.text += Letter6value;
                Message += Letter6value;
                Debug.LogFormat("[Connection Device #{0}] Character {1} is a {2}", moduleId, Characters, Letter6value);
                TypeCharacter = false;
                FirstCharacter = false;
            }
            else if (KeyPressed == "Ltr7")
            {
                TextMessage.text += Letter7value;
                Message += Letter7value;
                Debug.LogFormat("[Connection Device #{0}] Character {1} is a {2}", moduleId, Characters, Letter7value);
                TypeCharacter = false;
                FirstCharacter = false;
            }
            Debug.LogFormat("[Connection Device #{0}] The message at character {1} is: {2}", moduleId, Characters, Message);
        }
        else
        {
            Debug.LogFormat("[Connection Device #{0}] Your character limit of 6 has been reached", moduleId);
        }
    }

    //The code... And the buttons
    void Code()
    {
        int PositionGen1L;
        int PositionGen2L;
        int PositionGen3L;
        int PositionGen4L;
        int PositionGen5L;
        int PositionGen6L;
        int PositionGen7L;

        int PositionGen1N;
        int PositionGen2N;
        int PositionGen3N;
        int PositionGen4N;
        int PositionGen5N;
        int PositionGen6N;
        int PositionGen7N;

        int LetterGen1;
        int LetterGen2;
        int LetterGen3;
        int LetterGen4;
        int LetterGen5;
        int LetterGen6;
        int LetterGen7;

        int NumberGen1;
        int NumberGen2;
        int NumberGen3;
        int NumberGen4;
        int NumberGen5;
        int NumberGen6;
        int NumberGen7;

        FirstLetterSerial = BombInfo.GetSerialNumberLetters().First().ToString();

        if (Program == "Whatsapp")
        {
            if (BombInfo.GetPortCount(Port.DVI) > 0)
            {
                CodeShouldContain = "DVI";
                ItemPresent = "DVI";
                Debug.LogFormat("[Connection Device #{0}] Port '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("D");
                SetButtonLetters.Add("V");
                SetButtonLetters.Add("I");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 6);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.GetPortCount(Port.Parallel) > 0)
            {
                CodeShouldContain = "PRL";
                ItemPresent = "Parallel";
                Debug.LogFormat("[Connection Device #{0}] Port '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("P");
                SetButtonLetters.Add("R");
                SetButtonLetters.Add("L");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 6);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.GetPortCount(Port.PS2) > 0)
            {
                CodeShouldContain = "PS2";
                ItemPresent = "PS2";
                Debug.LogFormat("[Connection Device #{0}] Port '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("P");
                SetButtonLetters.Add("S");
                SetButtonLetters.Add(FirstLetterSerial);

                Number1value = "2";
                Number1BtnText.text = "2";

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);
                SetButtonLetters.Add(Letter5value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);



                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.GetPortCount(Port.RJ45) > 0)
            {
                CodeShouldContain = "RJ4";
                ItemPresent = "RJ4";
                Debug.LogFormat("[Connection Device #{0}] Port '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("R");
                SetButtonLetters.Add("J");
                SetButtonLetters.Add(FirstLetterSerial);

                Number2value = "4";
                Number2BtnText.text = "4";
                Number6value = "5";
                Number6BtnText.text = "5";

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                LetterGen4 = Random.Range(0, 22);
                Letter4value = LetterList[LetterGen4];
                LetterList.Remove(Letter4value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);
                SetButtonLetters.Add(Letter5value);
                SetButtonLetters.Add(Letter7value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);


                PositionGen1N = Random.Range(0, 6);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.GetPortCount(Port.Serial) > 0)
            {
                CodeShouldContain = "SRL";
                ItemPresent = "Serial";
                Debug.LogFormat("[Connection Device #{0}] Port '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("S");
                SetButtonLetters.Add("R");
                SetButtonLetters.Add("L");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 6);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);


                LogCode();
                return;
            }
            else if (BombInfo.GetPortCount(Port.StereoRCA) > 0)
            {
                CodeShouldContain = "RCA";
                ItemPresent = "Stereo-RCA";
                Debug.LogFormat("[Connection Device #{0}] Port '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("R");
                SetButtonLetters.Add("C");
                SetButtonLetters.Add("A");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 6);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.GetPortCount(Port.ComponentVideo) > 0)
            {
                CodeShouldContain = "CNV";
                ItemPresent = "Component-Video";
                Debug.LogFormat("[Connection Device #{0}] Port '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("C");
                SetButtonLetters.Add("N");
                SetButtonLetters.Add("V");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 6);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.GetPortCount(Port.CompositeVideo) > 0)
            {
                CodeShouldContain = "CSV";
                ItemPresent = "Composite-Video";
                Debug.LogFormat("[Connection Device #{0}] Port '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("C");
                SetButtonLetters.Add("S");
                SetButtonLetters.Add("V");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 6);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.GetPortCount(Port.USB) > 0)
            {
                CodeShouldContain = "USB";
                ItemPresent = "USB";
                Debug.LogFormat("[Connection Device #{0}] Port '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("U");
                SetButtonLetters.Add("S");
                SetButtonLetters.Add("B");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 6);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.GetPortCount(Port.HDMI) > 0)
            {
                CodeShouldContain = "HDM";
                ItemPresent = "HDMI";
                Debug.LogFormat("[Connection Device #{0}] Port '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("H");
                SetButtonLetters.Add("D");
                SetButtonLetters.Add("M");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 6);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.GetPortCount(Port.VGA) > 0)
            {
                CodeShouldContain = "VGA";
                ItemPresent = "VGA";
                Debug.LogFormat("[Connection Device #{0}] Port '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("V");
                SetButtonLetters.Add("G");
                SetButtonLetters.Add("A");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 6);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.GetPortCount(Port.AC) > 0)
            {
                CodeShouldContain = "ACC";
                ItemPresent = "AC";
                Debug.LogFormat("[Connection Device #{0}] Port '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("A");
                SetButtonLetters.Add("C");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                LetterGen4 = Random.Range(0, 22);
                Letter4value = LetterList[LetterGen4];
                LetterList.Remove(Letter4value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);
                SetButtonLetters.Add(Letter5value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 6);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.GetPortCount(Port.PCMCIA) > 0)
            {
                CodeShouldContain = "PCM";
                ItemPresent = "PCMCIA";
                Debug.LogFormat("[Connection Device #{0}] Port '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("P");
                SetButtonLetters.Add("C");
                SetButtonLetters.Add("M");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 6);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else
            {
                CodeShouldContain = "NAN";
                ItemPresent = "Not Availible";

                SetButtonLetters.Add("N");
                SetButtonLetters.Add("A");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen4 = Random.Range(0, 24);
                Letter4value = LetterList[LetterGen4];
                LetterList.Remove(Letter4value);

                LetterGen5 = Random.Range(0, 23);
                Letter5value = LetterList[LetterGen5];
                LetterList.Remove(Letter5value);

                LetterGen6 = Random.Range(0, 22);
                Letter6value = LetterList[LetterGen6];
                LetterList.Remove(Letter6value);

                LetterGen7 = Random.Range(0, 11);
                Letter7value = LetterList[LetterGen7];
                EvenLetterList.Remove(Letter7value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                LetterList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 8);
                Number2value = NumberList[NumberGen2];
                LetterList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 7);
                Number3value = NumberList[NumberGen3];
                LetterList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 6);
                Number4value = NumberList[NumberGen4];
                LetterList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 5);
                Number5value = NumberList[NumberGen5];
                LetterList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 4);
                Number6value = NumberList[NumberGen6];
                LetterList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 3);
                Number7value = NumberList[NumberGen7];
                LetterList.Remove(Number7value);

                SetButtonLetters.Add(Letter4value);
                SetButtonLetters.Add(Letter5value);
                SetButtonLetters.Add(Letter6value);
                SetButtonLetters.Add(Letter7value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 7);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
        }
        else if (Program == "Discord")
        {
            if (BombInfo.IsIndicatorPresent(Indicator.SND))
            {
                CodeShouldContain = "SND";
                ItemPresent = "SND";
                Debug.LogFormat("[Connection Device #{0}] Indicator '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("S");
                SetButtonLetters.Add("N");
                SetButtonLetters.Add("D");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 8);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 7);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 6);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 5);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 4);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 3);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 7);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.IsIndicatorPresent(Indicator.CLR))
            {
                CodeShouldContain = "CLR";
                ItemPresent = "CLR";
                Debug.LogFormat("[Connection Device #{0}] Indicator '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("C");
                SetButtonLetters.Add("L");
                SetButtonLetters.Add("R");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 7);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.IsIndicatorPresent(Indicator.CAR))
            {
                CodeShouldContain = "CAR";
                ItemPresent = "CAR";
                Debug.LogFormat("[Connection Device #{0}] Indicator '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("C");
                SetButtonLetters.Add("A");
                SetButtonLetters.Add("R");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 7);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.IsIndicatorPresent(Indicator.IND))
            {
                CodeShouldContain = "IND";
                ItemPresent = "IND";
                Debug.LogFormat("[Connection Device #{0}] Indicator '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("I");
                SetButtonLetters.Add("N");
                SetButtonLetters.Add("D");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 7);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.IsIndicatorPresent(Indicator.FRQ))
            {
                CodeShouldContain = "FRQ";
                ItemPresent = "FRQ";
                Debug.LogFormat("[Connection Device #{0}] Indicator '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("F");
                SetButtonLetters.Add("R");
                SetButtonLetters.Add("Q");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 7);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.IsIndicatorPresent(Indicator.SIG))
            {
                CodeShouldContain = "SIG";
                ItemPresent = "SIG";
                Debug.LogFormat("[Connection Device #{0}] Indicator '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("S");
                SetButtonLetters.Add("I");
                SetButtonLetters.Add("G");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 7);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.IsIndicatorPresent(Indicator.NSA))
            {
                CodeShouldContain = "NSA";
                ItemPresent = "NSA";
                Debug.LogFormat("[Connection Device #{0}] Indicator '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("N");
                SetButtonLetters.Add("S");
                SetButtonLetters.Add("A");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 7);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.IsIndicatorPresent(Indicator.MSA))
            {
                CodeShouldContain = "MSA";
                ItemPresent = "MSA";
                Debug.LogFormat("[Connection Device #{0}] Indicator '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("M");
                SetButtonLetters.Add("S");
                SetButtonLetters.Add("A");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 8);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 7);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 6);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 5);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 4);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 3);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 6);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.IsIndicatorPresent(Indicator.TRN))
            {
                CodeShouldContain = "TRN";
                ItemPresent = "TRN";
                Debug.LogFormat("[Connection Device #{0}] Indicator '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("T");
                SetButtonLetters.Add("R");
                SetButtonLetters.Add("N");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 7);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.IsIndicatorPresent(Indicator.BOB))
            {
                CodeShouldContain = "BOB";
                ItemPresent = "BOB";
                Debug.LogFormat("[Connection Device #{0}] Indicator '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("B");
                SetButtonLetters.Add("O");
                SetButtonLetters.Add("B");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 7);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.IsIndicatorPresent(Indicator.FRK))
            {
                CodeShouldContain = "FRK";
                ItemPresent = "FRK";
                Debug.LogFormat("[Connection Device #{0}] Indicator '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("F");
                SetButtonLetters.Add("R");
                SetButtonLetters.Add("K");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 23);
                Letter3value = LetterList[LetterGen3];
                LetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 7);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else
            {
                CodeShouldContain = "NAN";
                ItemPresent = "Not Availible";

                SetButtonLetters.Add("N");
                SetButtonLetters.Add("A");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen4 = Random.Range(0, 24);
                Letter4value = LetterList[LetterGen4];
                LetterList.Remove(Letter4value);

                LetterGen5 = Random.Range(0, 23);
                Letter5value = LetterList[LetterGen5];
                LetterList.Remove(Letter5value);

                LetterGen6 = Random.Range(0, 22);
                Letter6value = LetterList[LetterGen6];
                LetterList.Remove(Letter6value);

                LetterGen7 = Random.Range(0, 11);
                Letter7value = LetterList[LetterGen7];
                EvenLetterList.Remove(Letter7value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                LetterList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 8);
                Number2value = NumberList[NumberGen2];
                LetterList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 7);
                Number3value = NumberList[NumberGen3];
                LetterList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 6);
                Number4value = NumberList[NumberGen4];
                LetterList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 5);
                Number5value = NumberList[NumberGen5];
                LetterList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 4);
                Number6value = NumberList[NumberGen6];
                LetterList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 3);
                Number7value = NumberList[NumberGen7];
                LetterList.Remove(Number7value);

                SetButtonLetters.Add(Letter4value);
                SetButtonLetters.Add(Letter5value);
                SetButtonLetters.Add(Letter6value);
                SetButtonLetters.Add(Letter7value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 7);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
        }
        else if (Program == "Skype")
        {
            if (BombInfo.GetBatteryCount(Battery.D) > 0)
            {
                CodeShouldContain = "DBT";
                ItemPresent = "D";
                Debug.LogFormat("[Connection Device #{0}] Battery '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("D");
                SetButtonLetters.Add("B");
                SetButtonLetters.Add("T");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 11);
                Letter3value = EvenLetterList[LetterGen3];
                EvenLetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 8);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 7);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 5);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 4);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 3);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 7);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.GetBatteryCount(Battery.AA) > 0)
            {
                CodeShouldContain = "ABT";
                ItemPresent = "AA";
                Debug.LogFormat("[Connection Device #{0}] Battery '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("A");
                SetButtonLetters.Add("B");
                SetButtonLetters.Add("T");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 11);
                Letter3value = EvenLetterList[LetterGen3];
                EvenLetterList.Remove(Letter3value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                NumberList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 8);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 7);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 6);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 5);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 4);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 3);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 7);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.GetBatteryCount(Battery.AAx3) > 0)
            {
                CodeShouldContain = "3BT";
                ItemPresent = "3BT";
                Debug.LogFormat("[Connection Device #{0}] Port '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("B");
                SetButtonLetters.Add("T");
                SetButtonLetters.Add(FirstLetterSerial);

                Number1value = "3";
                Number1BtnText.text = "3";

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 11);
                Letter3value = EvenLetterList[LetterGen3];
                EvenLetterList.Remove(Letter3value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);
                SetButtonLetters.Add(Letter5value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);



                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else if (BombInfo.GetBatteryCount(Battery.AAx4) > 0)
            {
                CodeShouldContain = "4BT";
                ItemPresent = "4BT";
                Debug.LogFormat("[Connection Device #{0}] Port '{1}' is present", moduleId, ItemPresent);

                SetButtonLetters.Add("B");
                SetButtonLetters.Add("T");
                SetButtonLetters.Add(FirstLetterSerial);

                Number1value = "4";
                Number1BtnText.text = "4";

                LetterGen1 = Random.Range(0, 25);
                Letter1value = LetterList[LetterGen1];
                LetterList.Remove(Letter1value);

                LetterGen2 = Random.Range(0, 24);
                Letter2value = LetterList[LetterGen2];
                LetterList.Remove(Letter2value);

                LetterGen3 = Random.Range(0, 11);
                Letter3value = EvenLetterList[LetterGen3];
                EvenLetterList.Remove(Letter3value);

                NumberGen2 = Random.Range(0, 9);
                Number2value = NumberList[NumberGen2];
                NumberList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 8);
                Number3value = NumberList[NumberGen3];
                NumberList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 7);
                Number4value = NumberList[NumberGen4];
                NumberList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 6);
                Number5value = NumberList[NumberGen5];
                NumberList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 5);
                Number6value = NumberList[NumberGen6];
                NumberList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 4);
                Number7value = NumberList[NumberGen7];
                NumberList.Remove(Number7value);

                SetButtonLetters.Add(Letter1value);
                SetButtonLetters.Add(Letter2value);
                SetButtonLetters.Add(Letter3value);
                SetButtonLetters.Add(Letter4value);
                SetButtonLetters.Add(Letter5value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);



                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
            else
            {
                CodeShouldContain = "NAN";
                ItemPresent = "Not Availible";

                SetButtonLetters.Add("N");
                SetButtonLetters.Add("A");
                SetButtonLetters.Add(FirstLetterSerial);

                LetterGen4 = Random.Range(0, 24);
                Letter4value = LetterList[LetterGen4];
                LetterList.Remove(Letter4value);

                LetterGen5 = Random.Range(0, 23);
                Letter5value = LetterList[LetterGen5];
                LetterList.Remove(Letter5value);

                LetterGen6 = Random.Range(0, 22);
                Letter6value = LetterList[LetterGen6];
                LetterList.Remove(Letter6value);

                LetterGen7 = Random.Range(0, 11);
                Letter7value = LetterList[LetterGen7];
                EvenLetterList.Remove(Letter7value);

                NumberGen1 = Random.Range(0, 9);
                Number1value = NumberList[NumberGen1];
                LetterList.Remove(Number1value);

                NumberGen2 = Random.Range(0, 8);
                Number2value = NumberList[NumberGen2];
                LetterList.Remove(Number2value);

                NumberGen3 = Random.Range(0, 7);
                Number3value = NumberList[NumberGen3];
                LetterList.Remove(Number3value);

                NumberGen4 = Random.Range(0, 6);
                Number4value = NumberList[NumberGen4];
                LetterList.Remove(Number4value);

                NumberGen5 = Random.Range(0, 5);
                Number5value = NumberList[NumberGen5];
                LetterList.Remove(Number5value);

                NumberGen6 = Random.Range(0, 4);
                Number6value = NumberList[NumberGen6];
                LetterList.Remove(Number6value);

                NumberGen7 = Random.Range(0, 3);
                Number7value = NumberList[NumberGen7];
                LetterList.Remove(Number7value);

                SetButtonLetters.Add(Letter4value);
                SetButtonLetters.Add(Letter5value);
                SetButtonLetters.Add(Letter6value);
                SetButtonLetters.Add(Letter7value);

                SetButtonNumbers.Add(Number1value);
                SetButtonNumbers.Add(Number2value);
                SetButtonNumbers.Add(Number3value);
                SetButtonNumbers.Add(Number4value);
                SetButtonNumbers.Add(Number5value);
                SetButtonNumbers.Add(Number6value);
                SetButtonNumbers.Add(Number7value);

                PositionGen1L = Random.Range(0, 7);
                Letter1BtnText.text = SetButtonLetters[PositionGen1L];
                Letter1value = SetButtonLetters[PositionGen1L];
                SetButtonLetters.Remove(Letter1value);

                PositionGen2L = Random.Range(0, 6);
                Letter2BtnText.text = SetButtonLetters[PositionGen2L];
                Letter2value = SetButtonLetters[PositionGen2L];
                SetButtonLetters.Remove(Letter2value);

                PositionGen3L = Random.Range(0, 5);
                Letter3BtnText.text = SetButtonLetters[PositionGen3L];
                Letter3value = SetButtonLetters[PositionGen3L];
                SetButtonLetters.Remove(Letter3value);

                PositionGen4L = Random.Range(0, 4);
                Letter4BtnText.text = SetButtonLetters[PositionGen4L];
                Letter4value = SetButtonLetters[PositionGen4L];
                SetButtonLetters.Remove(Letter4value);

                PositionGen5L = Random.Range(0, 3);
                Letter5BtnText.text = SetButtonLetters[PositionGen5L];
                Letter5value = SetButtonLetters[PositionGen5L];
                SetButtonLetters.Remove(Letter5value);

                PositionGen6L = Random.Range(0, 2);
                Letter6BtnText.text = SetButtonLetters[PositionGen6L];
                Letter6value = SetButtonLetters[PositionGen6L];
                SetButtonLetters.Remove(Letter6value);

                PositionGen7L = Random.Range(0, 1);
                Letter7BtnText.text = SetButtonLetters[PositionGen7L];
                Letter7value = SetButtonLetters[PositionGen7L];
                SetButtonLetters.Remove(Letter7value);

                PositionGen1N = Random.Range(0, 7);
                Number1BtnText.text = SetButtonNumbers[PositionGen1N];
                Number1value = SetButtonNumbers[PositionGen1N];
                SetButtonNumbers.Remove(Number1value);

                PositionGen2N = Random.Range(0, 6);
                Number2BtnText.text = SetButtonNumbers[PositionGen2N];
                Number2value = SetButtonNumbers[PositionGen2N];
                SetButtonNumbers.Remove(Number2value);

                PositionGen3N = Random.Range(0, 5);
                Number3BtnText.text = SetButtonNumbers[PositionGen3N];
                Number3value = SetButtonNumbers[PositionGen3N];
                SetButtonNumbers.Remove(Number3value);

                PositionGen4N = Random.Range(0, 4);
                Number4BtnText.text = SetButtonNumbers[PositionGen4N];
                Number4value = SetButtonNumbers[PositionGen4N];
                SetButtonNumbers.Remove(Number4value);

                PositionGen5N = Random.Range(0, 3);
                Number5BtnText.text = SetButtonNumbers[PositionGen5N];
                Number5value = SetButtonNumbers[PositionGen5N];
                SetButtonNumbers.Remove(Number5value);

                PositionGen6N = Random.Range(0, 2);
                Number6BtnText.text = SetButtonNumbers[PositionGen6N];
                Number6value = SetButtonNumbers[PositionGen6N];
                SetButtonNumbers.Remove(Number6value);

                PositionGen7N = Random.Range(0, 1);
                Number7BtnText.text = SetButtonNumbers[PositionGen7N];
                Number7value = SetButtonNumbers[PositionGen7N];
                SetButtonNumbers.Remove(Number7value);

                LogCode();
                return;
            }
        }
    }




    void LogCode()
    {
        if (ItemPresent == "Not Availible")
        {
            Debug.LogFormat("[Connection Device #{0}] There is no port/indicator/battery present on bomb for the desired program {1}, so the code should contain 'NAN'", moduleId, Program);
            Debug.LogFormat("[Connection Device #{0}] The code should also contain '{1}', since the 1st letter of the serial is {1}", moduleId, FirstLetterSerial);
        }
        else
        {
            Debug.LogFormat("[Connection Device #{0}] The code should contain '{1}'", moduleId, CodeShouldContain);
            Debug.LogFormat("[Connection Device #{0}] The code should also contain '{1}', since the 1st letter of the serial is {1}", moduleId, FirstLetterSerial);
        }
    }

    //Submit the code
    void CodeSubmit()
    {

        if (Message.Length == 6)
        {
            Debug.LogFormat("[Connection Device #{0}] Sent message: '{1}'", moduleId, Message);
            if (Program == "Whatsapp")
            {
                if (Message.StartsWith("1") || Message.StartsWith("3") || Message.StartsWith("5") || Message.StartsWith("7") || Message.StartsWith("9"))
                {
                    if (Message.EndsWith("2") || Message.EndsWith("4") || Message.EndsWith("6") || Message.EndsWith("8") || Message.EndsWith("0"))
                    {
                        if (Message.Contains(CodeShouldContain))
                        {
                            if (Message.Contains(FirstLetterSerial))
                            {
                                CheckTime();
                            }
                            else
                            {
                                ErrorMessage = "Invalid code: Code doesn't contain 1st letter of serial.";
                                CodeIncorrect();
                            }
                        }
                        else
                        {
                            ErrorMessage = "Invalid code: Code doesn't contain the port/indicator/battery.";
                            CodeIncorrect();
                        }
                    }
                    else
                    {
                        ErrorMessage = "Last character of message not an even number.";
                        CodeIncorrect();
                    }
                }
                else
                {
                    ErrorMessage = "1st character of message not an uneven number.";
                    CodeIncorrect();
                }
            }
            if (Program == "Discord")
            {
                if (Message.StartsWith("1") || Message.StartsWith("2") || Message.StartsWith("3") || Message.StartsWith("4") || Message.StartsWith("5"))
                {
                    if (Message.Distinct().Count() == Message.Count())
                    {
                        if (Message.Contains(CodeShouldContain))
                        {
                            if (Message.Contains(FirstLetterSerial))
                            {
                                CheckTime();
                            }
                            else
                            {
                                ErrorMessage = "Invalid code: Code doesn't contain 1st letter of serial.";
                                CodeIncorrect();
                            }
                        }
                        else
                        {
                            ErrorMessage = "Invalid code: Code doesn't contain the port/indicator/battery.";
                            CodeIncorrect();
                        }
                    }
                    else
                    {
                        if (CodeShouldContain == "BOB")
                        {
                            if (Message.Contains(CodeShouldContain))
                            {
                                if (Message.Contains(FirstLetterSerial))
                                {
                                    CheckTime();
                                }
                                else
                                {
                                    ErrorMessage = "Invalid code: Code doesn't contain 1st letter of serial.";
                                    CodeIncorrect();
                                }
                            }
                            else
                            {
                                ErrorMessage = "Invalid code: Code doesn't contain the port/indicator/battery.";
                                CodeIncorrect();
                            }
                        }
                        else
                        {
                            ErrorMessage = "Message contains duplicate letters.";
                            CodeIncorrect();
                        }
                    }
                }
                else
                {
                    ErrorMessage = "1st character of message not 5 or less.";
                    CodeIncorrect();
                }
            }
            if (Program == "Skype")
            {
                if (Message.StartsWith("B") || Message.StartsWith("D") || Message.StartsWith("F") || Message.StartsWith("H") || Message.StartsWith("J") || Message.StartsWith("L") || Message.StartsWith("N") || Message.StartsWith("P") || Message.StartsWith("R") || Message.StartsWith("T") || Message.StartsWith("V") || Message.StartsWith("X") || Message.StartsWith("Z"))
                {
                    if (Message.EndsWith("6") || Message.EndsWith("7") || Message.EndsWith("8") || Message.EndsWith("9"))
                    {
                        if (Message.Distinct().Count() == Message.Count())
                        {
                            if (Message.Contains(CodeShouldContain))
                            {
                                if (Message.Contains(FirstLetterSerial))
                                {
                                    CheckTime();
                                }
                                else
                                {
                                    ErrorMessage = "Invalid code: Code doesn't contain 1st letter of serial.";
                                    CodeIncorrect();
                                }
                            }
                            else
                            {
                                ErrorMessage = "Invalid code: Code doesn't contain the port/indicator/battery.";
                                CodeIncorrect();
                            }
                        }
                        else if(Message.Distinct().Count() > 4)
                        {

                            if (Message.Contains(CodeShouldContain))
                            {
                                if (Message.Contains(FirstLetterSerial))
                                {
                                    CheckTime();
                                }
                                else
                                {
                                    ErrorMessage = "Invalid code: Code doesn't contain 1st letter of serial.";
                                    CodeIncorrect();
                                }
                            }
                            else
                            {
                                ErrorMessage = "Invalid code: Code doesn't contain the port/indicator/battery.";
                                CodeIncorrect();
                            }
                        }
                        else
                        {
                            ErrorMessage = "Message contains more than 2 duplicate characters.";
                            CodeIncorrect();
                        }
                    }
                    else
                    {
                        ErrorMessage = "Last character of message not 6 or more.";
                        CodeIncorrect();
                    }
                }
                else
                {
                    ErrorMessage = "1st character of message is not a letter with even value.";
                    CodeIncorrect();
                }
            }
        }
        else
        {
            Debug.LogFormat("[Connection Device #{0}] Your character count of {1} is not 6", moduleId, Characters);
        }
    }

    void CheckTime()
    {
        int Timer = (int)BombInfo.GetTime();
        Timer %= 60;
        Timer = (Timer / 10) + (Timer % 10);
        if (Timer > 10) Timer -= 10;
        int FirstNumberSerial = BombInfo.GetSerialNumberNumbers().First();
        int LastNumberSerial = BombInfo.GetSerialNumberNumbers().Last();
        int CombinedNumberSerial = FirstNumberSerial - LastNumberSerial;
        if (CombinedNumberSerial < 0)
            CombinedNumberSerial = CombinedNumberSerial + 10;

        Debug.LogFormat("[Connection Device #{0}] Time of submission: {1}", moduleId, Timer);

        if (Program == "Whatsapp")
        {
            Debug.LogFormat("[Connection Device #{0}] Time should be: {1}", moduleId, FirstNumberSerial);
        }
        else if (Program == "Discord")
        {
            Debug.LogFormat("[Connection Device #{0}] Time should be: {1}", moduleId, LastNumberSerial);
        }
        else if (Program == "Skype")
        {
            Debug.LogFormat("[Connection Device #{0}] Time should be: {1}", moduleId, CombinedNumberSerial);
        }

        if (Program == "Whatsapp" && Timer == FirstNumberSerial)
        {
             CodeCorrect();
        }
        else if (Program == "Discord" && Timer == LastNumberSerial)
        {
            CodeCorrect();
        }
        else if (Program == "Skype" && Timer == CombinedNumberSerial)
        {
            CodeCorrect();
        }
        else
        {
            ErrorMessage = "Submission too early/late.";
            CodeIncorrect();
        }
    }

    void CodeCorrect()
    {
        if (Program == "Whatsapp")
        {
            ScreenMat.material.mainTexture = WhatsappReceived;
            WhatsappCode.text = Message;
        }
        else if (Program == "Discord")
        {
            ScreenMat.material.mainTexture = DiscordReceived;
            DiscordCode.text = Message;
        }
        else if (Program == "Skype")
        {
            ScreenMat.material.mainTexture = SkypeReceived;
            SkypeCode.text = Message;
        }

        Number1Btn.OnInteract = Empty;
        Number2Btn.OnInteract = Empty;
        Number3Btn.OnInteract = Empty;
        Number4Btn.OnInteract = Empty;
        Number5Btn.OnInteract = Empty;
        Number6Btn.OnInteract = Empty;
        Number7Btn.OnInteract = Empty;

        Letter1Btn.OnInteract = Empty;
        Letter2Btn.OnInteract = Empty;
        Letter3Btn.OnInteract = Empty;
        Letter4Btn.OnInteract = Empty;
        Letter5Btn.OnInteract = Empty;
        Letter6Btn.OnInteract = Empty;
        Letter7Btn.OnInteract = Empty;

        GetComponent<KMBombModule>().HandlePass();
        Debug.LogFormat("[Connection Device #{0}] Message received succesfully. Module passed", moduleId);
        MessageReceivedText1.text = "Message:";
        MessageReceivedText2.text = "Received";
    }

    void CodeIncorrect()
    {
        Message = "";
        TextMessage.text = "";
        Characters = 0;
        TextBlock.transform.Translate(-0.0156f, 0, 0);
        GetComponent<KMBombModule>().HandleStrike();
        StartCoroutine("Wrong");
        Debug.LogFormat("[Connection Device #{0}] Message not received under reason: {1} Strike handed", moduleId, ErrorMessage);
    }

    IEnumerator Wrong()
    {
        for (int i = 0; i < 4; i++)
        {
            int TransitTime = i;
            if (TransitTime == 0)
            {
                ResponseCode.SetActive(true);
            }
            else if (TransitTime == 1)
            {
                ResponseCode.SetActive(false);
            }
            else if (TransitTime == 2)
            {
                ResponseCode.SetActive(true);
            }
            else if (TransitTime == 3)
            {
                ResponseCode.SetActive(false);
            }
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    //Buttons here.
    protected bool Number1()
    {
        if (TypingActive == true && TypeCharacter == false && Characters <= 6)
        {
            Debug.LogFormat("[Connection Device #{0}] Number key 1 pressed.", moduleId);
            KeyPressed = "Num1";
            Typing();
        }
        GetComponent<KMSelectable>().AddInteractionPunch();
        LaptopSFX.PlaySoundAtTransform("keyPress", transform);
        return false;
    }

    protected bool Number2()
    {
        if (TypingActive == true && TypeCharacter == false && Characters <= 6)
        {
            Debug.LogFormat("[Connection Device #{0}] Number key 2 pressed.", moduleId);
            KeyPressed = "Num2";
            Typing();
        }        
        GetComponent<KMSelectable>().AddInteractionPunch();
        LaptopSFX.PlaySoundAtTransform("keyPress", transform);
        return false;
    }

    protected bool Number3()
    {
        if (TypingActive == true && TypeCharacter == false && Characters <= 6)
        {
            Debug.LogFormat("[Connection Device #{0}] Number key 3 pressed.", moduleId);
            KeyPressed = "Num3";
            Typing();
        }        
        GetComponent<KMSelectable>().AddInteractionPunch();
        LaptopSFX.PlaySoundAtTransform("keyPress", transform);
        return false;
    }

    protected bool Number4()
    {
        if (TypingActive == true && TypeCharacter == false && Characters <= 6)
        {
            Debug.LogFormat("[Connection Device #{0}] Number key 4 pressed.", moduleId);
            KeyPressed = "Num4";
            Typing();
        }       
        GetComponent<KMSelectable>().AddInteractionPunch();
        LaptopSFX.PlaySoundAtTransform("keyPress", transform);
        return false;
    }

    protected bool Number5()
    {
        if (TypingActive == true && TypeCharacter == false && Characters <= 6)
        {
            Debug.LogFormat("[Connection Device #{0}] Number key 5 pressed.", moduleId);
            KeyPressed = "Num5";
            Typing();
        }        
        GetComponent<KMSelectable>().AddInteractionPunch();
        LaptopSFX.PlaySoundAtTransform("keyPress", transform);
        return false;
    }

    protected bool Number6()
    {
        if (TypingActive == true && TypeCharacter == false && Characters <= 6)
        {
            Debug.LogFormat("[Connection Device #{0}] Number key 6 pressed.", moduleId);
            KeyPressed = "Num6";
            Typing();
        }        
        GetComponent<KMSelectable>().AddInteractionPunch();
        LaptopSFX.PlaySoundAtTransform("keyPress", transform);
        return false;
    }

    protected bool Number7()
    {
        if (TypingActive == true && TypeCharacter == false && Characters <= 6)
        {
            Debug.LogFormat("[Connection Device #{0}] Number key 7 pressed.", moduleId);
            KeyPressed = "Num7";
            Typing();
        }        
        GetComponent<KMSelectable>().AddInteractionPunch();
        LaptopSFX.PlaySoundAtTransform("keyPress", transform);
        return false;
    }

    protected bool Letter1()
    {
        if (TypingActive == true && TypeCharacter == false && Characters <= 6)
        {
            Debug.LogFormat("[Connection Device #{0}] Letter key 1 pressed.", moduleId);
            KeyPressed = "Ltr1";
            Typing();
        }        
        GetComponent<KMSelectable>().AddInteractionPunch();
        LaptopSFX.PlaySoundAtTransform("keyPress", transform);
        return false;
    }

    protected bool Letter2()
    {
        if (TypingActive == true && TypeCharacter == false && Characters <= 6)
        {
            Debug.LogFormat("[Connection Device #{0}] Letter key 2 pressed.", moduleId);
            KeyPressed = "Ltr2";
            Typing();
        }        
        GetComponent<KMSelectable>().AddInteractionPunch();
        LaptopSFX.PlaySoundAtTransform("keyPress", transform);
        return false;
    }

    protected bool Letter3()
    {
        if (TypingActive == true && TypeCharacter == false && Characters <= 6)
        {
            Debug.LogFormat("[Connection Device #{0}] Letter key 3 pressed.", moduleId);
            KeyPressed = "Ltr3";
            Typing();
        }        
        GetComponent<KMSelectable>().AddInteractionPunch();
        LaptopSFX.PlaySoundAtTransform("keyPress", transform);
        return false;
    }

    protected bool Letter4()
    {
        if (TypingActive == true && TypeCharacter == false && Characters <= 6)
        {
            Debug.LogFormat("[Connection Device #{0}] Letter key 4 pressed.", moduleId);
            KeyPressed = "Ltr4";
            Typing();
        }        
        GetComponent<KMSelectable>().AddInteractionPunch();
        LaptopSFX.PlaySoundAtTransform("keyPress", transform);
        return false;
    }

    protected bool Letter5()
    {
        if (TypingActive == true && TypeCharacter == false && Characters <= 6)
        {
            Debug.LogFormat("[Connection Device #{0}] Letter key 5 pressed.", moduleId);
            KeyPressed = "Ltr5";
            Typing();
        }        
        GetComponent<KMSelectable>().AddInteractionPunch();
        LaptopSFX.PlaySoundAtTransform("keyPress", transform);
        return false;
    }

    protected bool Letter6()
    {
        if (TypingActive == true && TypeCharacter == false && Characters <= 6)
        {
            Debug.LogFormat("[Connection Device #{0}] Letter key 6 pressed.", moduleId);
            KeyPressed = "Ltr6";
            Typing();
        }        
        GetComponent<KMSelectable>().AddInteractionPunch();
        LaptopSFX.PlaySoundAtTransform("keyPress", transform);
        return false;
    }

    protected bool Letter7()
    {
        if (TypingActive == true && TypeCharacter == false && Characters <= 6)
        {
            Debug.LogFormat("[Connection Device #{0}] Letter key 7 pressed.", moduleId);
            KeyPressed = "Ltr7";
            Typing();
        }
        GetComponent<KMSelectable>().AddInteractionPunch();
        LaptopSFX.PlaySoundAtTransform("keyPress", transform);
        return false;
    }

    protected bool Bootup()
    {
        BootupBtnObj.SetActive(false);
        ActivateScreen();
        GetComponent<KMSelectable>().AddInteractionPunch();
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
        return false;
    }

    protected bool Submit()
    {
        GetComponent<KMSelectable>().AddInteractionPunch();
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        CodeSubmit();
        return false;
    }

    protected bool Whatsapp()
    {
        Debug.LogFormat("[Connection Device #{0}] Attempt to open 'Whatsapp'", moduleId);
        if (Program == "Whatsapp")
        {
            ScreenMat.material.mainTexture = WhatsappBack;
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
            Debug.LogFormat("[Connection Device #{0}] Whatsapp.exe succesfully opened", moduleId);
            StartCoroutine("Transition");
        }
        else
        {
            Debug.LogFormat("[Connection Device #{0}] Incorrect! Whatsapp.exe could not be found. Strike handed.", moduleId);
            GetComponent<KMBombModule>().HandleStrike();
        }
        return false;
    }

    protected bool Discord()
    {
        Debug.LogFormat("[Connection Device #{0}] Attempt to open 'Discord'", moduleId);
        if (Program == "Discord")
        {
            ScreenMat.material.mainTexture = DiscordBack;
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
            Debug.LogFormat("[Connection Device #{0}] Discord.exe succesfully opened", moduleId);
            StartCoroutine("Transition");
        }
        else
        {
            Debug.LogFormat("[Connection Device #{0}] Incorrect! Discord.exe could not be found. Strike handed.", moduleId);
            GetComponent<KMBombModule>().HandleStrike();
        }
        return false;
    }

    protected bool Skype()
    {
        Debug.LogFormat("[Connection Device #{0}] Attempt to open 'Skype'", moduleId);
        if (Program == "Skype")
        {
            ScreenMat.material.mainTexture = SkypeBack;
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
            Debug.LogFormat("[Connection Device #{0}] Skype.exe succesfully opened", moduleId);
            StartCoroutine("Transition");
        }
        else
        {
            Debug.LogFormat("[Connection Device #{0}] Incorrect! Skype.exe could not be found. Strike handed.", moduleId);
            GetComponent<KMBombModule>().HandleStrike();
        }
        return false;
    }

    protected bool Empty()
    {
        GetComponent<KMSelectable>().AddInteractionPunch();
        LaptopSFX.PlaySoundAtTransform("keyPress", transform);
        return false;
    }
}

