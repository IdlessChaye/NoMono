using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterpreterBaseNode {
    public abstract void Interpret(Context context);
    public abstract void Execute();
}
