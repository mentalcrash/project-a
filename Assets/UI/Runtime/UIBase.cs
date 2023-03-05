using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using MC.Project.UI.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace MC.Project.UI
{
    public abstract class UIBase<T> : UIBase
    {
        protected T data;
    }

    public abstract class UIBase : MonoBehaviour
    {
        private readonly Dictionary<string, Component[]> _cachedComponents = new();

        public void Initialize()
        {
            var root = gameObject.name.Replace("(Clone)", "");
            CacheComponents(root, transform);

            Debug.Log(string.Join("\n", _cachedComponents.Select(kv =>
                $"{kv.Key}:\t {string.Join(", ", kv.Value.Select(v => v))}")));

            FillFields();
            FillMethods();
            
            OnInitialize();
        }

        protected abstract void OnInitialize();

        protected abstract void OnChangedData();

        protected abstract void OnShow();

        protected abstract void OnHide();

        protected abstract void OnRelease();

        private void CacheComponents(string path, Transform target)
        {
            var components = target.GetComponents<Component>();
            var pathHash = CalculateMD5Hash(path);

            if (_cachedComponents.ContainsKey(pathHash))
                throw new UIException(ErrorCode.DuplicatePath, path);

            _cachedComponents.Add(pathHash, components);

            foreach (Transform child in target)
            {
                var childPath = Path.Combine(path, child.name);
                CacheComponents(childPath, child);
            }
        }

        private void FillFields()
        {
            var type = GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (field.IsDefined(typeof(UIFieldAttribute)))
                {
                    SetField(field);
                }
            }
        }

        private void SetField(FieldInfo field)
        {
            var fieldAttribute = field.GetCustomAttribute<UIFieldAttribute>();

            var fieldType = field.FieldType;
            var path = fieldAttribute.path;

            var hashedPath = CalculateMD5Hash(path);
            if (_cachedComponents.ContainsKey(hashedPath) == false)
                throw new UIException(ErrorCode.InvalidPath, path);

            var components = _cachedComponents[hashedPath];
            var comps = components.Where(comp => comp.GetType() == fieldType).ToArray();
            if (comps == null)
                throw new UIException(ErrorCode.NoComponentInPath, $"path: {path}, component type: {fieldType}");
            
            if (comps.Count() > 1)
                throw new UIException(ErrorCode.DuplicateComponent, $"path: {path}, component type: {fieldType}");

            field.SetValue(this, comps[0]);
        }
        
        private void FillMethods()
        {
            var type = GetType();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                if (method.IsDefined(typeof(UIButtonAttribute)))
                {
                    var button = FindComponent<Button>(method);
                    button.onClick.AddListener(() => method.Invoke(this, null));
                }
            }
        }

        private T FindComponent<T>(MethodInfo method) where T : Component
        {
            var buttonAttribute = method.GetCustomAttribute<UIButtonAttribute>();
            var path = buttonAttribute.path;
            var targetType = buttonAttribute.targetType;

            var hashedPath = CalculateMD5Hash(path);
            if (_cachedComponents.ContainsKey(hashedPath) == false)
                throw new UIException(ErrorCode.InvalidPath, path);

            var components = _cachedComponents[hashedPath];
            var comps = components.Where(comp => comp.GetType() == targetType).ToArray();
            if (comps == null)
                throw new UIException(ErrorCode.NoComponentInPath, $"path: {path}, component type: {targetType}");

            if (comps.Count() > 1)
                throw new UIException(ErrorCode.DuplicateComponent, $"path: {path}, component type: {targetType}");

            return comps[0] as T;

        }

        private static string CalculateMD5Hash(string input)
        {
            using MD5 md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var t in hashBytes)
            {
                sb.Append(t.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}