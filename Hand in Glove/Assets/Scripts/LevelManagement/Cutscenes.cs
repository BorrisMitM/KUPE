using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Cutscenes")]
public class Cutscenes : ScriptableObject {
    public List<CutsceneInfo> cutsceneInfo;
    public string GetCutsceneName(string _levelName)
    {
        foreach(CutsceneInfo ci in cutsceneInfo)
        {
            if (ci.levelName == _levelName && ci.before)
                return ci.cutsceneName;
        }
        return null;
    }
    public string GetCutSceneNameAfterLevel(string _levelName)
    {
        if (_levelName.EndsWith("K")) _levelName = _levelName.Remove(_levelName.Length - 1);
        else if (_levelName.EndsWith("PE")) _levelName = _levelName.Remove(_levelName.Length - 2, 2);
        else if (_levelName.EndsWith("KUP")) _levelName = _levelName.Remove(_levelName.Length - 3, 3);
        foreach (CutsceneInfo ci in cutsceneInfo)
        {
            if (ci.before) continue;

            if (ci.levelName == _levelName)
                return ci.cutsceneName;
        }
        return null;
    }
    [System.Serializable]
    public class CutsceneInfo
    {
        public bool before = true;
        public string levelName;
        public string cutsceneName;
    }
}
