﻿using System.Collections.Generic;
using UnityEngine;

public class MapGenerator {

    // SINGLETON PART

    private static MapGenerator _instance;
    private MapGenerator() { }

    public static MapGenerator GetInstance() {
        if (_instance == null) {
            _instance = new MapGenerator();
        }
        return _instance;
    }

    // LOGIC PART

    private float _xOrg = 0.0f;
    private float _yOrg = 0.0f;
    private float _scale = 1.0f;

    private float _riverFilter = 0.2f;
    private float _forestFilter = 0.55f;

    public void SetParameters(float xOrg, float yOrg, float scale) {
        _xOrg = xOrg;
        _yOrg = yOrg;
        _scale = scale;
    }

    public int[,] GenerateMap(Vector2 size) {
        _xOrg = Random.Range(0.0f, 100.0f);
        _yOrg = Random.Range(0.0f, 100.0f);

        int[,] map = new int[(int)size.y, (int)size.x];
        float[,] noise = GeneratePerlinNoise(size);
        
        for (int y = 0; y < size.y; y++) {
            for (int x = 0; x < size.x; x++) {
                if (map[y,x] != 3) {
                    if (noise[y, x] > _forestFilter) {
                        map[y, x] = 2;
                    }
                    else if (noise[y,x] > _riverFilter) {
                        map[y, x] = 0;
                    }
                    else {
                        map[y, x] = 3;
                    }
                }
            }
        }

        //RiverGeneration.Debug(map);
        List<Vector2> rivers = RiverGeneration.ConnectWaterBodies(map, noise);
        foreach (Vector2 waterCell in rivers) {
            map[(int)waterCell.y, (int)waterCell.x] = 3;
        }

        return map;
    }

    private float[,] GeneratePerlinNoise(Vector2 size) {
        float[,] noise = new float[(int)size.y, (int)size.x];

        for (int y = 0; y < size.y; y++) {
            for (int x = 0; x < size.x; x++) {
                float xCoord = _xOrg + x / size.x * _scale;
                float yCoord = _yOrg + y / size.y * _scale;
                noise[y, x] = Mathf.PerlinNoise(xCoord, yCoord);
            }
        }

        return noise;
    }
}
