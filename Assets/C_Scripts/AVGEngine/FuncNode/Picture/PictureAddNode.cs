using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureAddNode : FuncNode {

    public PictureAddNode() {
        // Initial function default parameters
    }

    public override void Interpret(Context context) {
        context.SkipToken("Add");
        InterpretPart(context);
    }

    public override void Execute() {
        // Need to be done
    }
}
