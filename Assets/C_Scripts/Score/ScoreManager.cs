using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : NetworkBehaviour {
    
    public Dictionary<NetworkInstanceId, List<int>> playerDictionary = new Dictionary<NetworkInstanceId, List<int>>();

    void AddPlayerWithNoNingyo(NetworkInstanceId netId) {
        if(playerDictionary.ContainsKey(netId) == false) {
            playerDictionary.Add(netId, new List<int> { 0, 0 });
            BoardcastGetPlayerDictionary();
        } else {
            print("hasAdded!");
        }
    }

    void AddAllNingyoCount(NetworkInstanceId clientPlayerNetId) {
        if(!isServer)
            return;
        if(playerDictionary.ContainsKey(clientPlayerNetId)) {
            playerDictionary[clientPlayerNetId][0]++;
            RpcAnsSetAllNingyoCount(clientPlayerNetId, playerDictionary[clientPlayerNetId][0]);
        } else {
            playerDictionary.Add(clientPlayerNetId, new List<int> { 1, 0 }); // 先all，后now
            RpcAnsSetAllNingyoCount(clientPlayerNetId, 1);
        }
    }
    void MinusAllNingyoCount(NetworkInstanceId clientPlayerNetId) {
        if(!isServer)
            return;
        if(playerDictionary.ContainsKey(clientPlayerNetId)) {
            if(playerDictionary[clientPlayerNetId][0] > 0 && playerDictionary[clientPlayerNetId][1] > 0)
                playerDictionary[clientPlayerNetId][0]--;
            RpcAnsSetAllNingyoCount(clientPlayerNetId, playerDictionary[clientPlayerNetId][0]);
        } else {
            print("BugHere");
            playerDictionary.Add(clientPlayerNetId, new List<int> { 0, 0 }); // 先all，后now
            RpcAnsSetAllNingyoCount(clientPlayerNetId, 0);
        }

    }
    [ClientRpc]
    void RpcAnsSetAllNingyoCount(NetworkInstanceId clientPlayerNetId, int count) {
        GameObject player = ClientScene.FindLocalObject(clientPlayerNetId);
        player.SendMessage("AnsSetAllNingyoCount", count);
    }

    void AddNowNingyoCount(NetworkInstanceId clientPlayerNetId) {
        if(playerDictionary.ContainsKey(clientPlayerNetId)) {
            playerDictionary[clientPlayerNetId][1]++;
            RpcAnsSetNowNingyoCount(clientPlayerNetId, playerDictionary[clientPlayerNetId][1]);
        } else {
            playerDictionary.Add(clientPlayerNetId, new List<int> { 0, 1 }); // 先all，后now
            RpcAnsSetNowNingyoCount(clientPlayerNetId, 1);
        }

    }
    void MinusNowNingyoCount(NetworkInstanceId clientPlayerNetId) {
        if(playerDictionary.ContainsKey(clientPlayerNetId)) {
            if(playerDictionary[clientPlayerNetId][1] > 0)
                playerDictionary[clientPlayerNetId][1]--;
            RpcAnsSetNowNingyoCount(clientPlayerNetId, playerDictionary[clientPlayerNetId][1]);
        } else {
            print("BugHere");
            playerDictionary.Add(clientPlayerNetId, new List<int> { 0, 0 }); // 先all，后now
            RpcAnsSetNowNingyoCount(clientPlayerNetId, 0);
        }

    }
    [ClientRpc]
    void RpcAnsSetNowNingyoCount(NetworkInstanceId clientPlayerNetId, int count) {
        GameObject player = ClientScene.FindLocalObject(clientPlayerNetId);
        player.SendMessage("AnsSetNowNingyoCount", count);
    }

    void GetPlayerDictionary(NetworkInstanceId clientPlayerNetId) {
        int[] lists = new int[playerDictionary.Count * 3];
        int i = 0;
        foreach(NetworkInstanceId netId in playerDictionary.Keys) {
            List<int> listInt = playerDictionary[netId];
            lists[i] = (int)netId.Value;
            i++;
            lists[i] = listInt[0];
            i++;
            lists[i] = listInt[1];
            i++;
        }
        RpcSetPlayerDictionary(clientPlayerNetId, lists);
    }
    [ClientRpc]
    void RpcSetPlayerDictionary(NetworkInstanceId clientPlayerNetId, int[] lists) {
        GameObject player = ClientScene.FindLocalObject(clientPlayerNetId);
        player.SendMessage("AnsSetPlayerDictionary", lists);
    }

    void BoardcastGetPlayerDictionary() {
        int[] lists = new int[playerDictionary.Count * 3];
        int i = 0;
        foreach(NetworkInstanceId netId in playerDictionary.Keys) {
            List<int> listInt = playerDictionary[netId];
            lists[i] = (int)netId.Value;
            i++;
            lists[i] = listInt[0];
            i++;
            lists[i] = listInt[1];
            i++;
        }
        RpcBoardcastSetPlayerDictionary(lists);
    }
    [ClientRpc]
    void RpcBoardcastSetPlayerDictionary(int[] lists) {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
            player.SendMessage("AnsSetPlayerDictionary", lists);
    }
}
