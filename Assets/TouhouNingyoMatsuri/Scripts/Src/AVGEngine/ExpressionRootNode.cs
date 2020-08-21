using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionRootNode : InterpreterBaseNode {
    private List<InterpreterBaseNode> nodeList = new List<InterpreterBaseNode>();

    public override void Interpret(Context context) {
        while(true) {
            if(context.GetCurrentToken() == null)
                break; 
            InterpreterBaseNode node = new CommandTypeNode();
            node.Interpret(context);
            nodeList.Add(node);
        }
    }

    public override void Execute() {
        foreach(InterpreterBaseNode node in nodeList)
            node.Execute();
    }
}