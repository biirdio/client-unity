using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

namespace Biird
{
    [CreateAssetMenu(fileName = "Biird Id's", menuName = "Biird/Ids List", order = 1)]
    public class BiirdIds : ScriptableObject
    {
        public TextAsset JsonFile;
     
        private Dictionary<string, object> _allObjects;
        private Dictionary<string, string> _allIds;

        #if NET_4_6
        public Dictionary<string, string> AllIds => _allIds;
        #else
        public Dictionary<string, string> AllIds
        {
            get { return _allIds; }
        }
        #endif
        public void Init()
        {
            _allObjects = Json.Deserialize(JsonFile.text) as Dictionary<string, object>;
            _allIds = new Dictionary<string, string>();
            if (_allObjects != null)
            {
                foreach (var pair in _allObjects)
                {
                    _allIds.Add(pair.Key, pair.Value.ToString());
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        private static BiirdIds _instance;
        public static BiirdIds Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<BiirdIds>("Data/Biird Id's");
                }

                return _instance;
            }
        }
    }
}