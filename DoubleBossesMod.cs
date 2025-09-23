using BepInEx;
using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Silksong.Mods.DoubleBossesMod
{
    [BepInPlugin("com.eniac.doublebossesmod", "Silksong All Bosses Duplicator", "0.0.1")]
    [BepInProcess("Hollow Knight Silksong.exe")]
    public class DoubleBossesMod : BaseUnityPlugin
    {

        private void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(DoubleBossesMod));
            Logger.LogInfo("com.eniac.doublbossesemod loaded and initialized!");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //Method call for enemy doubler, and various exclusions for objects I do not want duplicated
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        { 
            foreach (var fsm in FindObjectsOfType<PlayMakerFSM>())
            {
                if (scene.name == "Cog_Dancers" || scene.name == "Slab_10b" || scene.name == "Dock_09" || scene.name == "Shellwood_18")
                {
                    break;
                }
                if (scene.name == "Clover_10" && fsm.name == "Boss Scene")
                {
                    continue;
                }
                ModifyEnemies(fsm);

                if (fsm.name == "Boss Control" && scene.name == "Abyss Cocoon")
                {
                    GameObject doubler = Instantiate(fsm.gameObject, fsm.transform.position, fsm.transform.rotation, fsm.transform.parent);
                    doubler.name += "CLONE";
                    fsm.name += "CLONE";
                }
            }
            
        }

        //Makes the First Sinner work because it's spawning is weird

        [HarmonyPostfix]
        [HarmonyPatch(typeof(HutongGames.PlayMaker.FsmState), "OnEnter")]
        private static void OnFSMStateEntered(HutongGames.PlayMaker.FsmState __instance)
        {
            var fsm = __instance.Fsm?.FsmComponent;
            var fsm_parent = fsm.transform.parent;
            if (fsm == null) { return; }

            if (fsm.name == "First Weaver" && fsm.gameObject.scene.name == "Slab_10b")
            {
                if (__instance.Name == "First Idle")
                {
                    var doubler_par = GameObject.Find("Boss Scene(Clone)CLONE");
                    if (doubler_par == null) { return; }
                    var doubler = doubler_par.transform.Find("First Weaver");
                    if (doubler != null)
                    {

                        var doublerFSMs = doubler.GetComponents<PlayMakerFSM>();
                        foreach (var dfsm in doublerFSMs)
                        {
                            dfsm.Fsm.SetState("Idle");
                        }
                    }
                }
            }

            //Similar to above, but for Lost Lace
            if ((fsm.name == "Lost Lace Boss" && fsm.gameObject.scene.name == "Abyss_Cocoon"))
            {
                if (__instance.Name == "Start")
                {
                    GameObject doubler = Instantiate(fsm.gameObject, fsm.transform.position, fsm.transform.rotation, fsm.transform.parent);
                    doubler.name += "CLONE";
                    fsm.name += "CLONE";

                    var doublerFSMs = doubler.GetComponents<PlayMakerFSM>();
                    foreach (var dfsm in doublerFSMs)
                    {
                        dfsm.Fsm.SetState("Idle");
                    }
                }
            }


            if ((fsm.name == "Hunter Queen Boss" && fsm.gameObject.scene.name == "Memory_Ant_Queen"))
            {
                if (__instance.Name == "Roar")
                {
                    GameObject doubler = Instantiate(fsm.gameObject, fsm.transform.position, fsm.transform.rotation, fsm.transform.parent);
                    doubler.name += "CLONE";
                    fsm.name += "CLONE";

                    var doublerFSMs = doubler.GetComponents<PlayMakerFSM>();
                    foreach (var dfsm in doublerFSMs)
                    {
                        dfsm.Fsm.SetState("Roar");
                    }
                }
            }

            //TROBBIOOOOOOOOO
            if ((fsm.name == "Trobbio" && fsm.gameObject.scene.name == "Library_13"))
            {
                if (__instance.Name == "Land 2")
                {
                    GameObject doubler = Instantiate(fsm.gameObject, fsm.transform.position, fsm.transform.rotation, fsm.transform.parent);
                    doubler.name += "CLONE";
                    fsm.name += "CLONE";

                    var doublerFSMs = doubler.GetComponents<PlayMakerFSM>();
                    foreach (var dfsm in doublerFSMs)
                    {
                        dfsm.Fsm.SetState("Land 2");
                    }
                }
            }

            //TROBBIOOOOOOOOO :(
            if ((fsm.name == "Tormented Trobbio" && fsm.gameObject.scene.name == "Library_13"))
            {
                if (__instance.Name == "Flash Start 2")
                {
                    GameObject doubler = Instantiate(fsm.gameObject, fsm.transform.position, fsm.transform.rotation, fsm.transform.parent);
                    doubler.name += "CLONE";
                    fsm.name += "CLONE";

                    var doublerFSMs = doubler.GetComponents<PlayMakerFSM>();
                    foreach (var dfsm in doublerFSMs)
                    {
                        dfsm.Fsm.SetState("Flash Start 2");
                    }
                }
            }

            //Sister Splinter needs one of these too I guess
            if (fsm.name == "Splinter Queen" && fsm.gameObject.scene.name == "Shellwood_18")
            {
                if (__instance.Name == "Roar 4")
                {
                    GameObject doubler = Instantiate(fsm.gameObject, fsm.transform.position, fsm.transform.rotation, fsm.transform.parent);
                    doubler.name += "CLONE";
                    fsm.name += "CLONE";

                    var doublerFSMs = doubler.GetComponents<PlayMakerFSM>();
                    foreach (var dfsm in doublerFSMs)
                    {
                        dfsm.Fsm.SetState("Roar4");
                    }
                }
            }

        }


        //You know, looking back on this I really should have made a method for a lot of the above code, but I'm kind of too scared to touch it at this point



        //Override doubler originally for Widow. I don't why the default code didn't work for this, and I don't know why this works as a solution, but it does.
        //Also a lot of other random bosses like Signis and Gron???? Why????
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayMakerFSM), "OnEnable")] 
        private static void OnFsmEnabled(PlayMakerFSM __instance)
        {
            //This line is disgustingly long. No, I'm not doing anything about it
            if ((__instance.gameObject.scene.name == "Belltown_Shrine" || __instance.gameObject.scene.name == "Organ_01" || __instance.gameObject.scene.name == "Shellwood_11b_Memory" || __instance.gameObject.scene.name == "Coral_Judge_Arena" || __instance.gameObject.scene.name == "Coral_11" || __instance.gameObject.scene.name == "Slab_10b" || __instance.gameObject.scene.name == "Dock_09") && __instance.name == "Boss Scene" && !__instance.name.Contains("CLONE"))
            {
                GameObject doubler = Instantiate(__instance.gameObject, __instance.transform.position, __instance.transform.rotation, __instance.transform.parent);
                doubler.name += "CLONE";
                __instance.name += "CLONE";
               
            }
        }


        

        //Main enemy doubling code. Has a list of game objects to be doubled, and the code for doubling them.
        private void ModifyEnemies(PlayMakerFSM __instance)
        {
            String[] Keywords = new string[]
            {
                "Mossbone Mother",
                "Mossbone Mother A",
                "Mossbone Mother B",
                "Boss Scene",
                "Lace Boss1",
                "Lace Boss2 New",
                "Dancer Control",
                "Crawfather",
                "Roachkeeper Chef (1)",
                "Coral Conch Driller Giant Solo",
                "Swamp Shaman",
                "Vampire Gnat Boss Scene",
                "Seth",
                "Coral Warrior Grey"
            }; 
            bool isBoss = false;

            foreach (var key in Keywords)
            {
                if(__instance.name == key)
                {
                    isBoss = true;
                    break;
                }
            }

            if (isBoss && !__instance.name.Contains("CLONE") && __instance.gameObject.scene != SceneManager.GetActiveScene())
            {
                GameObject doubler = Instantiate(__instance.gameObject, __instance.transform.position, __instance.transform.rotation, __instance.transform.parent);
                doubler.name += "CLONE";
                __instance.name += "CLONE"; 

                return;
            }
        }

    }
}

