using System;
using System.Threading;
using JetBrains.Annotations;
using Unity.Cloud.Collaborate.Assets;
using Unity.Cloud.Collaborate.UserInterface;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Cloud.Collaborate.Components
{
    class SearchBar : VisualElement
    {
        public const string UssClassName = "unity-search-bar";
        public const string SearchFieldUssClassName = UssClassName + "__search-field";
        public const str