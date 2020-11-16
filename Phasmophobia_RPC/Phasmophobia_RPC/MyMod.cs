using MelonLoader;
using System;
using UnityEngine.SceneManagement;
using Photon.Pun;
namespace Phasmophobia_RPC {
    public class MyMod : MelonMod {
        public Discord.Discord discord;

        public static long timeStamp = 0;
        public static string roomNow, roomOld, mapa;

        public static int playerSize,playersCount, helpVar;
        public override void OnApplicationStart() {

            discord = new Discord.Discord(765944720882270211, (System.UInt64)Discord.CreateFlags.Default);
        }
        public override void OnLevelWasInitialized(int level) {
            if (level == 1) {
                helpVar = 0;
                roomNow = "Menu";

                var activityManager = discord.GetActivityManager();
                var activity = new Discord.Activity {
                    State = "",
                    Details = roomNow,
                    Timestamps =
           {
                Start = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
            },
                    Assets =
           {
                LargeImage = "icon",
                LargeText = "By Knuckles#4442"
            }, };
                activityManager.UpdateActivity(activity, (res) => {
                    if (res == Discord.Result.Ok) {
                    }
                });

            } 
            //else if (level == 1 && PhotonNetwork.InRoom) {
            //    UpdateActivity(discord, IsRoomPrivate(), RoomName(), (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds, PhotonNetwork.CurrentRoom.PlayerCount);
            //} else if (level > 1) {
            //    roomNow = PhotonNetwork.CurrentRoom.Name;
            //    roomOld = roomNow;
            //    UpdateActivity(discord, IsRoomPrivate(), RoomName(), (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds, PhotonNetwork.CurrentRoom.PlayerCount);
            //}
        }
        public override void OnUpdate() {

            if (PhotonNetwork.InRoom) {
                MelonLogger.Log(PhotonNetwork.CurrentRoom.Name + " \\ " + roomOld);
                playerSize = PhotonNetwork.CurrentRoom.players.Count;
                if(helpVar == 0) {
                    playersCount = playerSize;
                }
            }

            //if (PhotonNetwork.InRoom && helpVar == 1) {
            //    playersCount = playerSize;
            //    UpdateActivity(discord, IsRoomPrivate(), RoomName(), 0, playerSize);
            //} else if (PhotonNetwork.InLobby && helpVar == 0) {
            //    UpdateActivityMenu(discord);
            //} 

            if (RoomName() != mapa) {
                mapa = RoomName();
            }

            if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.Name != roomOld) {
                MelonLogger.Log("PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.Name != roomOld");
                roomNow = PhotonNetwork.CurrentRoom.Name;
                roomOld = roomNow;
                UpdateActivity(discord, IsRoomPrivate(), RoomName(), (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds, playerSize);
            } else if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.Name == roomOld) {
                MelonLogger.Log("PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.Name == roomOld");
                UpdateActivity(discord, IsRoomPrivate(), RoomName(), (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds, playerSize);
            } else if (PhotonNetwork.InLobby && helpVar ==2) {
                MelonLogger.Log("PhotonNetwork.InLobby && helpVar ==2");
                UpdateActivityMenu(discord);
            }

            discord.RunCallbacks();

        }

        public string RoomName() {
            MelonLogger.Log("RoomName");
            string SceneRoom = SceneManager.GetActiveScene().name.Replace('_', ' ');
            if (SceneRoom == "Menu New") {
                return "Menu";
            }
            return SceneRoom;
        }
        public string IsRoomPrivate() {
            MelonLogger.Log("IsRoomPrivate");
            if (PhotonNetwork.CurrentRoom.isVisible && PhotonNetwork.InRoom) {
                return "Public";
            }else{
                return "Private";
            }
        }
        static void UpdateActivityMenu(Discord.Discord discord) {
            MelonLogger.Log("UpdateActivityMenu");
            MyMod.helpVar = 1;
            var activityManager = discord.GetActivityManager();
            var lobbyManager = discord.GetLobbyManager();
            var activity = new Discord.Activity {
                State = "",
                Details = "Menu",
                Timestamps =
                {
                Start = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
            },
                Assets =
                {
                LargeImage = "icon",
                LargeText = "By Knuckles#4442",
            },
                Instance = true,
            };
            activityManager.UpdateActivity(activity, result => {
            });
        }

        static void UpdateActivity(Discord.Discord discord, string state, string Details, long time, int size) {
            MelonLogger.Log("UpdateActivity");
            helpVar = 2;
            var activityManager = discord.GetActivityManager();
            var lobbyManager = discord.GetLobbyManager();
            var activity = new Discord.Activity {
                State = state,
                Details = Details,
                Timestamps =
                {
                Start = time,
            },
                Assets =
                {
                LargeImage = "icon",
                LargeText = "By Knuckles#4442",
            },
                Party = {
               Id = "knuckles",
               Size = {
                    CurrentSize = size,
                    MaxSize = PhotonNetwork.CurrentRoom.MaxPlayers,
                },
            },
                Secrets = {
                Join = null,
            },
                Instance = true,
            };

            activityManager.UpdateActivity(activity, result => {

                // Send an invite to another user for this activity.
                // Receiver should see an invite in their DM.
                // Use a relationship user's ID for this.
                // activityManager
                //   .SendInvite(
                //       364843917537050624,
                //       Discord.ActivityActionType.Join,
                //       "",
                //       inviteResult =>
                //       {
                //           Console.WriteLine("Invite {0}", inviteResult);
                //       }
                //   );
            });

        }
    }
}
