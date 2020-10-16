using MelonLoader;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Phasmophobia_RPC {
    public class MyMod : MelonMod {
        public Discord.Discord discord;

        public static long timeStamp = 0;
        public static string roomNow, roomOld, mapa;

        int playerSize;

        public override void OnApplicationStart() {

            discord = new Discord.Discord(765944720882270211, (System.UInt64)Discord.CreateFlags.Default);
        }
        public override void OnLevelWasInitialized(int level) {
            if (level == 1) {
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
            } else if(level >0){
                roomNow = PhotonNetwork.room.Name;
                roomOld = roomNow;
                UpdateActivity(discord, IsRoomPrivate(), RoomName(), (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds, playerSize);
            }
        }
        public override void OnUpdate() {
            if (PhotonNetwork.inRoom) {
                playerSize = PhotonNetwork.playerList.Length;
            }

            if(RoomName() != mapa) {
                mapa = RoomName();
            }

            if (PhotonNetwork.inRoom && PhotonNetwork.room.Name != roomOld) {
                roomNow = PhotonNetwork.room.Name;
                roomOld = roomNow;
                UpdateActivity(discord, IsRoomPrivate(), RoomName(), (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds, playerSize);
            } 

            if(PhotonNetwork.inRoom && PhotonNetwork.room.Name == roomOld && RoomName() != mapa) {
                UpdateActivity(discord, IsRoomPrivate(), RoomName(), (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds, playerSize);
            }
            discord.RunCallbacks();

        }

        public string RoomName() {
            string SceneRoom = SceneManager.GetActiveScene().name.Replace('_', ' ');
            if (SceneRoom == "Menu New") {
                return "Menu";
            }
            return SceneRoom;
        }
        public string IsRoomPrivate() {
            if (PhotonNetwork.room.IsVisible) {
                return "Public";
            } else {
                return "Private";
            }
        }

        static void UpdateActivity(Discord.Discord discord, string state, string Details, long time, int size) {
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
                    MaxSize = 4,
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
