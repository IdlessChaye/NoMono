using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStopNode : FuncNode {

    public GameStopNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Stop");
        InterpretPart(context);
    }

    public override void Execute() {
        ComicBookManager.Console.GameStop();
    }
}
