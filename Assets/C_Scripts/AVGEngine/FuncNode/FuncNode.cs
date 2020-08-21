using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FuncNode : InterpreterBaseNode {
    protected Dictionary<string, string> paraDic = new Dictionary<string, string>();

    protected void InterpretPart(Context context) {
        while(true) {
            string denghaoToken = context.GetNextToken();
            
            if(denghaoToken == null || !denghaoToken.Equals("="))
                break;

            string paraToken = context.GetCurrentToken().ToLower();
            context.NextToken();
            string valueToken = context.NextToken();
            //Debug.Log("para= " + paraToken + "  value= " + valueToken);
            if(paraDic.ContainsKey(paraToken)) {
                paraDic[paraToken] = valueToken;
            } else {
                paraDic.Add(paraToken, valueToken);
            }
            context.NextToken();
        }
    }
}
