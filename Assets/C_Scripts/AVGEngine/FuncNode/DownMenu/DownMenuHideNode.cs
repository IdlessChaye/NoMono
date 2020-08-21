using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownMenuHideNode : FuncNode {

    public DownMenuHideNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Hide");
        InterpretPart(context);
    }

    public override void Execute() {
        // Need to be done
    }
}
