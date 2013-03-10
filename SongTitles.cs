﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;

namespace TransferSSS {
	class SongTitles {
		public static void copy(MSBinNode from, MSBinNode to) {
			for (int i = 0; i < from._strings.Count; i++) {
				string custom = from._strings[i];
				if (custom != originalTitles[i]) {
					to._strings[i] = from._strings[i];
				}
			}
		}

		public static string[] originalTitles = {
			"Super Smash Bros. Brawl Main Theme",
			"Tournament Grid",
			"Midna's Lament",
			"Great Temple / Temple",
			"Dragon Roost Island",
			"The Great Sea",
			"Tal Tal Heights",
			"Song of Storms",
			"Gerudo Valley",
			"Molgera Battle",
			"Village of the Blue Maiden",
			"Termina Field",
			"Tournament Match End",
			"Main Theme (Metroid)",
			"Ending (Metroid)",
			"Norfair",
			"Theme of Samus Aran, Space Warrior",
			"Vs. Ridley",
			"Vs. Parasite Queen",
			"Opening / Menu (Metroid Prime)",
			"Sector 1",
			"Vs. Meta Ridley",
			"Multiplayer (Metroid Prime 2)",
			"Classic: Results Screen",
			"Obstacle Course",
			"Ending (Yoshi's Story)",
			"Yoshi's Island",
			"Flower Field",
			"Wildlands",
			"Meta Knight's Revenge",
			"The Legendary Air Ride Machine",
			"Gourmet Race",
			"Butter Building",
			"King Dedede's Theme",
			"All-Star Rest Area",
			"Squeak Squad Theme",
			"Vs. Marx",
			"0A² Battle",
			"Boss Theme Medley",
			"Checker Knights",
			"Forest / Nature Area",
			"Frozen Hillside",
			"Space Armada",
			"Corneria",
			"Main Theme (Star Fox)",
			"Home-Run Contest",
			"Main Theme (Star Fox 64)",
			"Area 6",
			"Area 6 Ver. 2",
			"Star Wolf",
			"Star Wolf (Star Fox: Assault)",
			"Space Battleground",
			"Break Through the Ice",
			"PokAcmon Main Theme",
			"Road to Viridian City (From Pallet Town / Pewter City)",
			"PokAcmon Center",
			"Boss Battle",
			"PokAcmon Gym / Evolution",
			"Wild PokAcmon Battle! (Ruby / Sapphire)",
			"Victory Road",
			"Dialga / Palkia Battle at Spear Pillar!",
			"Wild PokAcmon Battle! (Diamond / Pearl)",
			"Team Galactic Battle!",
			"Route 209",
			"Mute City",
			"Fire Field",
			"White Land ",
			"Trophy Gallery",
			"Car Select",
			"Dream Chaser",
			"Devil's Call in Your Heart",
			"Climb Up! And Get The Last Chance!",
			"Brain Cleaner",
			"Shotgun Kiss",
			"Planet Colors",
			"Porky's Theme",
			"Unfounded Revenge / Smashing Song of Praise",
			"Mother 3 Love Theme",
			"Sticker Album / Album / Chronicle",
			"You Call This a Utopia?!",
			"Humoresque of a Little Dog",
			"Snowman",
			"Fire Emblem Theme",
			"With Mila's Divine Protection (Celica Map 1)",
			"Attack",
			"Preparing to Advance",
			"Winning Road - Roy's Hope",
			"Shadow Dragon Medley",
			"Ike's Theme",
			"Coin Launcher",
			"Against the Dark Knight",
			"Crimean Army Sortie",
			"Power-Hungry Fool",
			"Victory Is Near",
			"Underworld",
			"Skyworld",
			"Title (Kid Icarus)",
			"Kid Icarus Original Medley",
			"WarioWare, Inc.",
			"WarioWare, Inc. Medley",
			"Stage Builder",
			"Ashley's Song",
			"Ashley's Song (JP)",
			"Mike's Song",
			"Mike's Song (JP)",
			"Mona Pizza's Song",
			"Mona Pizza's Song (JP)",
			"Main Theme (Pikmin)",
			"World Map (Pikmin 2)",
			"Stage Clear / Title (Pikmin)",
			"Forest of Hope",
			"Menu 1",
			"Target Smash!!",
			"Ai no Uta",
			"Ai no Uta (French Version)",
			"Tane no Uta",
			"Environmental Noises",
			"Title (Animal Crossing)",
			"Go K.K. Rider!",
			"2:00 a.m.",
			"Town Hall and Tom Nook's Store",
			"The Roost",
			"Ice Climber",
			"Adventure Map",
			"Balloon Trip",
			"Shin Onigashima",
			"Clu Clu Land",
			"Mario Bros.",
			"Gyromite",
			"Famicom Medley",
			"Power-Up Music",
			"Douchuumen (Nazo no Murasamejo)",
			"Flat Zone 2",
			"Chill (Dr. Mario)",
			"Step: The Plain",
			"PictoChat",
			"Mii Channel",
			"Wii Shop Channel",
			"Shaberu! DS Cooking Navi",
			"Brain Age: Train Your Brain in Minutes a Day",
			"Opening Theme (Wii Sports)",
			"Charge! (Wii Play)",
			"Lip's Theme (Panel de Pon)",
			"Tetris: Type A",
			"Tetris: Type B",
			"Step: The Cave",
			"Title (3D Hot Rally)",
			"Tunnel Scene (X)",
			"Mario Tennis / Mario Golf",
			"Marionation Gear",
			"Title (Big Brain Academy)",
			"Golden Forest (1080A°Snowboarding)",
			"Battle Scene / Final Boss (Golden Sun)",
			"Excite Truck",
			"MGS4 ï½zTheme of Loveï½z Smash Bros. Brawl Version",
			"Encounter",
			"Step: Subspace",
			"Theme of Tara",
			"Battle in the Base",
			"Yell \"Dead Cell\"",
			"Cavern",
			"Snake Eater (Instrumental)",
			"Theme of Solid Snake",
			"Calling to the Night",
			"Green Hill Zone",
			"Angel Island Zone",
			"Scrap Brain Zone",
			"Step: Subspace Ver.2",
			"Emerald Hill Zone",
			"Sonic Boom",
			"Super Sonic Racing",
			"Open Your Heart",
			"Live & Learn",
			"Sonic Heroes",
			"Right There, Ride On",
			"HIS WORLD (Instrumental)",
			"Seven Rings In Hand",
			"K.K. Crusin'",
			"Step: Subspace Ver.3",
			"K.K. Western",
			"K.K. Gumbo",
			"Rockin' K.K.",
			"DJ K.K.",
			"K.K. Condor",
			"Cruel Brawl",
			"Boss Battle Song 1",
			"Boss Battle Song 2",
			"Save Point",
			"Menu 2",
			"Credits",
			"Menu (Super Smash Bros. Melee)",
			"Credits (Super Smash Bros.)",
			"Opening (Super Smash Bros. Melee)",
			"Princess Peach's Castle (Melee)",
			"Rainbow Cruise (Melee)",
			"Kong Jungle (Melee)",
			"Jungle Japes (Melee)",
			"Temple (Melee)",
			"Brinstar (Melee)",
			"Battlefield",
			"Brinstar Depths (Melee)",
			"Yoshi's Island (Melee)",
			"Fountain of Dreams (Melee)",
			"Green Greens (Melee)",
			"Corneria (Melee)",
			"Venom (Melee)",
			"PokAcmon Stadium (Melee)",
			"PokAc Floats (Melee)",
			"Mute City (Melee)",
			"Big Blue (Melee)",
			"Battlefield Ver. 2",
			"Mother (Melee)",
			"Icicle Mountain (Melee)",
			"Flat Zone (Melee)",
			"Super Mario Bros. 3 (Melee)",
			"Battle Theme (Melee)",
			"Fire Emblem (Melee)",
			"Mach Rider (Melee)",
			"Mother 2 (Melee)",
			"Dr. Mario (Melee)",
			"Battlefield (Melee)",
			"Final Destination",
			"Menu (Melee)",
			"Multi-Man Melee 1 (Melee)",
			"Final Destination (Melee)",
			"Giga Bowser (Melee)",
			"Delfino Plaza",
			"Title / Ending (Super Mario World)",
			"Main Theme (New Super Mario Bros.)",
			"Ricco Harbor",
			"Main Theme (Super Mario 64)",
			"Ground Theme (Super Mario Bros.)",
			"Online Practice Stage",
			"Ground Theme 2 (Super Mario Bros.)",
			"Gritzy Desert",
			"Underground Theme (Super Mario Bros.)",
			"Underwater Theme (Super Mario Bros.)",
			"Underground Theme (Super Mario Land)",
			"Luigi's Mansion Theme",
			"Castle / Boss Fortress (Super Mario World / SMB 3)",
			"Airship Theme (Super Mario Bros. 3)",
			"Mario Circuit",
			"Luigi Circuit",
			"Results Display Screen",
			"Waluigi Pinball",
			"Rainbow Road",
			"Jungle Level Ver.2",
			"Jungle Level  ",
			"King K.Rool / Ship Deck 2",
			"Bramble Blast",
			"Battle for Storm Hill",
			"DK Jungle 1 Theme (Barrel Blast)",
			"The Map Page / Bonus Level",
			"Donkey Kong",
			"Tournament Registration",
			"Opening (Donkey Kong)",
			"25m BGM",
			"Main Theme (The Legend of Zelda)",
			"Ocarina of Time Medley",
			"Title (The Legend of Zelda)",
			"The Dark World",
			"Hidden Mountain & Forest",
			"Hyrule Field Theme",
			"Main Theme (Twilight Princess)",
			"The Hidden Village",
		};
	}
}
