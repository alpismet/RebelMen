using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Dev.Scripts.Tools
{
    #region ####################       Extentions        ###########################

    ////////////////////////////////////////////////////////////////////////////////////////
    public static class GameObjectExtention
    {
        public delegate void CallBack();
        
        
        
        //--------------------------------------------------------------------------
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Fade out/in CanvasGroup items at Canvas via tween.
        /// </summary>
        /// <param name="value">True/False</param>
        public static void SetActiveFade(this GameObject go, bool value, float duration = 1f, float delay = 0, Ease ease = Ease.Linear, CallBack callBack = null)
        {
            go.SetActive(true);

            go.TryGetComponent(out CanvasGroup canvasGroup);

            if (canvasGroup)
            {
                canvasGroup.alpha = value ? 0 : 1;
                canvasGroup.DOFade(value ? 1 : 0, duration).
                    SetEase(ease).SetDelay(delay)
                    .OnComplete(() =>
                    {
                        go.SetActive(value);
                        callBack?.Invoke();
                    });
            }
            else
                Debug.LogWarning("There is no #CanvasGroup#");
        }


        //---------------------------------------------------------------------------------
        /// <summary>
        /// Get all children.
        /// </summary>
        /// <returns>Get all children as GameObject</returns>
        public static List<GameObject> GetChildren(this GameObject go)
        {
            List<GameObject> children = new List<GameObject>();
            for (int i = 0; i < go.transform.childCount; i++)
            {
                children.Add(go.transform.GetChild(i).gameObject);
            }
            return children;
        }


        //---------------------------------------------------------------------------------
        /// <summary>
        /// Get target child as GameObject.
        /// </summary>
        /// <param name="index">Target child index</param>
        /// <returns>Get children as GameObject</returns>
        public static GameObject GetChild(this GameObject go, int index)
        {
            if (go.transform.childCount > index)
                return go.transform.GetChild(index).gameObject;
        
            return null;
        }


        //---------------------------------------------------------------------------------
        public static void SetActive(this GameObject go, bool value, float switchTime)
        {
            go.SetActive(value);
            TweenCallback callback = () => SwitchActive(go);
            DOVirtual.DelayedCall(switchTime, callback);
        }


        //---------------------------------------------------------------------------------
        public static void SwitchActive(this GameObject go)
        {
            go.SetActive(!go.activeSelf);
        }
    }



    ////////////////////////////////////////////////////////////////////////////////////////
    public static class TransformExtention
    {

        //---------------------------------------------------------------------------------
        /// <summary>
        /// Get all children.
        /// </summary>
        /// <returns>Get all children as Transform</returns>
        public static List<Transform> GetChildren(this Transform trans)
        {
            List<Transform> children = new List<Transform>();
            for (int i = 0; i < trans.childCount; i++)
            {
                children.Add(trans.GetChild(i));
            }
            return children;
        }

        //---------------------------------------------------------------------------------
        public static void SetActive(this Transform trans, bool value)
        {
            trans.gameObject.SetActive(value);
        }
        
    
        //---------------------------------------------------------------------------------
        public static void SetPositionX(this Transform transform, float value)
        {
            transform.position = new Vector3(value, transform.position.y, transform.position.z);
        }


        //---------------------------------------------------------------------------------
        public static void SetPositionY(this Transform transform, float value)
        {
            transform.position = new Vector3( transform.position.x, value, transform.position.z);
        }
    

        //---------------------------------------------------------------------------------
        public static void SetPositionZ(this Transform transform, float value)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, value);
        }
    
    
        //---------------------------------------------------------------------------------
        public static void SetPositionWithoutX(this Transform transform, Vector3 pos)
        {
            transform.position = new Vector3(transform.position.x, pos.y,pos.z);
        }


        //---------------------------------------------------------------------------------
        public static void SetPositionWithoutY(this Transform transform, Vector3 pos)
        {
            transform.position = new Vector3(pos.x, transform.position.y,pos.z);
        }
    
    
        //---------------------------------------------------------------------------------
        public static void SetPositionWithoutZ(this Transform transform, Vector3 pos)
        {
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }

    
        //---------------------------------------------------------------------------------
        public static void DestroyChildren(this Transform transform)
        {
            List<Transform> children = transform.GetChildren();
            for (int i = 0; i < children.Count; i++)
                MonoBehaviour.DestroyImmediate(children[i].gameObject);
        }

    
        //---------------------------------------------------------------------------------
        public static void SyncWith(this Transform transform, Transform targetTransform)
        {
            transform.position = targetTransform.position;
            transform.rotation = targetTransform.rotation;
            transform.localScale = targetTransform.localScale;
        }
    }



    ////////////////////////////////////////////////////////////////////////////////////////
    public static class ColorExtention
    {

        //---------------------------------------------------------------------------------
        /// <summary>
        /// Checks If two colors are approximately similar to each other
        /// </summary>
        /// <param name="colorB">Second Color</param>
        /// <param name="similarity">Similarity</param>
        /// <returns>True/False</returns>
        public static bool CompareApproximately(this Color colorA, Color colorB, float similarity)
        {
            return Mathf.Abs(colorA.r - colorB.r) < similarity &&
                   Mathf.Abs(colorA.g - colorB.g) < similarity &&
                   Mathf.Abs(colorA.b - colorB.b) < similarity &&
                   Mathf.Abs(colorA.a - colorB.a) < similarity;
        }


        //---------------------------------------------------------------------------------
        public static Color Tincture(this Color color, float intensity)
        {
            color *= UnityEngine.Random.Range(1f-intensity,1f+intensity);
            return color;
        }


        //---------------------------------------------------------------------------------
        public static bool CompareBase(this Color colorA, Color colorB)
        {
            colorA.a = 255;
            colorB.a = 255;
            return colorA==colorB;
        }


        //---------------------------------------------------------------------------------
        public static Color Random(this Color color)
        {
            return new Color(UnityEngine.Random.Range(0,1f),UnityEngine.Random.Range(0,1f),UnityEngine.Random.Range(0,1f));
        }


        //---------------------------------------------------------------------------------
        public static Color Random(this IEnumerable<Color> colors)
        {
            var array = colors as Color[] ?? colors.ToArray();
            return array.ToArray()[UnityEngine.Random.Range(0,array.Count())];
        }


        //---------------------------------------------------------------------------------
        
        public static readonly Color[] ColorContainer = new Color[] {Color.red,Color.green,Color.magenta,Color.yellow,Color.white,Color.blue, Color.cyan};


        //---------------------------------------------------------------------------------
        public static Color Random(this Color color, int i)
        {
            return ColorContainer[Math.Abs(i) % ColorContainer.Length];
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////////
    public static class Color32Extention
    {
        //---------------------------------------------------------------------------------
        public static bool CompareBase(this Color32 colorA, Color colorB)
        {
            colorA.a = 255;
            colorB.a = 255;
            return colorA==colorB;
        }
    }



    ////////////////////////////////////////////////////////////////////////////////////////
    public static class ListExtention
    {

        //---------------------------------------------------------------------------------
        public static T GetRandom<T>(this List<T> t)
        {
            if (t.Count > 0)
                return t[Random.Range(0,t.Count)];
            else
                return default;
        }

        //---------------------------------------------------------------------------------
        public static T GetOrder<T>(this List<T> t)
        {
            if (t.Count > 0)
            {
                T tFirst = t[0];
                t.RemoveAt(0);
                t.Add(tFirst);
                return tFirst;
            }
            else
                return default;
        }

        //---------------------------------------------------------------------------------
        public static void AddUnique<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
                list.Add(item);
        }

        //---------------------------------------------------------------------------------
        public static void AddRangeUnique<T>(this List<T> list, IEnumerable<T> items)
        {
            foreach (T item in items)
                if (!list.Contains(item))
                    list.Add(item);
        }

        //---------------------------------------------------------------------------------
        public static void RemoveRange<T>(this List<T> list, IEnumerable<T> items)
        {
            foreach (T item in items) 
                if (list.Contains(item))
                    list.Remove(item);
        }

        //---------------------------------------------------------------------------------
        public static List<T> Clone<T>(this List<T> sourceList)
        {
            List<T> targetList = new List<T>();

            foreach (T item in sourceList)
                targetList.Add(item);

            return targetList;
        }
        
        //---------------------------------------------------------------------------------
        public static void Mix<T>(this List<T> list)
        {
            List<T> mixedList = new List<T>(list);
            int n = mixedList.Count;

            // Fisher-Yates
            for (int i = 0; i < n; i++)
            {
                int randomIndex = UnityEngine.Random.Range(i, n);
                (mixedList[i], mixedList[randomIndex]) = (mixedList[randomIndex], mixedList[i]);
            }

            list = mixedList;
        }
    }



    ////////////////////////////////////////////////////////////////////////////////////////
    public static class DictionaryExtention
    {

        //---------------------------------------------------------------------------------
        public static void Edit<T1,T2>(this Dictionary<T1,T2> dictionary, T1 key, T2 value)
        {
            dictionary.Remove(key);
            dictionary.Add(key,value);
        }

        //---------------------------------------------------------------------------------
        public static KeyValuePair<T1,T2> GetRandom<T1,T2>(this Dictionary<T1,T2> dictionary)
        {
            if (dictionary.Count > 0)
                return dictionary.ElementAt(Random.Range(0,dictionary.Count));
            else
                return default;
        }
        
        //---------------------------------------------------------------------------------
        public static void AddUnique<T1,T2>(this Dictionary<T1,T2> dictionary, T1 key, T2 value)
        {
            if (dictionary.ContainsKey(key) == false)
                dictionary.Add(key,value);
        }
    }



    ////////////////////////////////////////////////////////////////////////////////////////
    [ExecuteAlways]
    public static class MonoBehaviourExtention
    {
        
        //---------------------------------------------------------------------------------
        [ExecuteAlways]
        public static Action<Transform> OnChangedPositionOfObjectWithMouse;
        
        
        
        //---------------------------------------------------------------------------------
        private static Dictionary<string, Coroutine> coroutines = new Dictionary<string, Coroutine>();

        public static Coroutine DelayedCall(this MonoBehaviour behaviour, float delay, System.Action action)
        {
            Coroutine coroutine = behaviour.StartCoroutine(DelayedCallCoroutine(delay, action));
            return coroutine;
        }

        public static void KillDelayedCall(this MonoBehaviour behaviour, string id)
        {
            if (coroutines.TryGetValue(id, out Coroutine coroutine))
            {
                behaviour.StopCoroutine(coroutine);
                coroutines.Remove(id);
            }
        }

        public static Coroutine SetId(this Coroutine coroutine, string id)
        {
            coroutines[id] = coroutine;
            return coroutine;
        }

        private static IEnumerator DelayedCallCoroutine(float delay, System.Action action)
        {
            yield return new WaitForSeconds(delay);
            action();
        }
        
        
        //---------------------------------------------------------------------------------
    }



    ////////////////////////////////////////////////////////////////////////////////////////
    public static class ValueTypeExtention
    {

        //---------------------------------------------------------------------------------
        //ItemType type = EnumExtensions.GetRandom<ItemType>();
        public static T GetRandom<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }
    }



    ////////////////////////////////////////////////////////////////////////////////////////
    public static class MeshExtention
    {

        //--------------------------------------------------------------------------
        public static float Volume(this Mesh mesh)
        {
            if (!mesh)
                return 0;
                
            float volume = 0;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                Vector3 p1 = vertices[triangles[i + 0]];
                Vector3 p2 = vertices[triangles[i + 1]];
                Vector3 p3 = vertices[triangles[i + 2]];
                volume += SignedVolumeOfTriangle(p1, p2, p3);
            }
            return Mathf.Abs(volume);
        }
        

        //--------------------------------------------------------------------------
        private static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float v321 = p3.x * p2.y * p1.z;
            float v231 = p2.x * p3.y * p1.z;
            float v312 = p3.x * p1.y * p2.z;
            float v132 = p1.x * p3.y * p2.z;
            float v213 = p2.x * p1.y * p3.z;
            float v123 = p1.x * p2.y * p3.z;
            return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
        }
        

        //--------------------------------------------------------------------------
        public static List<int> GetNeighbourVertices(this Mesh mesh, int vertexIndex)
        {
            List<int> neighbours = new List<int>();
        
            if (vertexIndex < 0 || vertexIndex >= mesh.vertexCount)
            {
                Debug.LogError("Invalid vertex index!");
                return neighbours;
            }

            int[] triangles = mesh.triangles;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                if (triangles[i] == vertexIndex || triangles[i + 1] == vertexIndex || triangles[i + 2] == vertexIndex)
                {
                    int v0 = triangles[i];
                    int v1 = triangles[i + 1];
                    int v2 = triangles[i + 2];

                    if (v0 != vertexIndex && !neighbours.Contains(v0))
                        neighbours.Add(v0);
                    if (v1 != vertexIndex && !neighbours.Contains(v1))
                        neighbours.Add(v1);
                    if (v2 != vertexIndex && !neighbours.Contains(v2))
                        neighbours.Add(v2);
                }
            }

            return neighbours;
        }
        
    }



    ////////////////////////////////////////////////////////////////////////////////////////
    public static class Vector3Extensions
    {
        public static Vector3 Pow(this Vector3 vector, float power)
        {
            //return new Vector3(Mathf.Pow(vector.x, power), Mathf.Pow(vector.y, power), Mathf.Pow(vector.z, power));
            return Mathf.Pow(vector.magnitude, power) * vector.normalized ;
        }
        
        public static Vector3 Multiply(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        
        public static Vector3 Divide(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }
        
        public static Vector3 RandomInRange(this Vector3 value)
        {
            var x = Random.Range(-value.x,value.x);
            var y = Random.Range(-value.y,value.y);
            var z = Random.Range(-value.z,value.z);
            
            return new Vector3(x,y,z);
        }
        public static Vector3 Clamp(this Vector3 vector, Vector3 min, Vector3 max)
        {
            float x = Mathf.Clamp(vector.x, min.x, max.x);
            float y = Mathf.Clamp(vector.y, min.y, max.y);
            float z = Mathf.Clamp(vector.z, min.z, max.z);

            return new Vector3(x, y, z);
        }
    }



    ////////////////////////////////////////////////////////////////////////////////////////
    /// QuaternionExtensions
    [Flags] public enum FixedAxis
    {
        None = 0,  
        X = 1 << 0,
        Y = 1 << 1,
        Z = 1 << 2,
        W = 1 << 3
    }
    public static class QuaternionExtensions
    {
        public static Quaternion Random(this Quaternion quaternion, FixedAxis fixedAxis = FixedAxis.None)
        {
            float x = 0;
            float y = 0;
            float z = 0;
            float w = 0;
            
            if ((fixedAxis & FixedAxis.X) == FixedAxis.X)
                x = UnityEngine.Random.Range(-1f, 1f);
            if ((fixedAxis & FixedAxis.Y) == FixedAxis.Y)
                y = UnityEngine.Random.Range(-1f, 1f);
            if ((fixedAxis & FixedAxis.Z) == FixedAxis.Z)
                z = UnityEngine.Random.Range(-1f, 1f);
            if ((fixedAxis & FixedAxis.W) == FixedAxis.W)
                w = UnityEngine.Random.Range(-1f, 1f);

            return new Quaternion(x, y, z, w).normalized;
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////////
    /// StringExtensions
    public enum SanitizeType{Default, Email, Password}
    public static class StringExtensions
    {
        private const string Default = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        private const string Email = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        private const string Password = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";


        /// <summary>
        /// Sanitize user port InputField box allowing only alphanumerics and '.'
        /// </summary>
        /// <param name="dirtyString"> string to sanitize. </param>
        /// <param name="type">Type : Default, Email, Password</param>
        /// <returns> Sanitized text string. </returns>
        public static string Sanitize(this string dirtyString, SanitizeType type = SanitizeType.Default)
        {
            string pattern = type switch
            {
                SanitizeType.Default => pattern = Default,
                SanitizeType.Email => pattern = Default,
                SanitizeType.Password => pattern = Default,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            return Regex.Replace(dirtyString, pattern, "");
        }
    }


    #endregion ---------------------------------------------------------------------
    
    
    
    
    
    
    #region ####################       Attributes        ###########################
    

    ////////////////////////////        TagField         ///////////////////////////////////
    [AttributeUsage(AttributeTargets.Field)]
    public class TagFieldAttribute : PropertyAttribute { }


    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TagFieldAttribute))]
    public class TagFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String && property.propertyType != SerializedPropertyType.Integer)
            {
                // If the property is not a string, display an error message in the inspector
                EditorGUI.HelpBox(position, "TagField attribute can only be applied to string or int fields.", MessageType.Error);
                return;
            }

            bool isString = property.propertyType == SerializedPropertyType.String;

            EditorGUI.BeginProperty(position, label, property);

            // Draw the tag popup field using the InternalEditorUtility.tags array
            var tagNames = UnityEditorInternal.InternalEditorUtility.tags;
            var selectedIndex = isString ? GetSelectedIndex(property.stringValue, tagNames) : property.intValue;
            var newSelectedIndex = EditorGUI.Popup(position, label.ToString(), selectedIndex, tagNames);

            if (newSelectedIndex != selectedIndex)
                if (isString)
                    property.stringValue = tagNames[newSelectedIndex];
                else
                    property.intValue = newSelectedIndex;
            
            EditorGUI.EndProperty();
        }

        private static int GetSelectedIndex(string selectedTag, IReadOnlyList<string> tagNames)
        {
            for (var i = 0; i < tagNames.Count; i++)
                if (tagNames[i] == selectedTag)
                    return i;

            return 0;
        }
    }
    #endif
    
    
    ////////////////////////////////////////////////////////////////////////////////////////

    
    #endregion ---------------------------------------------------------------------







    #region ####################       General Utilities        ####################

    public static class Utils
    {

        //--------------------------------------------------------------------------
        /// <summary>
        /// Get GameObject which raycast hitted with screen position from main camera.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns>If hit any object at scene, returns GameObject; else Null.</returns>
        public static GameObject ScreenToObject(Vector3 screenPosition)
        {
            return ScreenToObject(screenPosition, Camera.main);
        }


        //--------------------------------------------------------------------------
        /// <summary>
        /// Get GameObject which raycast hitted with screen position.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <param name="worldCamera"></param>
        /// <returns>If hit any object at scene returns GameObject, else Null.</returns>
        public static GameObject ScreenToObject(Vector3 screenPosition, Camera worldCamera)
        {
            Ray ray = worldCamera.ScreenPointToRay(screenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit)) return hit.collider.gameObject;
            else return null;
        }


        //--------------------------------------------------------------------------
        /// <summary>
        /// Get All GameObjects which raycast hitted with screen position.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <param name="maxDistance"></param>
        /// <returns>Returns all GameObject in range, else empty list.</returns>
        public static List<GameObject> ScreenToAllObjects(Vector3 screenPosition, float maxDistance)
        {
            List<GameObject> lists = new List<GameObject>();

            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        
            foreach (RaycastHit hit in Physics.RaycastAll(ray, maxDistance)) 
                lists.Add(hit.collider.gameObject);

            return lists;
        }


        //--------------------------------------------------------------------------
        /// <summary>
        /// Get RaycastHit which raycast hitted with screen position from main camera.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns>If hit any object at scene returns RaycastHit, else Null.</returns>
        public static RaycastHit ScreenToRay(Vector3 screenPosition)
        {
            return ScreenToRay(screenPosition, Camera.main);
        }


        //--------------------------------------------------------------------------
        /// <summary>
        /// Get RaycastHit which raycast hitted with screen position.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <param name="worldCamera"></param>
        /// <returns>If hit any object at scene returns RaycastHit, else Null.</returns>
        public static RaycastHit ScreenToRay(Vector3 screenPosition, Camera worldCamera)
        {
            Ray ray = worldCamera.ScreenPointToRay(screenPosition);
            Physics.Raycast(ray, out RaycastHit hit);
            return hit;
        }
    

        //--------------------------------------------------------------------------
        /// <summary>
        /// Get Input Position in World Depends on Main Camera.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns>Input Position in World. </returns>
        public static Vector3 ScreenToWorldPositionWithoutY(Vector3 screenPosition, float distance = 0, float returnY = 0)
        {
            Vector3 targetPos = ScreenToWorldPosition(screenPosition, Camera.main, distance);
            return new Vector3(targetPos.x, returnY, targetPos.z);
        }

    
        //--------------------------------------------------------------------------
        /// <summary>
        /// Get Input Position in World Depends on Main Camera.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns>Input Position in World. </returns>
        public static Vector3 ScreenToWorldPositionWithoutZ(Vector3 screenPosition, float distance = 0, float returnZ = 0)
        {
            Vector3 targetPos = ScreenToWorldPosition(screenPosition, Camera.main, distance);
            return new Vector3(targetPos.x, targetPos.y, returnZ);
        }


        //--------------------------------------------------------------------------
        /// <summary>
        /// Get Input Position in World Depends on Main Camera.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns>Input Position in World. </returns>
        public static Vector3 ScreenToWorldPosition(Vector3 screenPosition, float distance = 0)
        {
            return ScreenToWorldPosition(screenPosition, Camera.main, distance);
        }


        //--------------------------------------------------------------------------
        public static Vector3 ScreenToWorldPosition(Vector3 screenPosition, Camera worldCamera, float distance = 0)
        {
            screenPosition.z = worldCamera.nearClipPlane + distance;
            return worldCamera.ScreenToWorldPoint(screenPosition);
        }


        //--------------------------------------------------------------------------
        public static Vector3 ScreenToWorldPositionOnLayer(Vector2 screenPosition, LayerMask layerMask, float distance = 100f)
        {
            Ray ray =  Camera.main.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, distance, layerMask))
                return hit.point;

            return default;
        }


        //--------------------------------------------------------------------------
        /// <summary>
        /// Get normalized Vector3 direction value from between two position.
        /// </summary>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <returns>Return normalized Vector3 value.</returns>
        public static Vector3 GetDirection(Vector3 fromPosition, Vector3 toPosition)
        {
            return (toPosition - fromPosition).normalized;
        }
        

        //---------------------------------------------------------------------------------
        public static int GetListenerNumber(this UnityEventBase unityEvent)
        {
            var field = typeof(UnityEventBase).GetField("m_Calls", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly );
            var invokeCallList = field.GetValue(unityEvent);
            var property = invokeCallList.GetType().GetProperty("Count");
            return (int)property.GetValue(invokeCallList);
        }
        

        //---------------------------------------------------------------------------------
        public class ThresholdController<T> where T : System.IComparable<T>
        {
            private T value;
            private float elapsedTime;

            public bool CheckThreshold(T threshold, float limit)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= limit)
                {
                    value = IncrementValue();
                    elapsedTime = 0f;
                }

                if (value.CompareTo(threshold) >= 0)
                {
                    Debug.Log("Değer eşik değerini aştı!");
                    return true;
                }

                return false;
            }

            private T IncrementValue()
            {
                // Burada value değerini nasıl artıracağınıza dair mantığı uygulayın.
                // Örneğin, eğer T bir sayı türü ise, value += increment gibi bir işlem yapabilirsiniz.

                return value;
            }
        }
        
        
    }

    #endregion ---------------------------------------------------------------------







    #region ####################       Debug Utilities        ####################
#if UNITY_EDITOR
    public static class DebugTools
    {


        //---------------------------------------------------------------------------------
        private static Vector3 TransformByPixel(Vector3 position, float x, float y) =>
            TransformByPixel(position, new Vector3(x, y));
        
        private static Vector3 TransformByPixel(Vector3 position, Vector3 translateBy) 
        {
            var view = SceneView.currentDrawingSceneView;
            
            Camera cam = view.camera;
            if (cam)
                return cam.ScreenToWorldPoint(cam.WorldToScreenPoint(position) + translateBy);

            return position;
        }

        

        //---------------------------------------------------------------------------------
        static readonly Vector4[] s_NdcFrustum = {
            new Vector4(-1, 1,  -1, 1),
            new Vector4(1, 1,  -1, 1),
            new Vector4(1, -1, -1, 1),
            new Vector4(-1, -1, -1, 1),

            new Vector4(-1, 1,  1, 1),
            new Vector4(1, 1,  1, 1),
            new Vector4(1, -1, 1, 1),
            new Vector4(-1, -1, 1, 1)
        };
        
        // Cube with edge of length 1
        private static readonly Vector4[] s_UnitCube = {
            new Vector4(-0.5f,  0.5f, -0.5f, 1),
            new Vector4(0.5f,  0.5f, -0.5f, 1),
            new Vector4(0.5f, -0.5f, -0.5f, 1),
            new Vector4(-0.5f, -0.5f, -0.5f, 1),

            new Vector4(-0.5f,  0.5f,  0.5f, 1),
            new Vector4(0.5f,  0.5f,  0.5f, 1),
            new Vector4(0.5f, -0.5f,  0.5f, 1),
            new Vector4(-0.5f, -0.5f,  0.5f, 1)
        };

        // Sphere with radius of 1
        private static readonly Vector4[] s_UnitSphere = MakeUnitSphere(16);

        // Square with edge of length 1
        private static readonly Vector4[] s_UnitSquare = {
            new Vector4(-0.5f, 0.5f, 0, 1),
            new Vector4(0.5f, 0.5f, 0, 1),
            new Vector4(0.5f, -0.5f, 0, 1),
            new Vector4(-0.5f, -0.5f, 0, 1),
        };

        private static Vector4[] MakeUnitSphere(int len)
        {
            Debug.Assert(len > 2);
            var v = new Vector4[len * 3];
            for (int i = 0; i < len; i++)
            {
                var f = i / (float)len;
                float c = Mathf.Cos(f * (float)(Math.PI * 2.0));
                float s = Mathf.Sin(f * (float)(Math.PI * 2.0));
                v[0 * len + i] = new Vector4(c, s, 0, 1);
                v[1 * len + i] = new Vector4(0, c, s, 1);
                v[2 * len + i] = new Vector4(s, 0, c, 1);
            }
            return v;
        }


        //---------------------------------------------------------------------------------
        /// <summary>
        /// This method draws a text label at a given world position in the Unity Editor with an optional color and offset.
        /// </summary>
        /// <param name="text">The string of text to display.</param>
        /// <param name="worldPos">The position in world space where the label should be drawn.</param>
        /// <param name="oX">(Optional) The horizontal offset of the label in pixels.</param>
        /// <param name="oY">(Optional) The vertical offset of the label in pixels.</param>
        /// <param name="colour">(Optional) The color of the label. If null, the label will be drawn in the default color.</param>
        public static void DrawLabel(string text, Vector3 worldPos, float oX = 0, float oY = 0, Color? colour = null) 
        {
            Handles.BeginGUI();
 
            var restoreColor = GUI.color;
 
            if (colour.HasValue) 
                GUI.color = colour.Value;
            var view = SceneView.currentDrawingSceneView;
            
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
 
            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0) 
            {
                GUI.color = restoreColor;
                Handles.EndGUI();
                return;
            }
 
            Handles.Label(TransformByPixel(worldPos, oX, oY), text);
            
            GUI.color = restoreColor;
            Handles.EndGUI();
        }


        //---------------------------------------------------------------------------------
        public static void DrawFrustum(Matrix4x4 projMatrix) { DrawFrustum(projMatrix, Color.red, Color.magenta, Color.blue); }
        public static void DrawFrustum(Matrix4x4 projMatrix, Color near, Color edge, Color far)
        {
            Vector4[] v = new Vector4[s_NdcFrustum.Length];
            Matrix4x4 m = projMatrix.inverse;

            for (int i = 0; i < s_NdcFrustum.Length; i++)
            {
                var s = m * s_NdcFrustum[i];
                v[i] = s / s.w;
            }

            // Near
            for (int i = 0; i < 4; i++)
            {
                var s = v[i];
                var e = v[(i + 1) % 4];
                Debug.DrawLine(s, e, near);
            }
            // Far
            for (int i = 0; i < 4; i++)
            {
                var s = v[4 + i];
                var e = v[4 + ((i + 1) % 4)];
                Debug.DrawLine(s, e, far);
            }
            // Middle
            for (int i = 0; i < 4; i++)
            {
                var s = v[i];
                var e = v[i + 4];
                Debug.DrawLine(s, e, edge);
            }
        }
        

        //---------------------------------------------------------------------------------
        public static void DrawFrustumSplits(Matrix4x4 projMatrix, float splitMaxPct, Vector3 splitPct, int splitStart, int splitCount, Color color)
        {
            Vector4[] v = s_NdcFrustum;
            Matrix4x4 m = projMatrix.inverse;

            // Compute camera frustum
            Vector4[] f = new Vector4[s_NdcFrustum.Length];
            for (int i = 0; i < s_NdcFrustum.Length; i++)
            {
                var s = m * v[i];
                f[i] = s / s.w;
            }

            // Compute shadow far plane/quad
            Vector4[] qMax = new Vector4[4];
            for (int i = 0; i < 4; i++)
            {
                qMax[i] = Vector4.Lerp(f[i], f[4 + i], splitMaxPct);
            }

            // Draw Shadow far/max quad
            for (int i = 0; i < 4; i++)
            {
                var s = qMax[i];
                var e = qMax[(i + 1) % 4];
                Debug.DrawLine(s, e, Color.black);
            }

            // Compute split quad (between near/shadow far)
            Vector4[] q = new Vector4[4];
            for (int j = splitStart; j < splitCount; j++)
            {
                float d = splitPct[j];
                for (int i = 0; i < 4; i++)
                {
                    q[i] = Vector4.Lerp(f[i], qMax[i], d);
                }

                // Draw
                for (int i = 0; i < 4; i++)
                {
                    var s = q[i];
                    var e = q[(i + 1) % 4];
                    Debug.DrawLine(s, e, color);
                }
            }
        }


        //---------------------------------------------------------------------------------
        public static void DrawBox(Vector4 pos, Vector3 size, Color color)
        {
            Vector4[] v = s_UnitCube;
            Vector4 sz = new Vector4(size.x, size.y, size.z, 1);
            for (int i = 0; i < 4; i++)
            {
                var s = pos + Vector4.Scale(v[i], sz);
                var e = pos + Vector4.Scale(v[(i + 1) % 4], sz);
                Debug.DrawLine(s , e , color);
            }
            for (int i = 0; i < 4; i++)
            {
                var s = pos + Vector4.Scale(v[4 + i], sz);
                var e = pos + Vector4.Scale(v[4 + ((i + 1) % 4)], sz);
                Debug.DrawLine(s , e , color);
            }
            for (int i = 0; i < 4; i++)
            {
                var s = pos + Vector4.Scale(v[i], sz);
                var e = pos + Vector4.Scale(v[i + 4], sz);
                Debug.DrawLine(s , e , color);
            }
        }

        public static void DrawBox(Matrix4x4 transform, Color color)
        {
            Vector4[] v = s_UnitCube;
            Matrix4x4 m = transform;
            for (int i = 0; i < 4; i++)
            {
                var s = m * v[i];
                var e = m * v[(i + 1) % 4];
                Debug.DrawLine(s , e , color);
            }
            for (int i = 0; i < 4; i++)
            {
                var s = m * v[4 + i];
                var e = m * v[4 + ((i + 1) % 4)];
                Debug.DrawLine(s , e , color);
            }
            for (int i = 0; i < 4; i++)
            {
                var s = m * v[i];
                var e = m * v[i + 4];
                Debug.DrawLine(s , e , color);
            }
        }
        

        //---------------------------------------------------------------------------------
        public static void DrawSphere(Vector3 pos, float radius, Color color) 
            => DrawSphere(new Vector4(pos.x, pos.y, pos.z, 1f),radius,color);
        
        public static void DrawSphere(Vector4 pos, float radius, Color color)
        {
            Vector4[] v = s_UnitSphere;
            int len = s_UnitSphere.Length / 3;
            for (int i = 0; i < len; i++)
            {
                var sX = pos + radius * v[0 * len + i];
                var eX = pos + radius * v[0 * len + (i + 1) % len];
                var sY = pos + radius * v[1 * len + i];
                var eY = pos + radius * v[1 * len + (i + 1) % len];
                var sZ = pos + radius * v[2 * len + i];
                var eZ = pos + radius * v[2 * len + (i + 1) % len];
                Debug.DrawLine(sX, eX, color);
                Debug.DrawLine(sY, eY, color);
                Debug.DrawLine(sZ, eZ, color);
            }
        }

        
        //---------------------------------------------------------------------------------
        public static void DrawPoint(Vector4 pos, float scale, Color color)
        {
            var sX = pos + new Vector4(+scale, 0, 0);
            var eX = pos + new Vector4(-scale, 0, 0);
            var sY = pos + new Vector4(0, +scale, 0);
            var eY = pos + new Vector4(0, -scale, 0);
            var sZ = pos + new Vector4(0, 0, +scale);
            var eZ = pos + new Vector4(0, 0, -scale);
            Debug.DrawLine(sX , eX , color);
            Debug.DrawLine(sY , eY , color);
            Debug.DrawLine(sZ , eZ , color);
        }


        //---------------------------------------------------------------------------------
        public static void DrawAxes(Vector4 pos, float scale = 1.0f)
        {
            Debug.DrawLine(pos, pos + new Vector4(scale, 0, 0), Color.red);
            Debug.DrawLine(pos, pos + new Vector4(0, scale, 0), Color.green);
            Debug.DrawLine(pos, pos + new Vector4(0, 0, scale), Color.blue);
        }

        public static void DrawAxes(Matrix4x4 transform, float scale = 1.0f)
        {
            Vector4 p = transform * new Vector4(0, 0, 0, 1);
            Vector4 x = transform * new Vector4(scale, 0, 0, 1);
            Vector4 y = transform * new Vector4(0, scale, 0, 1);
            Vector4 z = transform * new Vector4(0, 0, scale, 1);

            Debug.DrawLine(p, x, Color.red);
            Debug.DrawLine(p, y, Color.green);
            Debug.DrawLine(p, z, Color.blue);
        }


        //---------------------------------------------------------------------------------
        public static void DrawQuad(Matrix4x4 transform, Color color)
        {
            Vector4[] v = s_UnitSquare;
            Matrix4x4 m = transform;
            for (int i = 0; i < 4; i++)
            {
                var s = m * v[i];
                var e = m * v[(i + 1) % 4];
                Debug.DrawLine(s , e , color);
            }
        }


        //---------------------------------------------------------------------------------
        public static void DrawPlane(Plane plane, float scale, Color edgeColor, float normalScale, Color normalColor)
        {
            // Flip plane distance: Unity Plane distance is from plane to origin
            DrawPlane(new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, -plane.distance), scale, edgeColor, normalScale, normalColor);
        }

        public static void DrawPlane(Vector4 plane, float scale, Color edgeColor, float normalScale, Color normalColor)
        {
            Vector3 n = Vector3.Normalize(plane);
            float   d = plane.w;

            Vector3 u = Vector3.up;
            Vector3 r = Vector3.right;
            if (n == u)
                u = r;

            r = Vector3.Cross(n, u);
            u = Vector3.Cross(n, r);

            for (int i = 0; i < 4; i++)
            {
                var s = scale * s_UnitSquare[i];
                var e = scale * s_UnitSquare[(i + 1) % 4];
                s = s.x * r + s.y * u + n * d;
                e = e.x * r + e.y * u + n * d;
                Debug.DrawLine(s, e, edgeColor);
            }

            // Diagonals
            {
                var s = scale * s_UnitSquare[0];
                var e = scale * s_UnitSquare[2];
                s = s.x * r + s.y * u + n * d;
                e = e.x * r + e.y * u + n * d;
                Debug.DrawLine(s, e, edgeColor);
            }
            {
                var s = scale * s_UnitSquare[1];
                var e = scale * s_UnitSquare[3];
                s = s.x * r + s.y * u + n * d;
                e = e.x * r + e.y * u + n * d;
                Debug.DrawLine(s, e, edgeColor);
            }

            Debug.DrawLine(n * d, n * (d + 1 * normalScale), normalColor);
        }


        //---------------------------------------------------------------------------------
    }
    
#endif
    #endregion ---------------------------------------------------------------------







    #region ####################       Editor Utilities        ####################
#if UNITY_EDITOR
    
    [InitializeOnLoad]
    public class ObjectPositionChanged
    {
        static ObjectPositionChanged()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        static void OnSceneGUI(SceneView sceneView)
        {
            if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
            {
                if (Selection.activeTransform != null)
                {
                    //Debug.Log("Changed position of object: " + Selection.activeTransform.position);
                    MonoBehaviourExtention.OnChangedPositionOfObjectWithMouse?.Invoke(Selection.activeTransform);
                }
            }
        }
    }
    

#endif
    #endregion ---------------------------------------------------------------------
}