using HarmonyLib;
using InControl;
using Photon.Pun;
using RWF;
using System.Collections;
using System.Linq;
using UnboundLib;
using UnboundLib.Extensions;
using UnboundLib.GameModes;
using UnityEngine;

namespace RootAdvancedCards {
    public class Mitosis:MonoBehaviour {
        public const string MitosisID = "MitosisPlayerSpawnInstance";
        LobbyCharacter player;
        Player Owner;
        void Awake() {
            Owner = GetComponentInParent<Player>();
            if(!Owner.data.view.IsMine)// || PlayerManager.instance.players.Count > 15
                DestroyImmediate(this);
        }

        void Start() {

            //GameModeManager.AddOnceHook(GameModeHooks.HookGameEnd, cleanup, Priority.First);
            InputDevice inputDevice = Owner.data.playerActions.Device;

            /*
            List<LobbyCharacter> localCharacters = PhotonNetwork.LocalPlayer.GetProperty<LobbyCharacter[]>("players").ToList();
            int localPlayerNumber = Enumerable.Range(0, localCharacters.Count + 1).Where(i => i == localCharacters.Count || localCharacters[i] == null).First();
            int colorID = Owner.colorID();
            player = new LobbyCharacter(PhotonNetwork.LocalPlayer, colorID, localPlayerNumber);
            if(localPlayerNumber < localCharacters.Count)
                localCharacters[localPlayerNumber] = player;
            else
                localCharacters.Add(player);
            PhotonNetwork.LocalPlayer.SetProperty("players", localCharacters.ToArray());

            List<LobbyCharacter> temp = PhotonNetwork.LocalPlayer.GetProperty<LobbyCharacter[]>(PhotonPlayerExtensions.charkey).ToList();
            while(localPlayerNumber >= temp.Count) {
                temp.Add(null);
                PhotonNetwork.LocalPlayer.SetProperty(PhotonPlayerExtensions.charkey, temp.ToArray());
            }
            StartCoroutine(PlayerAssignerExtensions.CreatePlayer(PlayerAssigner.instance, localCharacters[localPlayerNumber], inputDevice));*/
            int count = PlayerManager.instance.players.Count;
            Vector3 position = Owner.transform.position;
            CharacterData component = PhotonNetwork.Instantiate(PlayerAssigner.instance.playerPrefab.name, position, Quaternion.identity, 0, new object[] { MitosisID, Owner.teamID, count, Owner.colorID() }).GetComponent<CharacterData>();
            if(inputDevice != null) {
                component.input.inputType = GeneralInput.InputType.Controller;
                component.playerActions = PlayerActions.CreateWithControllerBindings();
            } else {
                component.input.inputType = GeneralInput.InputType.Keyboard;
                component.playerActions = PlayerActions.CreateWithKeyboardBindings();
            }
            component.playerActions.Device = inputDevice;
            PlayerAssigner.instance.players.Add(component);
            Player newPlayer = component.player;
            PlayerManager.RegisterPlayer(newPlayer);
            var teamID = Owner.teamID;
            newPlayer.teamID = teamID;
            newPlayer.playerID = count;
            if(!PhotonNetwork.OfflineMode) {
                ExitGames.Client.Photon.Hashtable customProperties = newPlayer.data.view.Owner.CustomProperties;
                if(customProperties.ContainsKey("PlayerID")) {
                    customProperties["PlayerID"] = count;
                } else {
                    customProperties.Add("PlayerID", count);
                }
                if(customProperties.ContainsKey("TeamID")) {
                    customProperties["TeamID"] = teamID;
                } else {
                    customProperties.Add("TeamID", teamID);
                }
                newPlayer.data.view.Owner.SetCustomProperties(customProperties);
            }
        }
    }

    [HarmonyPatch(typeof(PlayerAssigner), nameof(PlayerAssigner.Awake))]
    public class MitosisPlayerAssigner {
        public static void Postfix() {
            PlayerAssigner.instance.playerPrefab.GetOrAddComponent<MitosisTracker>();
        }
    }

    public class MitosisTracker:MonoBehaviour, IPunInstantiateMagicCallback {
        public void OnPhotonInstantiate(PhotonMessageInfo info) {
            UnityEngine.Debug.Log("MitosisScriptRun");

            var data = info.photonView.InstantiationData;
            if(data != null && data.Count() == 4 && data[0] is string str && str == Mitosis.MitosisID) {
                UnityEngine.Debug.Log($"[{data[0]},{data[1]},{data[2]},{data[3]}]");
                Player player = GetComponent<Player>();
                player.teamID = (int)data[1];
                player.playerID = (int)data[2];
                player.AssignColorID((int)data[3]);
                PlayerManager.RegisterPlayer(player);
                GameModeManager.AddOnceHook(GameModeHooks.HookGameEnd, cleanup, Priority.First);
            }

            UnityEngine.Debug.Log("MitosisScriptRan");
        }
        IEnumerator cleanup(IGameModeHandler _) {
            Player player = GetComponent<Player>();
            PlayerManager.instance.RemovePlayer(player);
            yield break;
        }
    }
}
