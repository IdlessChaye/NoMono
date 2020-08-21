using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseNode : FuncNode {

    public GamePauseNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Pause");
        InterpretPart(context);
    }

    public override void Execute() {
        ComicBookManager.Console.GamePause();
    }
}
