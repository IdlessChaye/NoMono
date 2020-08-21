using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownMenuShowNode : FuncNode {

    public DownMenuShowNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Show");
        InterpretPart(context);
    }

    public override void Execute() {
        // Need to be done
    }
}
