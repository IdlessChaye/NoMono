using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRemoveNode : FuncNode {

    public MusicRemoveNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Remove");
        InterpretPart(context);
    }

    public override void Execute() {
        // Need to be done
    }
}
