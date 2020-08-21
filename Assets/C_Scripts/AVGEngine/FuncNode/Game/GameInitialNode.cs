using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitialNode : FuncNode {

    public GameInitialNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Initial");
        InterpretPart(context);
    }

    public override void Execute() {
        ComicBookManager.Console.GameInitial();
    }
}
