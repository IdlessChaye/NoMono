using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventNode : FuncNode {

    public GameEventNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Event");
        InterpretPart(context);
    }

    public override void Execute() {
        ComicBookManager.Console.GameEvent(paraDic["eventname"]);
    }
}
