using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureChangeNode : FuncNode {

    public PictureChangeNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Change");
        InterpretPart(context);
    }

    public override void Execute() {
        // Need to be done
    }
}
