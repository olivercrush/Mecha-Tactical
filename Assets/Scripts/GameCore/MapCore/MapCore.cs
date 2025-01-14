﻿using System.Collections;
using UnityEngine;

public class MapCore : GameplayCore {

    private Map _map;

    public MapCore() {

    }

    public Map CreateMap(int w, int h) {
        _map = new Map(MapGenerator.GetInstance().GenerateMap(new Vector2(w, h)));
        return _map;
    }

    public Map GetMap() { return _map; }

    public void HandleUpdate(Update update) {
        switch (update.GetUpdateType()) {
            case UpdateType.GENERATE_MAP:
                CreateMap(int.Parse(update.GetArgs()[0]), int.Parse(update.GetArgs()[1]));
                break;
        }
    }
}
