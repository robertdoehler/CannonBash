﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using System.Diagnostics;

namespace Fusee.Tutorial.Core
{

    static class MapGenerator
    {
        public static Dictionary<string, MapTile> positionIndex;
        public static float2 mapSize = float2.One;
        public static float tileLength = 1.2f;
        public static SceneNodeContainer generate()
        {
            SceneNodeContainer tileMap = new SceneNodeContainer();
            tileMap.Components = new List<SceneComponentContainer>();
            tileMap.Children = new List<SceneNodeContainer>();
            
            TransformComponent transComp = new TransformComponent();
            transComp.Rotation = float3.Zero;
            transComp.Scale = float3.One;
            transComp.Translation = float3.One;
            positionIndex = new Dictionary<string, MapTile>();

            MeshComponent meshComp = new MeshComponent();
            meshComp.BoundingBox = new AABBf(float3.Zero, float3.Zero);

            tileMap.Components.Add(transComp);
            tileMap.Components.Add(meshComp);

            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    MapTile tile = new MapTile("tile." + x + "." + y, new float3(tileLength * x, 0, tileLength * y));
                    tileMap.Children.Add(tile);
                    positionIndex.Add(x + "," + y, tile);
                }
            }


            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    for (int ix = -1; ix <= 1; ix++)
                    {
                        for (int iy = -1; iy <= 1; iy++)
                        {
                            var neighbourX = x + ix;
                            var neighbourY = y + iy;

                            float3 currentTileUR = positionIndex[x + "," + y].GetTransform().Translation;
                            float3 currentTileLR = new float3(currentTileUR.x, currentTileUR.y, currentTileUR.z - tileLength);
                            float3 currentTileUL = new float3(currentTileUR.x - tileLength, currentTileUR.y, currentTileUR.z);
                            float3 currentTileLL = new float3(currentTileUR.x - tileLength, currentTileUR.y, currentTileUR.z - tileLength);
                            float mapJointScale = 0.2f;

                            if (neighbourX > 1 && neighbourX < mapSize.x && neighbourY > 0 && neighbourY < mapSize.y && neighbourX != x && neighbourY != y)
                            {
                                positionIndex[x + "," + y].neighbours.Add(positionIndex[neighbourX + "," + neighbourY]);
                                var neighbour = positionIndex[neighbourX + "," + neighbourY];

                                MapTile mapJointX = new MapTile("MapJointX", new float3(currentTileUR.x - mapJointScale, currentTileUL.y, currentTileUL.z));
                                mapJointX.GetTransform().Scale.x = 0.2f;
                                tileMap.Children.Add(mapJointX);
                            }

                            if (neighbourX > 0 && neighbourX < mapSize.x && neighbourY > 1 && neighbourY < mapSize.y && neighbourX != x && neighbourY != y)
                            {
                                positionIndex[x + "," + y].neighbours.Add(positionIndex[neighbourX + "," + neighbourY]);
                                var neighbour = positionIndex[neighbourX + "," + neighbourY];

                                MapTile mapJointY = new MapTile("MapJointY", new float3(currentTileLR.x, currentTileLL.y, currentTileLL.z + 1));
                                mapJointY.GetTransform().Scale.z = 0.2f;
                                tileMap.Children.Add(mapJointY);
                            }
                        }
                    }
                }
            }
                    tileMap.Name = "TileMap";

            return tileMap;
        }
    }
}