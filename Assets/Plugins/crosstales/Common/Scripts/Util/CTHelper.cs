﻿#if UNITY_EDITOR
using UnityEngine;

namespace Crosstales.Common.Util
{
   /// <summary>Helper to reset the necessary settings.</summary>
   public class CTHelper : MonoBehaviour
   {
      #region Properties

      public static CTHelper Instance { get; private set; }

      #endregion


      #region Initalize methods

      [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
      private static void initialize()
      {
         //Debug.Log("initalize");
         Crosstales.Common.Util.BaseHelper.ApplicationIsPlaying = true;
      }

      [RuntimeInitializeOnLoadMethod]
      private static void create()
      {
         //Debug.Log("create");
         Crosstales.Common.Util.BaseHelper.ApplicationIsPlaying = true;

         if (!Crosstales.Common.Util.BaseHelper.isEditorMode)
         {
            GameObject go = new GameObject("_CTHelper");
            go.AddComponent<CTHelper>();
            DontDestroyOnLoad(go);
         }
      }

      #endregion


      #region MonoBehaviour methods

      private void Awake()
      {
         Instance = this;
      }

      /*
      private void OnApplicationQuit()
      {
         Debug.Log("OnApplicationQuit", this);
         //BaseHelper.ApplicationIsPlaying = false;
      }
*/
      private void OnDestroy()
      {
         //Debug.Log("OnDestroy", this);
         Crosstales.Common.Util.BaseHelper.ApplicationIsPlaying = false;
      }

      #endregion
   }

   [UnityEditor.CustomEditor(typeof(CTHelper))]
   public class CTHelperEditor : UnityEditor.Editor
   {
      public override void OnInspectorGUI()
      {
         UnityEditor.EditorGUILayout.HelpBox("This helper ensures the flawless working of the assets from 'crosstales LLC' inside the Editor.\nPlease do not delete it from the hierarchy.", UnityEditor.MessageType.Info);
      }
   }
}
#endif
// © 2020-2024 crosstales LLC (https://www.crosstales.com)