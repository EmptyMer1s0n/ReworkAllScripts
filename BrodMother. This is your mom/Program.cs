//Jointly with Evervolv and Vick.//
using System;
using System.Linq;
using System.Collections.Generic;

using Ensage;
using SharpDX;
using Ensage.Common.Extensions;
using Ensage.Common;
using SharpDX.Direct3D9;
using System.Windows.Input;
using EZGUI;

namespace This_is_your_Mom
{
    internal class Program
    {
        //
        private static bool activChase;
        private static bool activCombo;
        private static Ability Q, R;
        private static Item mom, abyssal, Soul, orchid, shiva, halberd, mjollnir, satanic, dagon, medall, orhid, sheep, cheese;
        private static Font txt, not;
        private static int spiderDenies = 58;
        private static int spiderDmgStatick = 155;
        private static int spiderDmg;
        private static ParticleEffect rangeDisplay;
        private static Key keyCHASING = Key.F;
        private static Key keyCOMBO = Key.Space;
        private static readonly uint[] spiderQ = { 74, 149, 224, 299 };
        private static readonly uint[] SoulLvl = { 120, 190, 270, 360 };
        //
        private static EzGUI gui;
        private static EzElement toggle, combo, toggleQ, toggles, toggleLasthit, chasing, chasingDisplay, combing, combingDisplay;

        static void Main(string[] args)
        {
            Game.OnUpdate += Game_OnUpdate;
            Game.OnWndProc += Game_OnWndProc;

            txt = new Font(
               Drawing.Direct3DDevice9,
               new FontDescription
               {
                   FaceName = "Tahoma",
                   Height = 12,
                   OutputPrecision = FontPrecision.Default,
                   Quality = FontQuality.Default
               });

            not = new Font(
               Drawing.Direct3DDevice9,
               new FontDescription
               {
                   FaceName = "Tahoma",
                   Height = 20,
                   OutputPrecision = FontPrecision.Default,
                   Quality = FontQuality.Default
               });

            gui = new EzGUI(200, 570, "Broodmother#");
            toggle = new EzElement(ElementType.CHECKBOX, "Enabled", true);
            toggleQ = new EzElement(ElementType.CHECKBOX, "Toggle Q", true);
            toggleLasthit = new EzElement(ElementType.CHECKBOX, "Toggle Lasthit", true);
            combo = new EzElement(ElementType.CATEGORY, "Combo", true);
            toggles = new EzElement(ElementType.CATEGORY, "Toggles", true);
            chasing = new EzElement(ElementType.CHECKBOX, "Chasing", true);
            combing = new EzElement(ElementType.CHECKBOX, "Combing", true);
            chasingDisplay = new EzElement(ElementType.TEXT, "Press " + keyCHASING + " for Chasing", false);
            combingDisplay = new EzElement(ElementType.TEXT, "Press " + keyCOMBO + " for Combing", false);

            gui.AddMainElement(toggle);
            gui.AddMainElement(toggles);
            toggles.AddElement(toggleQ);
            toggles.AddElement(toggleLasthit);
            gui.AddMainElement(combo);
            combo.AddElement(chasing);
            combo.AddElement(chasingDisplay);
            combo.AddElement(combing);
            combo.AddElement(combingDisplay);
        }

        public static void Game_OnUpdate(EventArgs args)
        {
            var me = ObjectMgr.LocalHero;
            if ((!Game.IsInGame || me.ClassID != ClassID.CDOTA_Unit_Hero_Broodmother))
            {
                return;
            }

            if (toggle.isActive && Utils.SleepCheck("combo") && !Game.IsPaused)
            {
                if (Q == null)
                    Q = me.Spellbook.SpellQ;


                if (Soul == null)
                    Soul = me.FindItem("item_soul_ring");


                var spiderlingsLevel = me.Spellbook.SpellQ.Level - 1;

                var myHero = ObjectMgr.LocalHero;

                var enemies = ObjectMgr.GetEntities<Hero>().Where(hero => hero.IsAlive && !hero.IsIllusion && hero.IsVisible && hero.Team != me.Team).ToList();

                var creeps = ObjectMgr.GetEntities<Creep>().Where(creep => (creep.ClassID == ClassID.CDOTA_BaseNPC_Creep_Lane || creep.ClassID == ClassID.CDOTA_BaseNPC_Creep_Siege || creep.ClassID == ClassID.CDOTA_BaseNPC_Creep_Neutral ||
                (creep.ClassID == ClassID.CDOTA_Unit_VisageFamiliar && creep.Team == me.GetEnemyTeam()) || (creep.ClassID == ClassID.CDOTA_Unit_SpiritBear && creep.Team == me.GetEnemyTeam()) || (creep.ClassID == ClassID.CDOTA_BaseNPC_Invoker_Forged_Spirit &&
                creep.Team == me.GetEnemyTeam()) || creep.ClassID == ClassID.CDOTA_BaseNPC_Creep &&
                creep.IsAlive && creep.IsVisible && creep.IsSpawned) && creep.Health <= 259).ToList();

                var creepQ = ObjectMgr.GetEntities<Creep>().Where(creep => (creep.ClassID == ClassID.CDOTA_BaseNPC_Creep_Lane || creep.ClassID == ClassID.CDOTA_BaseNPC_Creep_Siege || creep.ClassID == ClassID.CDOTA_BaseNPC_Creep_Neutral ||
                creep.ClassID == ClassID.CDOTA_Unit_SpiritBear || creep.ClassID == ClassID.CDOTA_BaseNPC_Invoker_Forged_Spirit || creep.ClassID == ClassID.CDOTA_BaseNPC_Creep &&
                creep.IsAlive && creep.IsVisible && creep.IsSpawned)).ToList();

                var Spiderlings = ObjectMgr.GetEntities<Unit>().Where(spiderlings => spiderlings.ClassID == ClassID.CDOTA_Unit_Broodmother_Spiderling).ToList();

               
                // Creep Q lasthit
                if(toggleQ.isActive)
                foreach (var creep in creepQ)
                {
                    if (Q.CanBeCasted() && creep.Health <= Math.Floor((spiderQ[spiderlingsLevel]) * (1 - creep.MagicDamageResist)) && creep.Health > 45 && creep.Team != me.Team)
                    {
                        if (Q.CanBeCasted() && creep.Position.Distance2D(me.Position) <= 600 && Utils.SleepCheck("QQQ"))
                        {
                            if (Soul != null && Soul.CanBeCasted())
                            {
                                Soul.UseAbility();
                                Utils.Sleep(300, "QQQ");
                            }
                            else
                                Q.UseAbility(creep);
                            Utils.Sleep(300, "QQQ");

                        }
                    }
                }
                if (toggleQ.isActive)
                    foreach (var creep in creepQ)
                {
                    if (me.Mana < Q.ManaCost && creep.Health <= Math.Floor((spiderQ[spiderlingsLevel]) * (1 - creep.MagicDamageResist)) && creep.Health > 55 && creep.Team != me.Team)
                    {
                        if (creep.Position.Distance2D(me.Position) <= 600 && Utils.SleepCheck("QQQ"))
                        {
                            if (Soul != null && Soul.CanBeCasted())
                            {
                                Soul.UseAbility();
                                Utils.Sleep(300, "QQQ");
                            }

                        }
                    }
                }
                // Enemy Q lasthit
                foreach (var enemy in enemies)
                {
                    if (Q.CanBeCasted() && enemy.Health <= (spiderQ[spiderlingsLevel]) && enemy.Health > 0)
                    {
                        if (enemy.Position.Distance2D(me.Position) <= 600 && Utils.SleepCheck("QQQ"))
                        {
                            if (Soul.CanBeCasted() && Soul != null)
                            {
                                Soul.UseAbility();
                                Utils.Sleep(300, "QQQ");
                            }
                            else
                                Q.UseAbility(enemy);
                            Utils.Sleep(300, "QQQ");

                        }
                    }
                }


                // Autodenies
                if (Utils.SleepCheck("attacking1") && toggleLasthit.isActive)
                {
                    foreach (var Spider in Spiderlings)
                    {
                        if (Spider.Health > 0 && Spider.Health <= spiderDenies)
                            foreach (var spiderlings in Spiderlings)
                            {
                                if (Spider.Position.Distance2D(spiderlings.Position) <= 500 && Utils.SleepCheck(spiderlings.Handle.ToString()))
                                {
                                    spiderlings.Attack(Spider);
                                    Utils.Sleep(700, spiderlings.Handle.ToString());
                                   
                                }
                            }
                    }
                    Utils.Sleep(700, "attacking1");
                }

                // Auto spider deny and lasthit
                if (Utils.SleepCheck("attacking") && toggleLasthit.isActive)
                {
                    foreach (var creep in creeps)
                    {
                        var Spiderling = ObjectMgr.GetEntities<Unit>().FirstOrDefault(x => x.ClassID == ClassID.CDOTA_Unit_Broodmother_Spiderling && x.IsAlive && x.IsControllable && x.Team == me.Team);

                        foreach (var spiderling in Spiderlings)
                        {
                            if (creep != null)
                            {

                                if (creep.Position.Distance2D(spiderling.Position) <= 800 &&
                                    creep.Team != me.Team && creep.Health > 0 && creep.Health < Math.Floor(spiderDmgStatick * (1 - creep.DamageResist)))
                                {
                                    {
                                        spiderling.Attack(creep);
                                    }
                                }
                                else if (creep.Position.Distance2D(spiderling.Position) <= 500 &&
                                    creep.Team == me.Team && creep.Health > 0 && creep.Health < Math.Floor(spiderDmgStatick * (1 - creep.DamageResist)))
                                {
                                    spiderling.Attack(creep);
                                }
                            }
                        }
                    }
                    Utils.Sleep(850, "attacking");
                }

                // Auto spider enemy lasthit
                if (Utils.SleepCheck("attacking_enemy") && toggleLasthit.isActive)
                {
                    foreach (var enemy in enemies)
                    {
                        foreach (var spiderling in Spiderlings)
                        {
                            if (enemy != null)
                            {
                                spiderDmg = Spiderlings.Count(y => y.Distance2D(enemy) < 800) * spiderling.MinimumDamage + 20;

                                if ((enemy.Position.Distance2D(spiderling.Position)) <= 800 &&
                                    enemy.Team != me.Team && enemy.Health > 0 && enemy.Health < Math.Floor(spiderDmg * (1 - enemy.DamageResist)) + 50)
                                {
                                    spiderling.Attack(enemy);
                                }
                            }
                        }
                    }
                    Utils.Sleep(750, "attacking_enemy");
                }

                if (activChase)
                    Chasing();
                if (activCombo)
                    ChasingAll();

                Utils.Sleep(290, "combo");
            }
        }

        private static void Chasing()
        {
            if (Utils.SleepCheck("All"))
            {
                var me = ObjectMgr.LocalHero;
                var target = me.ClosestToMouseTarget(1500);
                var Spiderlings = ObjectMgr.GetEntities<Unit>().Where(spiderlings => spiderlings.ClassID == ClassID.CDOTA_Unit_Broodmother_Spiderling).ToList();
                {
                    if (target != null && target.IsAlive && !target.IsIllusion && chasing.isActive)
                    {

                        {
                            foreach (var Spider in Spiderlings)
                                if (Spider.Distance2D(target) <= 1000 && (Utils.SleepCheck("combo")))
                                {
                                    Spider.Attack(target);
                                }
                            Utils.Sleep(750, "combo");
                        }
                    }
                    else
                    {
                        if (Utils.SleepCheck("combo"))
                        {
                            foreach (var Spider in Spiderlings)
                            {
                                Spider.Move(Game.MousePosition);
                            }

                        }
                        Utils.Sleep(750, "combo");
                    }

                }
            }
            Utils.Sleep(350, "All");
        }


        private static void ChasingAll()
        {

            if (Utils.SleepCheck("All"))
            {

                var me = ObjectMgr.LocalHero;
                var target = me.ClosestToMouseTarget(1500);
                var Spiderlings = ObjectMgr.GetEntities<Unit>().Where(spiderlings => spiderlings.ClassID == ClassID.CDOTA_Unit_Broodmother_Spiderling).ToList();


                {
                    if (target != null && target.IsAlive && !target.IsIllusion  && chasing.isActive)

                    {
                        {
                            if (Utils.SleepCheck("combing"))
                                foreach (var Spider in Spiderlings)
                                {
                                    if (Spider.Distance2D(target) <= 1500)
                                    {
                                        Spider.Attack(target);

                                    }

                                }
                            Utils.Sleep(550, "combing");
                        }

                    }
                    if (Utils.SleepCheck("combo"))
                        foreach (var Spider in Spiderlings)
                        {
                            if (Spider.Distance2D(target) >= 1500)
                            {
                                Spider.Move(Game.MousePosition);
                            }
                        }
                    Utils.Sleep(700, "combo");


                    var linkens = target.Modifiers.Any(x => x.Name == "modifier_item_spheretarget") || target.Inventory.Items.Any(x => x.Name == "item_sphere");

                    var enemies = ObjectMgr.GetEntities<Hero>().Where(hero => hero.IsAlive && !hero.IsIllusion && hero.IsVisible && hero.Team != me.Team).ToList();

                    {
                        if (target != null && target.IsAlive && !target.IsIllusion && me.Distance2D(target) <= 1000 && Utils.SleepCheck("combo") && chasing.isActive)
                        {

                            foreach (var Enemy in enemies)
                            {
                                if (me.Distance2D(target) <= 1000 && Utils.SleepCheck("combing"))
                                {
                                    Q.UseAbility(target);
                                    Utils.Sleep(350, "combing");
                                }
                            }
                        }
                        else
                        {
                            me.Attack(target);
                        }
                        Utils.Sleep(470, "combo");
                    }


                    if (Q == null)
                        Q = me.Spellbook.SpellQ;

                    /*  if (W == null) ///////It will be added later//////////
                          W = me.Spellbook.SpellW; */

                    if (R == null)
                        R = me.Spellbook.SpellR;

                    // Item

                    if (sheep == null)
                        sheep = target.ClassID == ClassID.CDOTA_Unit_Hero_Tidehunter ? null : me.FindItem("item_sheepstick");


                    if (cheese == null)
                        cheese = me.FindItem("item_cheese");

                    if (orchid == null)
                        orchid = me.FindItem("item_orchid");

                    if (Soul == null)
                        Soul = me.FindItem("item_soul_ring");

                    if (shiva == null)
                        shiva = me.FindItem("item_shivas_guard");

                    dagon = me.GetDagon();

                    if (mom == null)
                        mom = me.FindItem("item_mask_of_madness");

                    if (abyssal == null)
                        abyssal = me.FindItem("item_abyssal_blade");

                    if (mjollnir == null)
                        mjollnir = me.FindItem("item_mjollnir");

                    if (halberd == null)
                        halberd = me.FindItem("item_heavens_halberd");

                    if (medall == null)
                        medall = me.FindItem("item_medallion_of_courage") ?? me.FindItem("item_solar_crest");

                    if (satanic == null)
                        satanic = me.FindItem("item_satanic");

                    if ( // Q Skill
                           Q != null &&
                           Q.CanBeCasted() &&
                           me.CanCast() &&
                           !target.IsMagicImmune() &&
                           me.Distance2D(target) <= 600 &&
                           Utils.SleepCheck("Q")
                           )

                    {
                        Q.UseAbility(target);
                        Utils.Sleep(250 + Game.Ping, "Q");
                    } // Q Skill end




                    if (//R Skill
                        R != null &&
                        R.CanBeCasted() &&
                        me.CanCast() &&
                        me.Distance2D(target) <= 350 &&
                        Utils.SleepCheck("R")
                        )
                    {
                        R.UseAbility();
                        Utils.Sleep(250 + Game.Ping, "R");
                    } // R Skill end


                    if ( // orchid
                        orchid != null &&
                        orchid.CanBeCasted() &&
                        me.CanCast() &&
                        !target.IsMagicImmune() &&
                        !linkens &&
                        Utils.SleepCheck("orchid") &&
                        me.Distance2D(target) <= 1000
                        )
                    {
                        orchid.UseAbility(target);
                        Utils.Sleep(250 + Game.Ping, "orchid");
                    } // orchid Item end

                    if (// Soul Item 
                        Soul != null &&
                        me.Health / me.MaximumHealth <= 0.5 &&

                        me.Mana <= Q.ManaCost &&
                        Soul.CanBeCasted())
                    {
                        Soul.UseAbility();
                    } // Soul Item end

                    if (// Shiva Item
                        shiva != null &&
                        shiva.CanBeCasted() &&
                        me.CanCast() &&
                        !target.IsMagicImmune() &&
                        Utils.SleepCheck("shiva") &&
                        me.Distance2D(target) <= 600
                        )
                    {
                        shiva.UseAbility();
                        Utils.Sleep(250 + Game.Ping, "shiva");
                    } // Shiva Item end

                    if (// MOM
                        mom != null &&
                        mom.CanBeCasted() &&
                        me.CanCast() &&
                        Utils.SleepCheck("mom") &&
                        me.Distance2D(target) <= 700
                        )
                    {
                        mom.UseAbility();
                        Utils.Sleep(250 + Game.Ping, "mom");
                    } // MOM Item end

                    if ( // Medall
                        medall != null &&
                        medall.CanBeCasted() &&

                        Utils.SleepCheck("Medall") &&
                        me.Distance2D(target) <= 500
                        )
                    {
                        medall.UseAbility(target);
                        Utils.Sleep(250 + Game.Ping, "Medall");
                    } // Medall Item end

                    if ( // Abyssal Blade
                        abyssal != null &&
                        abyssal.CanBeCasted() &&
                        me.CanCast() &&
                        !target.IsMagicImmune() &&
                        Utils.SleepCheck("abyssal") &&
                        me.Distance2D(target) <= 400
                        )
                    {
                        abyssal.UseAbility(target);
                        Utils.Sleep(250 + Game.Ping, "abyssal");
                    } // Abyssal Item end

                    if ( // Hellbard
                        halberd != null &&
                        halberd.CanBeCasted() &&
                        me.CanCast() &&
                        !target.IsMagicImmune() &&
                        Utils.SleepCheck("halberd") &&
                        me.Distance2D(target) <= 700
                        )
                    {
                        halberd.UseAbility(target);
                        Utils.Sleep(250 + Game.Ping, "halberd");
                    } // Hellbard Item end

                    if ( // Mjollnir
                        mjollnir != null &&
                        mjollnir.CanBeCasted() &&
                        me.CanCast() &&
                        !target.IsMagicImmune() &&
                        Utils.SleepCheck("mjollnir") &&
                        me.Distance2D(target) <= 600
                       )
                    {
                        mjollnir.UseAbility(me);
                        Utils.Sleep(250 + Game.Ping, "mjollnir");
                    } // Mjollnir Item end

                    if (// Dagon
                        dagon != null &&
                        dagon.CanBeCasted() &&
                        me.CanCast() &&
                        !target.IsMagicImmune() &&
                        Utils.SleepCheck("dagon")
                       )
                    {
                        dagon.UseAbility(target);
                        Utils.Sleep(250 + Game.Ping, "dagon");
                    } // Dagon Item end


                    if (// Satanic 
                        satanic != null &&
                        me.Health / me.MaximumHealth <= 0.4 &&
                        satanic.CanBeCasted() &&
                        me.Distance2D(target) <= 300
                         &&
                        Utils.SleepCheck("Satanic")
                        )
                    {
                        satanic.UseAbility();
                        Utils.Sleep(250 + Game.Ping, "Satanic");
                    } // Satanic Item end
                }
            }
            Utils.Sleep(290, "All");
        }






        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (!Game.IsChatOpen)
            {
                if (Game.IsKeyDown(keyCHASING))
                {
                    activChase = true;
                    chasingDisplay.Content = "CHASING ON!";
                }
                else
                {
                    activChase = false;
                    chasingDisplay.Content = "Press " + keyCHASING + " for chasing";
                }


                if (Game.IsKeyDown(keyCOMBO))
                {
                    activCombo = true;
                    combingDisplay.Content = "COMBO ON!";
                }
                else
                {
                    activCombo = false;
                    combingDisplay.Content = "Press " + keyCOMBO + " for All Combo";
                }
            }
        }
    }
}
