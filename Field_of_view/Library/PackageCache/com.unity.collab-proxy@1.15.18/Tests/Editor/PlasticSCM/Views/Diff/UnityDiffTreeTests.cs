using System.Collections.Generic;

using NUnit.Framework;

using Codice.Client.Commands;
using Codice.Client.Commands.Mount;
using Codice.CM.Common;
using Codice.CM.Common.Merge;
using Codice.Utils;
using PlasticGui.WorkspaceWindow.Diff;
using Unity.PlasticSCM.Editor.Views.Diff;

namespace Unity.PlasticSCM.Tests.Editor.Views.Diff
{
    [TestFixture]
    class UnityDiffTreeTests
    {
        [Test]
        public void TestAddedNoMeta()
        {
            ClientDiff added = Build.Added("/foo/bar.c");

