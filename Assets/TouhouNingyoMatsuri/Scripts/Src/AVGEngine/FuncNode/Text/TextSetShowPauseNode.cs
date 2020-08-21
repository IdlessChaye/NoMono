using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSetShowPauseNode : FuncNode {

    public TextSetShowPauseNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("SetShowPause");
        InterpretPart(context);
    }

    public override void Execute() {
        // Need to be done
    }
}
