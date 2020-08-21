using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicBookManager : MonoBehaviour {
    private static ComicBookManager comicBook = null;
    public static ComicBookManager ComicBook {
        get {
            if(comicBook == null)
                comicBook = ((GameObject)Instantiate(Resources.Load<GameObject>("Text/ComicBookManager"))).GetComponent<ComicBookManager>();
            return comicBook;
        }
    }

    // UI
    private static ComicBookUIConsole console = null;
    public static ComicBookUIConsole Console {
        get {
            return console;
        }
    }

    // Chapter
    public static readonly string chapter1 = "Text/Chapter1";
    public static readonly string chapter2 = "Text/Chapter2";
    public static readonly string chapter3 = "Text/Chapter3";

    // state parameters
    private bool isPause;
    private bool isStop;
    private bool isLocked;

    // scripts
    private string[] comicScripts;
    private int viewIndex; // row number of comicScripts


    // Use this for initialization
    void Awake () {
        isStop = true;
        isPause = false;
        isLocked = false;
        viewIndex = -1;
    }
	
	// Update is called once per frame
	void Update() {
        if(isStop)
            return;
        
        if(isLocked)
            return;

        if(isPause) { 
            if(Input.GetKeyDown(KeyCode.Z) || Input.GetKey(KeyCode.LeftControl))
                Continue();
        } else {
            NextPage();
        }
    }

    private void Continue() {
        isPause = false;
    }

    public void Pause() {
        isPause = true;
        isLocked = true;
        Invoke("UnLockComicBook", 0.1f);
    }

    public void RoyalFlare() {
        isStop = false;
    }

    public void CloseComicBook() {
        isStop = true;
    }

    public void LockComicBook() {
        isLocked = true;
    }

    public void UnLockComicBook() {
        isLocked = false;
    }

    public void RaiseComicBook(string chapter) {
        isStop = true; // RoyalFlare
        isPause = false; // Z
        isLocked = false;
        viewIndex = -1;
        this.comicScripts = GetComicScript(chapter);
        // UI
        if (console == null)
            console = ((GameObject)Instantiate(Resources.Load<GameObject>("Text/ComicBookUIConsole"))).GetComponent<ComicBookUIConsole>();
    }

    private string[] GetComicScript(string whichChapter) {
        string comicScript = Resources.Load<TextAsset>(whichChapter).text as string;
        comicScript = comicScript.Replace("\r\n", "\n");
        return comicScript.Split('\n');
    }

    private void NextPage() {
        while(!isPause && !isStop && !isLocked) {
            viewIndex++;
            if(viewIndex >= comicScripts.Length) {
                Pause();
                throw new System.Exception("End This Chapter In ComicBook!");
                break;
            }
            print(comicScripts[viewIndex]);
            Context context = new Context(comicScripts[viewIndex]);
            ExpressionRootNode rootNode = new ExpressionRootNode();
            rootNode.Interpret(context);
            rootNode.Execute();

        }
    }




}
