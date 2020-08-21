using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStopNode : FuncNode {

    public MusicStopNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Stop");
        InterpretPart(context);
    }

    public override void Execute() {
        // Need to be done
    }
}
