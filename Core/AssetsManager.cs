﻿using System.Collections.Generic;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Serialization;

namespace Fusee.Tutorial.Core.Assets
{
    static class AssetsManager
    {
        public enum FILE_TYPE
        {
            TEXTURE_SKY,
            FUS_DIVERSE,
            SHADER_VERT,
            SHADER_PIX,
            FUS_BUNKER,
            TEXTURE_MAP
        }

        //FILE LOCATIONS
        private const string TEXTURE_SKY_FILEPATH = "Textures/Sky/";
        private const string TEXTURE_MAPS_FILEPATH = "Textures/Landscape/";
        private const string SHADERS_FILEPATH = "Shaders/";
        private const string FUS_BUNKERS_FILEPATH = "Fus/Bunkers/";
        private const string FUS_ROOT_FILEPATH = "Fus/";

        //SHADER FILE NAMES
        private static readonly List<string> SHADER_VERT_FILES = new List<string>() { "VertexShader", "VertexShader_mountain", "VertexShader_texture" };
        private static readonly List<string> SHADER_PIX_FILES = new List<string>() { "PixelShader", "PixelShader_mountains", "PixelShader_texture" };

        //TEXTURE FILE NAMES
        public static readonly List<string> TEXTURE_MAP_FILES = new List<string>() { "mountainsTexture_3", "mountainsTexture_0", "mountainsTexture_4" };
        public static readonly List<string> TEXTURE_SKY_FILES = new List<string>() { "sky_6", "sky_8" };

        //FUS FILE NAMES
        public static readonly string[] FUS_BUNKER_FILES = { "Bunker_white", "Bunker_pink", "Bunker_yellow", "Bunker_green", "Bunker_blue", "Bunker_red" };
        private static readonly string[] FUS_SKY_FILES = { "360Sky" };
        public static readonly string[] FUS_DIVERSE_FILES = { "projectile" };

        //ASSETS STORAGE
        public static Dictionary<string, SceneNodeContainer> fusFiles; 
        public static Dictionary<string, TextureImage> textures;
        public static Dictionary<string, string> shaders_pix;
        public static Dictionary<string, string> shaders_vert;

        public static ContainerClone projectile;

        public static T loadAsset<T>(FILE_TYPE _type, string _filename)
        {
            string _filePath = "";
            switch (_type)
            {
                    case FILE_TYPE.FUS_BUNKER:
                        _filePath = FUS_BUNKERS_FILEPATH + _filename + ".fus";
                        break;
                    case FILE_TYPE.TEXTURE_MAP:
                        _filePath = TEXTURE_MAPS_FILEPATH + _filename + ".png";
                        break;
                    case FILE_TYPE.SHADER_PIX:
                        _filePath = SHADERS_FILEPATH + _filename + ".frag";
                        break;
                    case FILE_TYPE.SHADER_VERT:
                        _filePath = SHADERS_FILEPATH + _filename + ".vert";
                        break;
                    case FILE_TYPE.FUS_DIVERSE:
                        _filePath = FUS_ROOT_FILEPATH + _filename + ".fus";
                        break;
                    case FILE_TYPE.TEXTURE_SKY:
                        _filePath = TEXTURE_SKY_FILEPATH + _filename + ".fus";
                        break;
            }
            return AssetStorage.Get<T>(_filePath);
        }

        public static void loadGameAssets()
        {
            fusFiles = new Dictionary<string, SceneNodeContainer>();
            textures = new Dictionary<string, TextureImage>();
            shaders_pix = new Dictionary<string, string>();
            shaders_vert = new Dictionary<string, string>();

            foreach (var bunkerName in FUS_BUNKER_FILES)
            {
                SceneNodeContainer _bunkerContainer = loadAsset<SceneContainer>(FILE_TYPE.FUS_BUNKER, bunkerName).Children[0];
                renameNodesRecursively(_bunkerContainer, "", "_" + bunkerName);
                fusFiles.Add(bunkerName, _bunkerContainer);
            }

            foreach (var sky in FUS_SKY_FILES)
            {
                fusFiles.Add(sky, loadAsset<SceneContainer>(FILE_TYPE.FUS_DIVERSE, sky).Children[0]);
            }

            foreach (var skyTex in TEXTURE_SKY_FILES)
            {
                ImageData src = loadAsset<ImageData>(FILE_TYPE.TEXTURE_SKY, skyTex);
                string path = TEXTURE_SKY_FILEPATH + skyTex + ".png";

                TextureImage _tex = new TextureImage(src, skyTex, path);
                textures.Add(skyTex, _tex);
            }

            foreach (var mapTex in TEXTURE_MAP_FILES)
            {
                ImageData src = loadAsset<ImageData>(FILE_TYPE.TEXTURE_MAP, mapTex);
                string path = TEXTURE_MAPS_FILEPATH + mapTex + ".png";

                TextureImage _tex = new TextureImage(src, mapTex, path);
                textures.Add(mapTex, _tex);
            }

            foreach (var shader in SHADER_PIX_FILES)
            {
                shaders_pix.Add(shader, loadAsset<string>(FILE_TYPE.SHADER_PIX, shader));
            }

            foreach (var shader in SHADER_VERT_FILES)
            {
                shaders_vert.Add(shader, loadAsset<string>(FILE_TYPE.SHADER_VERT, shader));
            }

            foreach (var fus in FUS_DIVERSE_FILES)
            {
                fusFiles.Add(fus, loadAsset<SceneContainer>(FILE_TYPE.FUS_DIVERSE, fus).Children[0]);
            }

            projectile = new ContainerClone(FUS_DIVERSE_FILES[0]);
        }

        public static void renameNodesRecursively(SceneNodeContainer elem, string _prefix, string _appendix)
        {
            elem.Name = _prefix + elem.Name + _appendix;

            if (elem.Children != null && elem.Children.Count > 0)
            {
                foreach (var child in elem.Children)
                {
                    renameNodesRecursively(child, _prefix, _appendix);
                }
            }
        }

    }
}
