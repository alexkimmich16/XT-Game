using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Custom
{
    public static class Cus
    {
        public static string PlayerTeam = "Team";
        public static string PlayerHealth = "Health";

        public static string GameWarmupTimer = "WarmupTimer";
        public static string GameFinishTimer = "FinishTimer";
        public static string GameStateText = "State";

        public static string AttackTeamCount = "AttackTeam";
        public static string DefenseTeamCount = "DefenseTeam";
        
        public static int GetLocal()
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[i])
                {
                    return i;
                }
            }
            Debug.LogError("Get Local Failure");
            return 100;
        }
        public static int GetOther()
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.LocalPlayer != PhotonNetwork.PlayerList[i])
                {
                    return i;
                }
            }
            Debug.LogError("Get Local Failure");
            return 100;
        }
        #region NetworkGet
        public static float GetGameFloat(string text)
        {
            //Debug.Log("Getfloat");
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(text, out object temp))
                return (float)temp;
            else
            {
                Debug.LogError("GetGameFloat.main with string: " + text + "has not been set");
                return 0f;
            }
        }
        public static int GetGameInt(string text)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(text, out object temp))
                return (int)temp;
            else
            {
                Debug.LogError("GetHash.GetInt.OfRoom with string: " + text + "has not been set");
                return 100;
            }
        }
        public static int GetPlayerInt(string text, Player player)
        {
            //Debug.Log(text);
            if (player.CustomProperties.TryGetValue(text, out object temp))
                return (int)temp;
            else
            {
                Debug.LogError("GetHash.GetInt.OfPlayer with string: " + text + " has not been set");
                return 100;
            }
        }
        public static bool GetGameBool(string text)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(text, out object temp))
                return (bool)temp;
            else
            {
                Debug.LogError("GetHash.GetBool with string: " + text + "has not been set");
                return true;
            }
        }

        public static bool GetPlayerBool(string text, Player player)
        {
            if (player.CustomProperties.TryGetValue(text, out object temp))
                return (bool)temp;
            else
            {
                Debug.LogError("GetHash.GetBool with string: " + text + "has not been set");
                return true;
            }
        }
        #endregion Exists
        #region Exists
        public static bool Exists(string text, Player player)
        {
            if (player == null)
            {
                //if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(text))

                if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(text))
                    return true;
                else
                {
                    return false;
                }
            }
            else
            {
                if (player.CustomProperties.ContainsKey(text))
                    return true;
                else
                {
                    return false;
                }
            }

        }

        #endregion
        #region NetworkSet

        public static void SetGameFloat(string text, float Num)
        {
            //Debug.Log("Setfloat");
            Hashtable TeamHash = new Hashtable();
            TeamHash.Add(text, Num);
            PhotonNetwork.CurrentRoom.SetCustomProperties(TeamHash);
        }
        public static void SetPlayerBool(string text, bool State, Player player)
        {
            Hashtable HealthHash = new Hashtable();
            HealthHash.Add(text, State);
            player.SetCustomProperties(HealthHash);
        }
        public static void SetGameBool(string text, bool State)
        {
            Hashtable HealthHash = new Hashtable();
            HealthHash.Add(text, State);
            //PhotonNetwork.CurrentRoom.SetCustomProperties(HealthHash);
            PhotonNetwork.CurrentRoom.SetCustomProperties(HealthHash);
        }
        public static void SetPlayerInt(string text, int SetNum, Player player)
        {
            Hashtable HealthHash = new Hashtable();
            HealthHash.Add(text, SetNum);
            player.SetCustomProperties(HealthHash);
            //Debug.Log(text);
        }
        public static void SetGameInt(string text, int SetNum)
        {
            Hashtable HealthHash = new Hashtable();
            HealthHash.Add(text, SetNum);
            
            PhotonNetwork.CurrentRoom.SetCustomProperties(HealthHash);
        }
        
#endregion
    }
}
