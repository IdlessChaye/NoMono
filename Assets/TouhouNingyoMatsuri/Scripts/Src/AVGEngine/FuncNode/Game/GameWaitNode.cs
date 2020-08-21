using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWaitNode : FuncNode {

    public GameWaitNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Wait");
        InterpretPart(context);
    }

    public override void Execute() {
        // Need to be done
    }
}
