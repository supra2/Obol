using Core.FightSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PartyManager : Singleton<PartyManager>
{

    #region Members
    #region Hidden
    protected Party _party;
    #endregion
    #endregion

    #region Getter

    public List<Core.Items.Item> Inventory => _party.Inventory;

    public int InventorySize => _party.InventorySize;

    public Party Party => _party;

    #endregion

    #region Initialisation
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    #endregion

    #region Method

    public void UpdateGroup(List<PlayableCharacter> playable_character)
    {
        _party.CharacterParty = playable_character;
    }

    public void Load(string filename)
    {
        string persistantDatapath = System.IO.Path.Combine(Application.persistentDataPath, "Save", "save.json");
#if UNITY_ANDROID && !UNityEditor
        WWW www = new WWW (persistantDatapath);
        while (!www.isDone) {}
        if (string.IsNullOrEmpty(www.error))
        {
            dataAsJson = www.text;
            party = JsonUtility.FromJson<Party> (dataAsJson);
        }
        else
        {
            Debug.Log ("No such file");
        }
        else 
        {
            if (File.Exists (filePath)) 
            {
                dataAsJson = File.ReadAllText (filePath);
                data = JsonUtility.FromJson<TestData> (dataAsJson);
            }
            else
            {
                Debug.Log ("No such file");
            }
        }
#else
        _party = JsonUtility.FromJson<Party>(File.ReadAllText(persistantDatapath));
#endif
    }

    public void Save(string filename)
    {
        string persistantDatapath = System.IO.Path.Combine(Application.persistentDataPath, "Save", "save.json");
#if UNITY_ANDROID
        WWW www = new WWW (persistantDatapath);
#else
        File.WriteAllText(persistantDatapath, JsonUtility.ToJson(_party));
#endif
    }

    public void Debug_Init( List<PlayableCharacter> _character )
    {
        _party = new Party();
        _party.CharacterParty = _character;
        _party.OnFoodChanged+= (X) => FoodChanged?.Invoke(X);
    }

    public Character GetMainCharacter()
    {
        return _party.CharacterParty.Find((X) => X.MainCharacter == true);
    }

    #endregion

    #region Event

    public UnityIntEvent FoodChanged = new UnityIntEvent();


    #endregion

}
