using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Profiling;
using UnityEditor.Build.Reporting;


public class BuilOptimizer : EditorWindow
{

    public static List<Texture2D> _AllTexture2DAuto = new List<Texture2D>();

    public static Texture2D ObbAuto;
    public static TextureImporterFormat _TextureFormatAuto = TextureImporterFormat.ASTC_6x6;


    public static int TextursizeAuto;

    public static _platformAuto PlatformName = _platformAuto.Android;
    public static _UserSizeAuto UserSize = _UserSizeAuto._256;
    public static _UserSizeAuto UserSize2 = _UserSizeAuto._1024;


    public static bool toggleTextFieldAuto = false;
    public static int MinimizeSize = 256, MaximumSize = 1024;
    static string[] SceneArray;
    public static List<string> names = new List<string>();

    static string[] Words;
    public enum _platformAuto
    {
        Android = 1,
        iPhone = 2
    }

    public enum _UserSizeAuto
    {
        _32 = 32,
        _64 = 64,
        _128 = 128,
        _256 = 256,
        _512 = 512,
        _1024 = 1024,
        _2048 = 2048,
        _4096 = 4096,
    }

    GUIStyle guistyle = new GUIStyle();
    GUIStyle guistyle1 = new GUIStyle();
    GUIStyle LogoFont = new GUIStyle();
    private static Texture2D tt;


    [MenuItem("GameDistrict/TexturesOptimizer/Auto-Optimize (Recomended)", false, 100)]


    static void showWindow()
    {
#if UNITY_ANDROID
        PlatformName = _platformAuto.Android;
#elif UNITY_IOS
        PlatformName = _platformAuto.iPhone;
#endif

        BuilOptimizer window = (BuilOptimizer)EditorWindow.GetWindowWithRect(typeof(BuilOptimizer), new Rect(0, 0, 450, 700));
        window.titleContent = new GUIContent("Auto Optimizer");
        window.Show();

    }

    void OnGUI()
    {
        try
        {
            GUILayout.BeginHorizontal();


            GUILayout.Label("Version : 1.0", GUILayout.Width(100), GUILayout.Height(20));

            GUILayout.EndHorizontal();

            tt = EditorGUIUtility.Load("Assets/Plugins/TexturesOptimizer/logo.png") as Texture2D;

            GUI.Label(new Rect(145, 25, 173, 186), tt);

            GUILayout.Space(160);

            GUIStyle largeStyle = new GUIStyle(EditorStyles.boldLabel);
            largeStyle.fontSize = 25;

            GUILayout.Space(70);

            PlatformName = (_platformAuto)EditorGUILayout.EnumPopup("Select Your Platform:", PlatformName);

            GUILayout.Space(20);

            _TextureFormatAuto = (TextureImporterFormat)EditorGUILayout.EnumPopup("Select Texture Format:", _TextureFormatAuto);

            GUILayout.Space(15);


            toggleTextFieldAuto = EditorGUI.ToggleLeft(new Rect(5, 330, 300, 25), "Select Custom Size Range For Textures", toggleTextFieldAuto);

            if (toggleTextFieldAuto == true)
            {
                GUILayout.Space(42);
                UserSize = (_UserSizeAuto)EditorGUILayout.EnumPopup("Select Minimum Size:", UserSize);
                MinimizeSize = (int)UserSize;


                GUILayout.Space(20);
                UserSize2 = (_UserSizeAuto)EditorGUILayout.EnumPopup("Select Maximum Size:", UserSize2);
                MaximumSize = (int)UserSize2;



            }
            else
            {
                GUILayout.Space(39);
                UserSize = (_UserSizeAuto)EditorGUILayout.EnumPopup("Select Minimum Size:", UserSize);
                MinimizeSize = (int)UserSize;


                guistyle.fontSize = 10;
                guistyle.normal.textColor = new Color(1f, 0.091f, 0f);
                GUI.Label(new Rect(155, 390, 300, 25), "*Compression Start From Minimum Size", guistyle);




            }
            GUILayout.Space(60);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Optimize From Build!", GUILayout.Width(185), GUILayout.Height(40))) //4
            {
                SetupAndTour();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(50);
            Splitter(new Color(0.35f, 0.35f, 0.35f));



            GUIStyle greyStyle = new GUIStyle(EditorStyles.label);
            greyStyle.fontSize = 12;

            DrawLabelWithFlexibleSpace("*Please Use Simple ASTC_*x* format for Android/iPhone Platform", greyStyle, 20);


            GUILayout.FlexibleSpace();

            GUIStyle SmallStyle = new GUIStyle(EditorStyles.label);
            SmallStyle.fontSize = 13;

            DrawLabelWithFlexibleSpace("By Team Koko Zone", SmallStyle, 50);
            EditorUtility.ClearProgressBar();
        }
        catch (Exception ex)
        {

            Debug.LogError("Unexpected Error, OnGUI");
        }
    }

    static void SetupAndTour()
    {
        // Debug.Log("Comer Scc");

        try
        {
            _AllTexture2DAuto.Clear();
            GetBuildScenes();

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
           
            buildPlayerOptions.scenes = SceneArray;

            buildPlayerOptions.locationPathName = "DetailedAboutBuild/MyGame.exe";
            if (PlatformName.ToString() == "Android")
            {

                buildPlayerOptions.target = BuildTarget.Android;
            }
            else
            {
                buildPlayerOptions.target = BuildTarget.iOS;

            }
            BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);

            //PackedAssets[] PAssets = buildReport.packedAssets;
            PackedAssets[] PAssets = buildReport.packedAssets;

            foreach (PackedAssets e in PAssets)
            {
                foreach (PackedAssetInfo q in e.contents)
                {
                   
                    try
                    {
                        ObbAuto = (Texture2D)AssetDatabase.LoadAssetAtPath(q.sourceAssetPath, typeof(Texture2D));

                        if (ObbAuto != null)
                        {
                            if (ObbAuto.width > ObbAuto.height)
                            {
                                TextursizeAuto = ObbAuto.width;
                            }
                            else
                            {
                                TextursizeAuto = ObbAuto.height;
                            }
                            if (toggleTextFieldAuto == true)
                            {
                                if (TextursizeAuto >= MinimizeSize && TextursizeAuto <= MaximumSize)
                                {
                                    if (!_AllTexture2DAuto.Contains(ObbAuto))
                                        _AllTexture2DAuto.Add(ObbAuto);
                                }
                            }
                            else
                            {

                                if (TextursizeAuto >= MinimizeSize)
                                {
                                    if (!_AllTexture2DAuto.Contains(ObbAuto))

                                        _AllTexture2DAuto.Add(ObbAuto);
                                }

                            }
                        }
                    }
                    catch (Exception qq)
                    {
                        continue;
                    }







                }


            }




            InfoGet();

        }
        catch (Exception ex)
        {
            Debug.LogError("UnExpected Error, SetupAndTour");
        }
    }

    static void InfoGet()
    {
        if (_AllTexture2DAuto.Count > 0)
        {
            
            OptimizerAuto.Work(PlatformName.ToString(), _TextureFormatAuto);

        }
        else
        {
            Debug.LogError("No Files Found! Please Try Again...");
        }
    }

    private static void DrawLabelWithFlexibleSpace(string text, GUIStyle style, int height)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(text, style, new GUILayoutOption[] { GUILayout.Height(height) });
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }



    public static void Splitter(Color rgb, float thickness = 1, int margin = 0)
    {
        GUIStyle splitter = new GUIStyle();
        splitter.normal.background = EditorGUIUtility.whiteTexture;
        splitter.stretchWidth = true;
        splitter.margin = new RectOffset(margin, margin, 7, 7);

        Rect position = GUILayoutUtility.GetRect(GUIContent.none, splitter, GUILayout.Height(thickness));

        if (Event.current.type == EventType.Repaint)
        {
            Color restoreColor = GUI.color;
            GUI.color = rgb;
            splitter.Draw(position, false, false, false, false);
            GUI.color = restoreColor;
        }
    }

    static void GetBuildScenes()
    {
        names.Clear();
        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
            {
                continue;
            }

            if (e.enabled)
            {
                //Debug.Log("Come" + e.path);

                names.Add(e.path);

            }
        }

        SceneArray = names.ToArray();
    }

}


public class OptimizerAuto : MonoBehaviour
{
    static Texture2D _Texture;
    public static void Work(string Platform, TextureImporterFormat op)
    {

        int size;

        for (int i = 0; i < BuilOptimizer._AllTexture2DAuto.Count; i++)
        {
            try
            {

                _Texture = BuilOptimizer._AllTexture2DAuto[i];
                TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(BuilOptimizer._AllTexture2DAuto[i]));

                if (BuilOptimizer._AllTexture2DAuto[i].width > BuilOptimizer._AllTexture2DAuto[i].height)
                {
                    size = BuilOptimizer._AllTexture2DAuto[i].width;
                }
                else
                {
                    size = BuilOptimizer._AllTexture2DAuto[i].height;
                }

                EditorUtility.DisplayProgressBar("Please Wait...", "Converting Textures : " + i + " Out Of " + BuilOptimizer._AllTexture2DAuto.Count, BuilOptimizer._AllTexture2DAuto.Count);

                //  importer.textureType = TextureImporterType.Sprite;

                importer.SetPlatformTextureSettings(Platform, size, op, 100, false);

                //importer.spriteImportMode = SpMode;

                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();

            }
            catch (System.Exception ex)
            {
                continue;
            }
        }

        EditorUtility.ClearProgressBar();
        BuilOptimizer._AllTexture2DAuto.Clear();
        System.GC.Collect();

        Debug.Log("Successfuly Converted");

    }
}




