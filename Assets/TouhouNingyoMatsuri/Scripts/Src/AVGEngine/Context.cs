using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 只能处理一行的内容，要是太大了，递归受不了
public class Context{
    private int index = -1;
    private string[] tokens;
    private string currentToken;

    private Stack<char> stackRPN = new Stack<char>();
    private List<string> stringList = new List<string>();
    private Stack<char> stackChar = new Stack<char>();

    public Context(string text) {
        text.Trim();
        text = text.Replace("  ", " ").Replace("\r\n","\n");
        tokens = RPNSplit(text);
        NextToken();
    }

    public string NextToken() {
        if(IsOver()) {
            currentToken = null;
            return currentToken;
        }
        index++;
        if(index < tokens.Length) {
            currentToken = tokens[index];
        } else {
            currentToken = null;
        }
        return currentToken;
    }

    public string GetCurrentToken() {
        return currentToken;
    }

    public string GetNextToken() {
        int newIndex = index + 1;
        if(newIndex >= tokens.Length)
            return null;
        return tokens[newIndex];
    }

    public bool IsOver() {
        return index >= tokens.Length;
    }

    public void SkipToken(string tokenName) {
        if(!currentToken.Equals(tokenName,System.StringComparison.OrdinalIgnoreCase)) {
            throw new System.Exception("ERRER IN SkipToken!");
        }
        NextToken();
    }

    private string[] RPNSplit(string text) {
        for(int i = 0;i < text.Length; i++) {
            char currentChar = text[i];
            switch(currentChar) {
                case ' ':
                    AddString();
                    break;
                case '=':
                    AddString();
                    stringList.Add("=");
                    break;
                case '(':
                    char nextChar;
                    for(int j = i+1;j<text.Length;j++) {
                        nextChar = text[j];
                        if(nextChar == ')') {
                            AddString();
                            i = j;
                            break;
                        }
                        stackRPN.Push(text[j]);
                    }
                    break;
                case '\n':
                    AddString();
                    break;
                default:
                    stackRPN.Push(text[i]);
                    break;
            }
        }
        if(stackRPN.Count != 0)
            AddString();
        return stringList.ToArray();
    }

    private string GetStringFromStackRPN() {
        stackChar.Clear();
        while(stackRPN.Count != 0)
            stackChar.Push(stackRPN.Pop());
        string str = "";
        while(stackChar.Count != 0)
            str += stackChar.Pop();
        return str;
    }

    private void AddString() {
        if(stackRPN.Count != 0)
            stringList.Add(GetStringFromStackRPN());
    }
}
