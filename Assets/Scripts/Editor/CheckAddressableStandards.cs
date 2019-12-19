using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Build.AnalyzeRules;
using UnityEditor.AddressableAssets.GUI;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using UnityEngine.Video;

namespace TheGamadevGuru
{
    public class CheckVideoCompression : AnalyzeRule
    {
        public override bool CanFix { get { return false; } }
        public override string ruleName { get { return "CheckVideoCompression"; } }
        
        public override List<AnalyzeResult> RefreshAnalysis(AddressableAssetSettings aaSettings)
        {
            var analyzeResults = new List<AnalyzeResult>();
            foreach (var group in aaSettings.groups)
            {
                foreach (var entry in group.entries)
                {
                    if (entry.MainAsset == null)
                        continue;
                    var assetType = entry.MainAsset.GetType();
                    if (assetType == typeof(VideoClip))
                    {
                        var bundledAssetGroupSchema = entry.parentGroup.GetSchema<BundledAssetGroupSchema>();
                        if (bundledAssetGroupSchema.Compression != BundledAssetGroupSchema.BundleCompressionMode.Uncompressed)
                        {
                            analyzeResults.Add(new AnalyzeResult {resultName = $"Video' '{entry.address}' should not be in compressed asset bundle"});
                        }
                    }
                }
            }

            return analyzeResults;
        }
    }
    
    public class CheckVideoPath : AnalyzeRule
    {
        public override bool CanFix { get { return false; } }
        public override string ruleName { get { return "CheckVideoPath"; } }
        
        public override List<AnalyzeResult> RefreshAnalysis(AddressableAssetSettings aaSettings)
        {
            var analyzeResults = new List<AnalyzeResult>();
            foreach (var group in aaSettings.groups)
            {
                foreach (var entry in group.entries)
                {
                    if (entry.MainAsset == null)
                        continue;
                    var assetType = entry.MainAsset.GetType();
                    if (assetType == typeof(VideoClip))
                    {
                        if (entry.AssetPath.Contains("/Videos/") == false)
                        {
                            analyzeResults.Add(new AnalyzeResult {resultName = $"Video '{entry.address}' should be within a Videos directory"});
                        }
                    }
                }
            }

            return analyzeResults;
        }
    }
    
    public class CheckVideoLabel : AnalyzeRule
    {
        public override bool CanFix { get { return true; } }
        public override string ruleName { get { return "CheckVideoLabel"; } }
        private List<AddressableAssetEntry> _videosToLabel = null;
        
        public override List<AnalyzeResult> RefreshAnalysis(AddressableAssetSettings aaSettings)
        {
            _videosToLabel = new List<AddressableAssetEntry>();
            var analyzeResults = new List<AnalyzeResult>();
            foreach (var group in aaSettings.groups)
            {
                foreach (var entry in group.entries)
                {
                    if (entry.MainAsset == null)
                        continue;
                    var assetType = entry.MainAsset.GetType();
                    if (assetType == typeof(VideoClip))
                    {
                        if (entry.labels.Contains("Video") == false)
                        {
                            analyzeResults.Add(new AnalyzeResult {resultName = $"Video '{entry.address}' should have a Video label"});
                            _videosToLabel.Add(entry);
                        }
                    }
                }
            }

            return analyzeResults;
        }

        public override void FixIssues(AddressableAssetSettings aaSettings)
        {
            foreach (var entry in _videosToLabel)
            {
                entry.SetLabel("Video", true, true);
            }

            _videosToLabel = null;
        }

        public override void ClearAnalysis()
        {
            base.ClearAnalysis();
            _videosToLabel = null;
        }
    }
    
    public class CheckPrefabAddress : AnalyzeRule
    {
        public override bool CanFix { get { return true; } }
        public override string ruleName { get { return "CheckPrefabAddress"; } }
        private List<AddressableAssetEntry> _prefabAddressesToRename = null;

        public override List<AnalyzeResult> RefreshAnalysis(AddressableAssetSettings aaSettings)
        {
            _prefabAddressesToRename = new List<AddressableAssetEntry>();
            var analyzeResults = new List<AnalyzeResult>();
            foreach (var group in aaSettings.groups)
            {
                foreach (var entry in group.entries)
                {
                    if (entry.MainAsset == null)
                        continue;
                    var assetType = entry.MainAsset.GetType();
                    if (assetType == typeof(GameObject))
                    {
                        var requiredAddress = GetSuggestedAddressForAsset(entry);
                        if (entry.address != requiredAddress)
                        {
                            analyzeResults.Add(new AnalyzeResult {resultName = $"Video '{entry.address}' should have address '{requiredAddress}'"});
                            _prefabAddressesToRename.Add(entry);
                        }
                    }
                }
            }

            return analyzeResults;
        }


        public override void FixIssues(AddressableAssetSettings aaSettings)
        {
            foreach (var entry in _prefabAddressesToRename)
            {
                var requiredAddress = GetSuggestedAddressForAsset(entry);
                entry.SetAddress(requiredAddress);
            }

            _prefabAddressesToRename = null;
        }
        
        private string GetSuggestedAddressForAsset(AddressableAssetEntry addressableAssetEntry)
        {
            return addressableAssetEntry.MainAsset.name;
        }

        public override void ClearAnalysis()
        {
            base.ClearAnalysis();
            _prefabAddressesToRename = null;
        }
    }

    [InitializeOnLoad]
    class RegisterCustomAnalyzeRules
    {
        static RegisterCustomAnalyzeRules()
        {
            AnalyzeWindow.RegisterNewRule<CheckVideoCompression>();
            AnalyzeWindow.RegisterNewRule<CheckVideoPath>();
            AnalyzeWindow.RegisterNewRule<CheckVideoLabel>();
            AnalyzeWindow.RegisterNewRule<CheckPrefabAddress>();
        }
    }
}